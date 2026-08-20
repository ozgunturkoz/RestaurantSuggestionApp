using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.Core.DTOs.Responses;

namespace RestaurantSuggestionApp.Business.ExternalServices
{
    public interface IOsrmService
    {
        Task<List<RestaurantDistance>> GetDistancesFromOsrmAsync(List<Restaurant> nearbyCandidates, double userLat, double userLng);
    }

    public class OsrmService : IOsrmService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OsrmService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<RestaurantDistance>> GetDistancesFromOsrmAsync(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var resultList = new List<RestaurantDistance>();
            try
            {
                var url = BuildOsrmUrl(nearbyCandidates, userLat, userLng);

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var osrmData = await response.Content.ReadFromJsonAsync<OsrmTableResponse>();
                    if (osrmData != null && osrmData.Code == "Ok" && osrmData.Distances != null && osrmData.Distances.Length > 0)
                    {
                        resultList = MapResponseToRestaurantDistances(osrmData, nearbyCandidates);
                    }
                }
            }
            catch (Exception)
            {
            }
            return resultList;
        }

        private string BuildOsrmUrl(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var osrmBaseUrl = _configuration["OSRM:BaseUrl"] ?? throw new InvalidOperationException("OSRM:BaseUrl konfigürasyonu bulunamadı.");

            var coords = new List<string>
            {
                $"{userLng.ToString(CultureInfo.InvariantCulture)},{userLat.ToString(CultureInfo.InvariantCulture)}"
            };

            foreach (var rest in nearbyCandidates)
            {
                coords.Add($"{rest.Longitude.ToString(CultureInfo.InvariantCulture)},{rest.Latitude.ToString(CultureInfo.InvariantCulture)}");
            }

            var coordsString = string.Join(";", coords);
            return $"{osrmBaseUrl}/table/v1/foot/{coordsString}?sources=0&annotations=distance,duration";
        }

        private static List<RestaurantDistance> MapResponseToRestaurantDistances(OsrmTableResponse osrmData, List<Restaurant> nearbyCandidates)
        {
            var resultList = new List<RestaurantDistance>();

            for (int i = 0; i < nearbyCandidates.Count; i++)
            {
                var rest = nearbyCandidates[i];
                double? walkingDistance = null;
                double? walkingDuration = null;

                if (osrmData.Distances != null && osrmData.Distances.Length > 0 && osrmData.Distances[0].Length > i + 1)
                    walkingDistance = osrmData.Distances[0][i + 1];

                if (osrmData.Durations != null && osrmData.Durations.Length > 0 && osrmData.Durations[0].Length > i + 1)
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

            return resultList;
        }
    }

    public class OsrmTableResponse
    {
        public string Code { get; set; } = string.Empty;
        public double[][] Distances { get; set; } = Array.Empty<double[]>();
        public double[][] Durations { get; set; } = Array.Empty<double[]>();

        public OsrmTableResponse() { }

        public OsrmTableResponse(string code, double[][] distances, double[][] durations)
        {
            Code = code;
            Distances = distances;
            Durations = durations;
        }
    }
}
