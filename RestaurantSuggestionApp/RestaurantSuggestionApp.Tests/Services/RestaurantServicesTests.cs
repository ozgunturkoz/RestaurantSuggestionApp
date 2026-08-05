using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Business.ExternalServices;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.Core.DTOs.Responses;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Tests.Helpers;

namespace RestaurantSuggestionApp.Tests.Services
{
    [TestFixture]
    public class RestaurantServicesTests
    {
        private AppDbContext _context;
        private Mock<IOsrmService> _mockOsrmService;
        private RestaurantService _restaurantService;

        [SetUp]
        public void Setup()
        {
            // Her test öncesi sıfır bellek içi DB kurulur.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _mockOsrmService = new Mock<IOsrmService>();

            _restaurantService = new RestaurantService(
                _context,
                _mockOsrmService.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region GetAll Tests (Tüm Restoranları Listeleme)

        [Test]
        public void GetAll_WithRestaurants_ReturnsSuccessAndList()
        {
            // Arrange - Test restoranları ekleniyor
            var rest1 = TestDataHelper.SeedRestaurant(_context, "Rest 1", "Kebap", 41.0M, 29.0M, 100.0M);
            var rest2 = TestDataHelper.SeedRestaurant(_context, "Rest 2", "Burger", 41.1M, 29.1M, 150.0M);

            // Act
            var response = _restaurantService.GetAll();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetAll_NoRestaurants_ReturnsSuccessAndEmptyList()
        {
            // Act
            var response = _restaurantService.GetAll();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.Count, Is.EqualTo(0));
        }

        #endregion

        #region GetById Tests (ID'ye Göre Restoran Getirme)

        [Test]
        public void GetById_Existing_ReturnsSuccessWithRestaurant()
        {
            // Arrange
            var rest = TestDataHelper.SeedRestaurant(_context, "Rest 1", "Kebap", 41.0M, 29.0M, 100.0M);

            // Act
            var response = _restaurantService.GetById(rest.Id);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.Name, Is.EqualTo("Rest 1"));
        }

        [Test]
        public void GetById_NonExisting_ReturnsFailWithErrorMessage()
        {
            // Act
            var response = _restaurantService.GetById(999);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Aradığınız restoran bulunamadı."));
        }

        #endregion

        #region Add Tests (Restoran Ekleme)

        [Test]
        public void Add_ValidRestaurant_ReturnsSuccessAndSavesToDb()
        {
            // Arrange
            var newRest = new Restaurant(0, "New Rest", "Cafe", 41.0M, 29.0M, 80.0M);

            // Act
            var response = _restaurantService.Add(newRest);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);

            var saved = _context.Restaurants.FirstOrDefault(r => r.Name == "New Rest");
            Assert.That(saved, Is.Not.Null);
        }

        #endregion

        #region Update Tests (Restoran Güncelleme)

        [Test]
        public void Update_Existing_ReturnsSuccessAndUpdatesDb()
        {
            // Arrange
            var rest = TestDataHelper.SeedRestaurant(_context, "Old Name", "Kebap", 41.0M, 29.0M, 100.0M);

            var updatedRest = new Restaurant(0, "New Name", "Kebap", 41.0M, 29.0M, 120.0M);

            // Act
            var response = _restaurantService.Update(rest.Id, updatedRest);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.Name, Is.EqualTo("New Name"));

            var updatedInDb = _context.Restaurants.FirstOrDefault(r => r.Id == rest.Id);
            Assert.That(updatedInDb.AveragePrice, Is.EqualTo(120.0M));
        }

        [Test]
        public void Update_NonExisting_ReturnsFailWithErrorMessage()
        {
            // Arrange
            var updatedRest = new Restaurant(0, "New Name", "Kebap", 41.0M, 29.0M, 120.0M);

            // Act
            var response = _restaurantService.Update(999, updatedRest);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Güncellenecek restoran bulunamadı."));
        }

        #endregion

        #region Delete Tests (Silme Testleri)

        // =============================================================
        //  BU BÖLÜM TAMAMEN SİZİN İSTEDİĞİNİZ SİLME TEST KODLARIDIR
        // =============================================================

        [Test]
        public void Delete_ExistingRestaurant_ReturnsSuccessAndRemovesFromDb()
        {
            var restaurant = TestDataHelper.SeedRestaurant(_context, "Gourmet Bistro", "Italian", 41.0082M, 28.9784M, 150.00M);

            var response = _restaurantService.Delete(restaurant.Id);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.True);

            var deletedRestaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurant.Id);
            Assert.That(deletedRestaurant, Is.Null);
        }

        [Test]
        public void Delete_NonExistingRestaurant_ReturnsFailWithErrorMessage()
        {
            var response = _restaurantService.Delete(99);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Silinecek restoran bulunamadı."));
        }

        // =============================================================
        //  SİLME TEST KODLARININ SONU
        // =============================================================

        #endregion

        #region GetNearbyAsync Tests (Yakın Restoran Öneri Testleri)

        [Test]
        public async Task GetNearbyAsync_RestaurantsInRange_ReturnsScoredAndOrderedList()
        {
            // Arrange
            // Koordinatı: 41.0082, 28.9784 (Yaklaşık 50 metre uzağına restoran ekliyoruz)
            var rest = TestDataHelper.SeedRestaurant(_context, "Yakın Restoran", "Ev Yemekleri", 41.0085M, 28.9789M, 120.0M);

            _mockOsrmService.Setup(s => s.GetDistancesFromOsrmAsync(
                It.IsAny<List<Restaurant>>(),
                It.IsAny<double>(),
                It.IsAny<double>()
            )).ReturnsAsync(new List<RestaurantDistance>
            {
                new RestaurantDistance
                {
                    Id = rest.Id,
                    Name = "Yakın Restoran",
                    Category = "Ev Yemekleri",
                    Latitude = 41.0085M,
                    Longitude = 28.9789M,
                    AveragePrice = 120.0M,
                    WalkingDistanceInMeters = 50,
                    WalkingDurationInSeconds = 40
                }
            });

            // Act - 41.0082 enlemi ve 28.9784 boylamından 2 km yarıçapındaki restoranları arıyoruz.
            var response = await _restaurantService.GetNearbyAsync(41.0082M, 28.9784M, 2.0, null);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(1));
            Assert.That(response.Data[0].Name, Is.EqualTo("Yakın Restoran"));
            Assert.That(response.Data[0].RecommendationScore, Is.GreaterThan(0)); // Algoritmanın çalışıp puan ürettiğini doğruluyoruz.
        }

        [Test]
        public async Task GetNearbyAsync_NoRestaurantsInRange_ReturnsSuccessAndEmptyList()
        {
            // Arrange - Çok uzakta bir restoran ekliyoruz (örn. Ankara koordinatları)
            TestDataHelper.SeedRestaurant(_context, "Uzak Restoran", "Kebap", 39.9208M, 32.8541M, 150.0M);

            // Act - İstanbul (41.0082, 28.9784) adresinden 5 km yarıçapında restoran arıyoruz
            var response = await _restaurantService.GetNearbyAsync(41.0082M, 28.9784M, 5.0, null);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(0));
        }

        #endregion
    }
}