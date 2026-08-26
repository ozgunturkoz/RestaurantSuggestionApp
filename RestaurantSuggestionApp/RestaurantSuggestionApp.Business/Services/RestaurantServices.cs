using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Business.Helpers;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.DTOs.Responses;
using RestaurantSuggestionApp.Business.ExternalServices;

namespace RestaurantSuggestionApp.Business.Services
{
    // =========================================================================================
    // BU ARAYÜZ (INTERFACE) NE İŞE YARAR?
    // Restoranlar ile ilgili yapılabilecek tüm işlemleri (listeleme, ekleme, güncelleme, silme
    // ve kullanıcının konumuna/bütçesine göre en iyi restoranları hesaplama) tanımlayan sözleşmedir.
    // =========================================================================================
    public interface IRestaurantService
    {
        // Sistemdeki tüm restoranları listeler
        BaseResponse<List<Restaurant>> GetAll();

        // Belirli bir ID'ye sahip restoranın detaylarını getirir
        BaseResponse<Restaurant> GetById(int id);

        // Sisteme yeni bir restoran ekler
        BaseResponse<Restaurant> Add(Restaurant newRestaurant);

        // Var olan bir restoranın bilgilerini günceller
        BaseResponse<Restaurant> Update(int id, Restaurant updatedRestaurant);

        // Bir restoranı sistemden siler
        BaseResponse<bool> Delete(int id);

        // Kullanıcının konumuna ve bütçesine göre yakınındaki restoranları akıllı puanlayarak getirir
        Task<BaseResponse<List<RestaurantDistance>>> GetNearbyAsync(decimal latitude, decimal longitude, double radiusInKm, int? userId);
    }

    // =========================================================================================
    // BU SINIF (CLASS) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Sistemin "Restoran ve Yapay Zeka Öneri Motorudur".
    // 1. Kullanıcının haritadaki yerini alır.
    // 2. Belirlenen yarıçaptaki (örneğin 2 km) restoranları coğrafi formüllerle bulur.
    // 3. OSRM harita servisiyle yürüyüş mesafesini ve süresini hesaplar.
    // 4. Kullanıcının geçmiş harcama alışkanlıkları ile restoranın fiyatını eşleştirip (%60 Konum + %40 Bütçe) akıllı bir uyum skoru (%95 gibi) üretir.
    // =========================================================================================
    public class RestaurantService : IRestaurantService
    {
        // Veritabanı tablolarına erişim sağlayan DbContext nesnesi
        private readonly AppDbContext _context;

        // Gerçek yürüyüş rotasını ve süresini hesaplayan harita yönlendirme servisi (OSRM)
        private readonly IOsrmService _osrmService;

        // Mesajları TR ve EN dillerine çeviren yerelleştirme nesnesi
        private readonly IStringLocalizer _localizer;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // RestaurantService ayağa kalktığında veritabanı, harita servisi ve dil desteğini içeri bağlar.
        // -------------------------------------------------------------------------------------
        public RestaurantService(AppDbContext context, IOsrmService osrmService, IStringLocalizer localizer)
        {
            _context = context;
            _osrmService = osrmService;
            _localizer = localizer;
        }

