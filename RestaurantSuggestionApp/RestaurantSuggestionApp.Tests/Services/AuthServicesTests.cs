using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Tests.Helpers;

namespace RestaurantSuggestionApp.Tests.Services
{
    [TestFixture]
    public class AuthServicesTests
    {
        private AppDbContext _context;
        private Mock<IConfiguration> _mockConfiguration;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            // Her test için temiz bir bellek içi (InMemory) veritabanı kurulur.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // JWT token üretimi için sahte IConfiguration nesnesi hazırlanır.
            _mockConfiguration = new Mock<IConfiguration>();

            // JWT token imzalanırken hata vermemesi için EN AZ 32 karakterli gizli bir anahtar taklit edilir.
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_that_is_at_least_32_bytes_long_123456");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("RestaurantApp");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("RestaurantAppUsers");

            // Servisimiz ayağa kaldırılır.
            _authService = new AuthService(_context, _mockConfiguration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region Register Tests (Kayıt Olma Testleri)

        [Test]
        public void Register_Successful_ReturnsJwtTokenAndSavesUser()
        {
            // Arrange - Hazırlık (Veritabanı boş)

            // Act - Eylem (Kayıt olunmaya çalışılıyor)
            var response = _authService.Register("Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null.And.Not.Empty); // Token dönmüş olmalı.

            // Kullanıcının veritabanına gerçekten kaydedilip kaydedilmediği doğrulanır.
            var savedUser = _context.Users.FirstOrDefault(u => u.Email == "ahmet@mail.com");
            Assert.That(savedUser, Is.Not.Null);
            Assert.That(savedUser.Name, Is.EqualTo("Ahmet Yılmaz"));

            // Şifrenin yalın haliyle değil, hashlenmiş olarak saklandığı doğrulanır.
            Assert.That(savedUser.PasswordHash, Is.Not.EqualTo("sifre123"));
            Assert.That(BCrypt.Net.BCrypt.Verify("sifre123", savedUser.PasswordHash), Is.True);
        }

        [Test]
        public void Register_EmailAlreadyExists_ReturnsFail()
        {
            // Arrange - Hazırlık (E-posta veritabanında zaten kayıtlı)
            var existingUser = TestDataHelper.SeedUser(_context, "Eski Kullanıcı", "ahmet@mail.com", "eskiSifre");

            // Act - Eylem (Aynı e-posta ile yeni kayıt denemesi)
            var response = _authService.Register("Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Bu e-posta adresi zaten kullanılmaktadır!"));
        }

        [Test]
        public void Register_EmptyInputs_ReturnsFail()
        {
            // Arrange - Hazırlık (Alanlar boş)

            // Act - Eylem
            var response = _authService.Register("", "", "");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Ad, e-posta ve şifre alanları boş bırakılamaz."));
        }

        #endregion

        #region Login Tests (Giriş Yapma Testleri)

        [Test]
        public void Login_Successful_ReturnsJwtToken()
        {
            // Arrange - Hazırlık (Şifresi hashlenmiş kullanıcı sisteme eklenir)
            var user = TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            // Act - Eylem (Doğru bilgilerle giriş yapılıyor)
            var response = _authService.Login("ahmet@mail.com", "sifre123");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null.And.Not.Empty); // Başarılı token dönmeli.
        }

        [Test]
        public void Login_WrongPassword_ReturnsFail()
        {
            // Arrange - Hazırlık
            var user = TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            // Act - Eylem (Yanlış şifre giriliyor)
            var response = _authService.Login("ahmet@mail.com", "yanlisSifre");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("E-posta veya şifre hatalı!"));
        }

        [Test]
        public void Login_UserNotFound_ReturnsFail()
        {
            // Arrange - Hazırlık (Veritabanı boş)

            // Act - Eylem (Olmayan kullanıcı ile giriş)
            var response = _authService.Login("ahmet@mail.com", "sifre123");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("E-posta veya şifre hatalı!"));
        }

        [Test]
        public void Login_EmptyInputs_ReturnsFail()
        {
            // Arrange - Hazırlık

            // Act - Eylem
            var response = _authService.Login("", "");

            // Assert - Doğrulama
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("E-posta ve şifre boş bırakılamaz."));
        }

        #endregion
    }
}