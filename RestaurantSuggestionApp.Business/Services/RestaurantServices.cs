using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Business.Helpers;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.DTOs.Responses;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IRestaurantService
    {
        BaseResponse<List<Restaurant>> GetAll();
        BaseResponse<Restaurant> GetById(int id);
        BaseResponse<Restaurant> Add(Restaurant newRestaurant);
        BaseResponse<Restaurant> Update(int id, Restaurant updatedRestaurant);
        BaseResponse<bool> Delete(int id);

        Task<BaseResponse<List<RestaurantDistance>>> GetNearbyAsync(decimal latitude, decimal longitude, double radiusInKm, int? userId);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RestaurantService(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public BaseResponse<List<Restaurant>> GetAll()
        {
            try
            {
                var list = _context.Restaurants.ToList();
                return BaseResponse<List<Restaurant>>.Success(list);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<Restaurant>>.Fail($"Restoranlar getirilirken hata oluştu: {ex.Message}");
            }
        }
        public BaseResponse<Restaurant> GetById(int id)
        {
            try
            {
                var restaurant = GetRestaurantOrThrow(id, "Aradığınız restoran bulunamadı.");
                return BaseResponse<Restaurant>.Success(restaurant);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<Restaurant>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse<Restaurant>.Fail($"Restoran getirilirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<Restaurant> Add(Restaurant newRestaurant)
        {
            try
            {
                _context.Restaurants.Add(newRestaurant);
                _context.SaveChanges();
                return BaseResponse<Restaurant>.Success(newRestaurant);
            }
            catch (Exception ex)
            {
                return BaseResponse<Restaurant>.Fail($"Restoran eklenirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<Restaurant> Update(int id, Restaurant updatedRestaurant)
        {
            try
            {
                var existing = GetRestaurantOrThrow(id, "Güncellenecek restoran bulunamadı.");

                existing.Name = updatedRestaurant.Name;
                existing.Category = updatedRestaurant.Category;
                existing.AveragePrice = updatedRestaurant.AveragePrice;
                existing.Latitude = updatedRestaurant.Latitude;
                existing.Longitude = updatedRestaurant.Longitude;
                _context.SaveChanges();

                return BaseResponse<Restaurant>.Success(existing);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<Restaurant>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse<Restaurant>.Fail($"Restoran güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var existing = GetRestaurantOrThrow(id, "Silinecek restoran bulunamadı.");

                _context.Restaurants.Remove(existing);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Restoran silinirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<BaseResponse<List<RestaurantDistance>>> GetNearbyAsync(decimal latitude, decimal longitude, double radiusInKm, int? userId)
        {
            try
            {
                double userLat = (double)latitude;
                double userLng = (double)longitude;

                var nearbyCandidates = GetNearbyCandidates(userLat, userLng, radiusInKm);
                if (!nearbyCandidates.Any())
                {
                    return BaseResponse<List<RestaurantDistance>>.Success(new List<RestaurantDistance>());
                }

                var resultList = await GetDistancesFromOsrmAsync(nearbyCandidates, userLat, userLng);

                if (!resultList.Any())
                {
                    resultList = CalculateFallbackDistances(nearbyCandidates, userLat, userLng);
                }

                ScoreRestaurants(resultList, userId, userLat, userLng);

                return BaseResponse<List<RestaurantDistance>>.Success(
                    resultList.OrderByDescending(r => r.RecommendationScore ?? 0).ToList()
                );
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RestaurantDistance>>.Fail($"Yakın restoranlar hesaplanırken bir hata oluştu: {ex.Message}");
            }
        }

        private Restaurant GetRestaurantOrThrow(int id, string errorMessage)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                throw new ArgumentException(errorMessage);
            }
            return restaurant;
        }

        private List<Restaurant> GetNearbyCandidates(double userLat, double userLng, double radiusInKm)
        {
            double kmPerLatitudeDegree = 111.0;
            double deltaLat = radiusInKm / kmPerLatitudeDegree;

            double userLatRadians = GeoHelper.ToRadians(userLat);
            double kmPerLongitudeDegree = 111.0 * Math.Cos(userLatRadians);
            double deltaLng = radiusInKm / kmPerLongitudeDegree;

            decimal minLat = (decimal)(userLat - deltaLat);
            decimal maxLat = (decimal)(userLat + deltaLat);
            decimal minLng = (decimal)(userLng - deltaLng);
            decimal maxLng = (decimal)(userLng + deltaLng);

            var candidateRestaurants = _context.Restaurants
                .Where(r => r.Latitude >= minLat && r.Latitude <= maxLat &&
                            r.Longitude >= minLng && r.Longitude <= maxLng)
                .ToList();

            return candidateRestaurants.Where(rest =>
            {
                double distanceInKm = GeoHelper.CalculateHaversineDistance(userLat, userLng, (double)rest.Latitude, (double)rest.Longitude) / 1000.0;
                return distanceInKm <= radiusInKm;
            }).ToList();
        }

        private async Task<List<RestaurantDistance>> GetDistancesFromOsrmAsync(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var resultList = new List<RestaurantDistance>();
            try
            {
                var osrmBaseUrl = _configuration["OSRM:BaseUrl"] ?? "http://router.project-osrm.org";

                var coords = new List<string> {
                    $"{userLng.ToString(CultureInfo.InvariantCulture)},{userLat.ToString(CultureInfo.InvariantCulture)}"
                };

                foreach (var rest in nearbyCandidates)
                {
                    coords.Add($"{rest.Longitude.ToString(CultureInfo.InvariantCulture)},{rest.Latitude.ToString(CultureInfo.InvariantCulture)}");
                }

                var coordsString = string.Join(";", coords);
                var url = $"{osrmBaseUrl}/table/v1/foot/{coordsString}?sources=0&annotations=distance,duration";

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var osrmData = await response.Content.ReadFromJsonAsync<OsrmTableResponse>();
                    if (osrmData != null && osrmData.Code == "Ok" && osrmData.Distances != null && osrmData.Distances.Length > 0)
                    {
                        for (int i = 0; i < nearbyCandidates.Count; i++)
                        {
                            var rest = nearbyCandidates[i];
                            double? walkingDistance = null;
                            double? walkingDuration = null;

                            if (osrmData.Distances[0].Length > i + 1)
                                walkingDistance = osrmData.Distances[0][i + 1];

                            if (osrmData.Durations[0].Length > i + 1)
                                walkingDuration = osrmData.Durations[0][i + 1];

                            resultList.Add(new RestaurantDistance
                            {
                                Id = rest.Id,
                                Name = rest.Name,
                                Category = rest.Category,
                                Latitude = rest.Latitude,
                                Longitude = rest.Longitude,
                                AveragePrice = rest.AveragePrice,
                                WalkingDistanceInMeters = walkingDistance,
                                WalkingDurationInSeconds = walkingDuration
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                // OSRM API çökerse Fallback devreye girecek
            }
            return resultList;
        }

        private List<RestaurantDistance> CalculateFallbackDistances(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var fallbackList = new List<RestaurantDistance>();
            foreach (var rest in nearbyCandidates)
            {
                double distanceInMeters = GeoHelper.CalculateHaversineDistance(userLat, userLng, (double)rest.Latitude, (double)rest.Longitude);
                double durationInSeconds = distanceInMeters / 1.38; // ~5 km/s

                fallbackList.Add(new RestaurantDistance
                {
                    Id = rest.Id,
                    Name = rest.Name,
                    Category = rest.Category,
                    Latitude = rest.Latitude,
                    Longitude = rest.Longitude,
                    AveragePrice = rest.AveragePrice,
                    WalkingDistanceInMeters = distanceInMeters,
                    WalkingDurationInSeconds = durationInSeconds
                });
            }
            return fallbackList;
        }

        private void ScoreRestaurants(List<RestaurantDistance> resultList, int? userId, double userLat, double userLng)
        {
            int userBudgetIndex = 3; // Varsayılan orta düzey
            if (userId.HasValue)
            {
                var lastExpenditures = _context.UserExpenditures
                    .Where(ue => ue.UserId == userId.Value)
                    .OrderByDescending(ue => ue.SpentAt)
                    .Take(20)
                    .Select(ue => ue.Amount)
                    .ToList();

                if (lastExpenditures.Any())
                {
                    decimal averageExpenditure = lastExpenditures.Average();
                    userBudgetIndex = BudgetHelper.GetBudgetIndex(averageExpenditure);
                }
            }

            foreach (var item in resultList)
            {
                double distanceForScoring = item.WalkingDistanceInMeters ??
                    GeoHelper.CalculateHaversineDistance(userLat, userLng, (double)item.Latitude, (double)item.Longitude);

                double locationScore = 0;
                if (distanceForScoring <= 500)
                {
                    locationScore = 100;
                }
                else if (distanceForScoring < 5000)
                {
                    locationScore = 100.0 * (1.0 - (distanceForScoring - 500.0) / 4500.0);
                }

                int restaurantPriceIndex = BudgetHelper.GetBudgetIndex(item.AveragePrice);
                double budgetScore = 100 - (25 * Math.Abs(userBudgetIndex - restaurantPriceIndex));
                if (budgetScore < 0) budgetScore = 0;

                double recommendationScore = (locationScore * 0.60) + (budgetScore * 0.40);

                item.LocationScore = Math.Round(locationScore, 1);
                item.BudgetScore = Math.Round(budgetScore, 1);
                item.RecommendationScore = Math.Round(recommendationScore, 1);
            }
        }
    }

    public class OsrmTableResponse
    {
        public string Code { get; set; } = string.Empty;
        public double[][] Distances { get; set; } = Array.Empty<double[]>();
        public double[][] Durations { get; set; } = Array.Empty<double[]>();
    }
}