        // -------------------------------------------------------------------------------------
        // 1. Tüm Restoranları Listele
        // -------------------------------------------------------------------------------------
        public BaseResponse<List<Restaurant>> GetAll()
        {
            try
            {
                var list = _context.Restaurants.ToList();
                return BaseResponse<List<Restaurant>>.Success(list);
            }
            catch (Exception)
            {
                return BaseResponse<List<Restaurant>>.Fail(_localizer["RestaurantFetchError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 2. ID Numarasına Göre Restoran Detayı Getir
        // -------------------------------------------------------------------------------------
        public BaseResponse<Restaurant> GetById(int id)
        {
            try
            {
                var restaurant = GetRestaurantOrThrow(id, _localizer["RestaurantNotFound"]);
                return BaseResponse<Restaurant>.Success(restaurant);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<Restaurant>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<Restaurant>.Fail(_localizer["RestaurantGetByIdError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 3. Yeni Restoran Ekle
        // -------------------------------------------------------------------------------------
        public BaseResponse<Restaurant> Add(Restaurant newRestaurant)
        {
            try
            {
                _context.Restaurants.Add(newRestaurant);
                _context.SaveChanges();
                return BaseResponse<Restaurant>.Success(newRestaurant);
            }
            catch (Exception)
            {
                return BaseResponse<Restaurant>.Fail(_localizer["RestaurantAddError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 4. Restoran Bilgilerini Güncelle
        // -------------------------------------------------------------------------------------
        public BaseResponse<Restaurant> Update(int id, Restaurant updatedRestaurant)
        {
            try
            {
                var existing = GetRestaurantOrThrow(id, _localizer["RestaurantToUpdateNotFound"]);

                existing.UpdateDetails(updatedRestaurant.Name, updatedRestaurant.Category, updatedRestaurant.AveragePrice);
                existing.UpdateLocation(updatedRestaurant.Latitude, updatedRestaurant.Longitude);
                _context.SaveChanges();

                return BaseResponse<Restaurant>.Success(existing);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<Restaurant>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<Restaurant>.Fail(_localizer["RestaurantUpdateError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 5. Restoranı Sil
        // -------------------------------------------------------------------------------------
        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var existing = GetRestaurantOrThrow(id, _localizer["RestaurantToDeleteNotFound"]);

                _context.Restaurants.Remove(existing);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["RestaurantDeleteError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 6. YAKINDAKİ RESTORANLARI VE AKILLI PUANLARI HESAPLA (GetNearbyAsync):
        // En önemli çekirdek algoritmadır. Konum + Bütçe optimizasyonu yapar.
        // -------------------------------------------------------------------------------------
        public async Task<BaseResponse<List<RestaurantDistance>>> GetNearbyAsync(decimal latitude, decimal longitude, double radiusInKm, int? userId)
        {
            try
            {
                double userLat = (double)latitude;
                double userLng = (double)longitude;

                // 1. Aşama: Kullanıcının belirlediği yarıçap içindeki restoranları filtrele
                var nearbyCandidates = GetNearbyCandidates(userLat, userLng, radiusInKm);
                if (!nearbyCandidates.Any())
                {
                    return BaseResponse<List<RestaurantDistance>>.Success(new List<RestaurantDistance>());
                }

                // 2. Aşama: OSRM harita servisinden gerçek sokak yürüyüş mesafesini ve süresini çek
                var resultList = await _osrmService.GetDistancesFromOsrmAsync(nearbyCandidates, userLat, userLng);

                // Eğer OSRM harita servisine internetten ulaşılamazsa yedek olarak kuş uçuşu Haversine formülü devreye girer
                if (!resultList.Any())
                {
                    resultList = CalculateFallbackDistances(nearbyCandidates, userLat, userLng);
                }

                // 3. Aşama: Restoranları kullanıcının bütçesi ve mesafesine göre puanla
                ScoreRestaurants(resultList, userId, userLat, userLng);

                // En yüksek puana sahip restoranları en başta gösterecek şekilde sıralayıp dönüyoruz
                return BaseResponse<List<RestaurantDistance>>.Success(
                    resultList.OrderByDescending(r => r.RecommendationScore ?? 0).ToList()
                );
            }
            catch (Exception)
            {
                return BaseResponse<List<RestaurantDistance>>.Fail(_localizer["NearbyRestaurantsError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Restoranı veritabanında arar, bulamazsa hata fırlatır.
        // -------------------------------------------------------------------------------------
        private Restaurant GetRestaurantOrThrow(int id, string errorMessage)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant == null)
            {
                throw new ArgumentException(errorMessage);
            }
            return restaurant;
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Yarıçap (Bounding Box + Haversine) filtresi uygulayarak etraftaki restoranları seçer.
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Harita servisi çevrimdışı olduğunda devreye giren yedek mesafe hesaplayıcı.
        // -------------------------------------------------------------------------------------
        private List<RestaurantDistance> CalculateFallbackDistances(List<Restaurant> nearbyCandidates, double userLat, double userLng)
        {
            var fallbackList = new List<RestaurantDistance>();
            foreach (var rest in nearbyCandidates)
            {
                double distanceInMeters = GeoHelper.CalculateHaversineDistance(userLat, userLng, (double)rest.Latitude, (double)rest.Longitude);
                double durationInSeconds = distanceInMeters / 1.38; // Ortalama insan yürüme hızı (1.38 m/sn)

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

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Kullanıcının harcamalarına ve mesafeye göre 0-100 arası akıllı öneri skoru üretir.
        // -------------------------------------------------------------------------------------
        private void ScoreRestaurants(List<RestaurantDistance> resultList, int? userId, double userLat, double userLng)
        {
            // Kullanıcının ortalama öğün harcamasını varsayılan olarak 350 TL alıyoruz
            decimal userAverageExpenditure = 350.0m;
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

                // 500 metreden yakınsa 100 tam puan, 5000 metreye doğru azalan konum skoru
                double locationScore = 0;
                if (distanceForScoring <= 500)
                {
                    locationScore = 100;
                }
                else if (distanceForScoring < 5000)
                {
                    locationScore = 100.0 * (1.0 - (distanceForScoring - 500.0) / 4500.0);
                }

                // Kullanıcının ortalama harcaması ile mekanın menü fiyatı arasındaki bütçe uyumu skoru
                double budgetScore = BudgetHelper.CalculateProximityScore(userAverageExpenditure, item.AveragePrice);

                // Toplam Öneri Skoru: %60 Konum + %40 Bütçe Ağırlıklı
                double recommendationScore = (locationScore * 0.60) + (budgetScore * 0.40);

                item.LocationScore = Math.Round(locationScore, 1);
                item.BudgetScore = Math.Round(budgetScore, 1);
                item.RecommendationScore = Math.Round(recommendationScore, 1);
            }
        }
    }
}