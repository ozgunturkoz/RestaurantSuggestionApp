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
    // =========================================================================================
    // BU ARAYÜZ (INTERFACE) NE İŞE YARAR?
    // Dış dünyadaki açık kaynaklı harita yönlendirme servisine (OSRM) bağlanıp
    // sokak sokak gerçek yürüyüş rotalarını ve dakikalarını istemek için kullanılan sözleşmedir.
    // =========================================================================================
    public interface IOsrmService
    {
        Task<List<RestaurantDistance>> GetDistancesFromOsrmAsync(List<Restaurant> nearbyCandidates, double userLat, double userLng);
    }

    // =========================================================================================
    // BU SINIF (CLASS) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Kuş uçuşu mesafe binaların, sokakların ve denizlerin üzerinden dümdüz geçer ancak insanlar sokakta yürür!
    // Bu servis, OpenStreetMap harita verilerini kullanan OSRM sunucusuna HTTP isteği atar ve
    // "Kullanıcı bu restorana yürüyerek sokaklardan kaç metrede ve kaç dakikada gider?" sorusunun
    // gerçek ve net cevabını getirir.
    // =========================================================================================
    public class OsrmService : IOsrmService
    {
        // İnternet üzerinden harita sunucusuna istek atmamızı sağlayan istemci nesnesi
        private readonly HttpClient _httpClient;

        // Harita sunucusunun web adresini appsettings.json'dan okuyan nesne
        private readonly IConfiguration _configuration;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // OsrmService ayağa kalkarken HttpClient ve ayar nesnelerini içeri alır.
        // -------------------------------------------------------------------------------------
        public OsrmService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // -------------------------------------------------------------------------------------
        // OSRM Harita Sunucusundan Yürüyüş Mesafelerini ve Sürelerini Çeken Metot
        // -------------------------------------------------------------------------------------
        public async Task<List<RestaurantDistance>> GetDistancesFromOsrmAsync(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var resultList = new List<RestaurantDistance>();
            try
            {
                // Harita sunucusu için istek adresini oluşturuyoruz
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
            catch
            {
                // Harita sunucusuna ulaşılamazsa sistem çökmez, boş liste döner ve yedek Haversine hesaplayıcısı devreye girer
            }
            return resultList;
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Kullanıcının ve restoranların GPS koordinatlarını OSRM URL formatına birleştirir
        // -------------------------------------------------------------------------------------
        private string BuildOsrmUrl(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var osrmBaseUrl = _configuration["OSRM:BaseUrl"] ?? "https://router.project-osrm.org";

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

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: OSRM'den gelen JSON matris cevabını Restoran listesine dönüştürür
        // -------------------------------------------------------------------------------------
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

    // -----------------------------------------------------------------------------------------
    // OSRM Sunucusunun Döndüğü JSON Verisini Karşılayan Model Sınıfı
    // -----------------------------------------------------------------------------------------
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
