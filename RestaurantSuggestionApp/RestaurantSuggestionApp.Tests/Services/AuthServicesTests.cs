using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
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
        private Mock<IEmailService> _mockEmailService;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_that_is_at_least_32_bytes_long_123456");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("RestaurantApp");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("RestaurantAppUsers");

            _mockEmailService = new Mock<IEmailService>();

            _authService = new AuthService(_context, _mockConfiguration.Object, _mockEmailService.Object, TestDataHelper.CreateMockLocalizer().Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region Register Tests (Kayıt Olma Testleri)

        [Test]
        public async Task Register_Successful_ReturnsJwtTokenAndSavesUser()
        {
            var response = await _authService.Register("Ahmet Yılmaz", "ahmet@mail.com", "sifre123", "sifre123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.RequiresVerification, Is.True);

            var savedUser = _context.Users.FirstOrDefault(u => u.Email == "ahmet@mail.com");
            Assert.That(savedUser, Is.Not.Null);
            Assert.That(savedUser.Name, Is.EqualTo("Ahmet Yılmaz"));
            Assert.That(savedUser.PasswordHash, Is.Not.EqualTo("sifre123"));
            Assert.That(BCrypt.Net.BCrypt.Verify("sifre123", savedUser.PasswordHash), Is.True);
        }

        [Test]
        public async Task Register_EmailAlreadyExists_ReturnsFail()
        {
            TestDataHelper.SeedUser(_context, "Eski Kullanıcı", "ahmet@mail.com", "eskiSifre");

            var response = await _authService.Register("Ahmet Yılmaz", "ahmet@mail.com", "sifre123", "sifre123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Bu e-posta adresi zaten kullanımda."));
        }

        [Test]
        public async Task Register_EmptyInputs_ReturnsFail()
        {
            var response = await _authService.Register("", "", "", "");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Ad, e-posta ve şifre alanları boş bırakılamaz."));
        }

        [Test]
        public async Task Register_InvalidEmailFormat_ReturnsFail()
        {
            var response = await _authService.Register("Ahmet Yılmaz", "gecersiz-email", "sifre123", "sifre123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Lütfen geçerli bir e-posta adresi giriniz."));
        }

        [Test]
        public async Task Register_PasswordTooShort_ReturnsFail()
        {
            var response = await _authService.Register("Ahmet Yılmaz", "ahmet@mail.com", "123", "123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Şifre en az 6 karakter olmalıdır."));
        }

        [Test]
        public async Task Register_PasswordsDoNotMatch_ReturnsFail()
        {
            var response = await _authService.Register("Ahmet Yılmaz", "ahmet@mail.com", "sifre123", "farkliSifre");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Girdiğiniz şifreler birbiriyle eşleşmiyor."));
        }

        #endregion

        #region Login Tests (Giriş Yapma Testleri)

        [Test]
        public async Task Login_Successful_RequiresVerificationAndSendsEmail()
        {
            TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            var response = await _authService.Login("ahmet@mail.com", "sifre123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.RequiresVerification, Is.True);
            Assert.That(response.Data.Email, Is.EqualTo("ahmet@mail.com"));

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == "ahmet@mail.com");
            Assert.That(userInDb?.VerificationCode, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Login_EmailServiceFails_StillReturnsSuccess()
        {
            TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            _mockEmailService
                .Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("E-posta gönderilemedi."));

            var response = await _authService.Login("ahmet@mail.com", "sifre123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.RequiresVerification, Is.True);
        }

        [Test]
        public async Task VerifyLogin_CorrectCode_ReturnsJwtToken()
        {
            var user = TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");
            user.SetVerificationCode("123456", 5);
            _context.SaveChanges();

            var response = await _authService.VerifyLogin("ahmet@mail.com", "123456");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task VerifyLogin_WrongCode_ReturnsFail()
        {
            var user = TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");
            user.SetVerificationCode("123456", 5);
            _context.SaveChanges();

            var response = await _authService.VerifyLogin("ahmet@mail.com", "999999");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Girdiğiniz doğrulama kodu hatalı!"));
        }

        [Test]
        public async Task Login_WrongPassword_ReturnsFail()
        {
            TestDataHelper.SeedUser(_context, "Ahmet Yılmaz", "ahmet@mail.com", "sifre123");

            var response = await _authService.Login("ahmet@mail.com", "yanlisSifre");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("E-posta veya şifre hatalı!"));
        }

        [Test]
        public async Task Login_UserNotFound_ReturnsFail()
        {
            var response = await _authService.Login("ahmet@mail.com", "sifre123");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("E-posta veya şifre hatalı!"));
        }

        [Test]
        public async Task Login_EmptyInputs_ReturnsFail()
        {
            var response = await _authService.Login("", "");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("E-posta ve şifre boş bırakılamaz."));
        }

        #endregion
    }
}