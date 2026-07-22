using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;

namespace RestaurantSuggestionApp.Tests.Services
{
    [TestFixture]
    public class RestaurantServicesTests
    {
        private AppDbContext _context;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<IConfiguration> _mockConfiguration;
        private RestaurantService _restaurantService;

        [SetUp]
        public void Setup()
        {
            // Her test öncesi sıfır bellek içi DB kurulur.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfiguration = new Mock<IConfiguration>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _restaurantService = new RestaurantService(
                _context,
                _mockHttpClientFactory.Object,
                _mockConfiguration.Object
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
            var rest1 = new Restaurant { Id = 1, Name = "Rest 1", Category = "Kebap", Latitude = 41.0M, Longitude = 29.0M, AveragePrice = 100.0M };
            var rest2 = new Restaurant { Id = 2, Name = "Rest 2", Category = "Burger", Latitude = 41.1M, Longitude = 29.1M, AveragePrice = 150.0M };
            _context.Restaurants.AddRange(rest1, rest2);
            _context.SaveChanges();

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
            var rest = new Restaurant { Id = 1, Name = "Rest 1", Category = "Kebap", Latitude = 41.0M, Longitude = 29.0M, AveragePrice = 100.0M };
            _context.Restaurants.Add(rest);
            _context.SaveChanges();

            // Act
            var response = _restaurantService.GetById(1);

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
            var newRest = new Restaurant { Name = "New Rest", Category = "Cafe", Latitude = 41.0M, Longitude = 29.0M, AveragePrice = 80.0M };

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
            var rest = new Restaurant { Id = 1, Name = "Old Name", Category = "Kebap", Latitude = 41.0M, Longitude = 29.0M, AveragePrice = 100.0M };
            _context.Restaurants.Add(rest);
            _context.SaveChanges();

            var updatedRest = new Restaurant { Name = "New Name", Category = "Kebap", Latitude = 41.0M, Longitude = 29.0M, AveragePrice = 120.0M };

            // Act
            var response = _restaurantService.Update(1, updatedRest);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.Name, Is.EqualTo("New Name"));

            var updatedInDb = _context.Restaurants.FirstOrDefault(r => r.Id == 1);
            Assert.That(updatedInDb.AveragePrice, Is.EqualTo(120.0M));
        }

        [Test]
        public void Update_NonExisting_ReturnsFailWithErrorMessage()
        {
            // Arrange
            var updatedRest = new Restaurant { Name = "New Name" };

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
            var restaurant = new Restaurant
            {
                Id = 1,
                Name = "Gourmet Bistro",
                Category = "Italian",
                Latitude = 41.0082M,
                Longitude = 28.9784M,
                AveragePrice = 150.00M
            };

            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            var response = _restaurantService.Delete(1);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.True);

            var deletedRestaurant = _context.Restaurants.FirstOrDefault(r => r.Id == 1);
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
            var rest = new Restaurant
            {
                Id = 1,
                Name = "Yakın Restoran",
                Category = "Ev Yemekleri",
                Latitude = 41.0085M,
                Longitude = 28.9789M,
                AveragePrice = 120.0M
            };
            _context.Restaurants.Add(rest);
            _context.SaveChanges();

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

        #endregion
    }
}