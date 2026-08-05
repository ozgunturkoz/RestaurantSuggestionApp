using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Business.Helpers;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.DTOs.Responses;
using RestaurantSuggestionApp.Business.ExternalServices;

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
        private readonly IOsrmService _osrmService;

        public RestaurantService(AppDbContext context, IOsrmService osrmService)
        {
            _context = context;
            _osrmService = osrmService;
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

                existing.UpdateDetails(updatedRestaurant.Name, updatedRestaurant.Category, updatedRestaurant.AveragePrice);
                existing.UpdateLocation(updatedRestaurant.Latitude, updatedRestaurant.Longitude);
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

                var resultList = await _osrmService.GetDistancesFromOsrmAsync(nearbyCandidates, userLat, userLng);

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
            decimal userAverageExpenditure = 350.0m; // Varsayılan ortalama öğün tutarı
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
                    userAverageExpenditure = lastExpenditures.Average();
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

                // Basamaklı index yerine oransal öğün yakınlığı yüzdesi (0-100)
                double budgetScore = BudgetHelper.CalculateProximityScore(userAverageExpenditure, item.AveragePrice);

                double recommendationScore = (locationScore * 0.60) + (budgetScore * 0.40);

                item.LocationScore = Math.Round(locationScore, 1);
                item.BudgetScore = Math.Round(budgetScore, 1);
                item.RecommendationScore = Math.Round(recommendationScore, 1);
            }
        }
    }
}