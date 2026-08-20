using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Tests.Helpers;

namespace RestaurantSuggestionApp.Tests.Services
{
    [TestFixture]
    public class UserServicesTests
    {
        private AppDbContext _context;
        private Mock<IEmailService> _emailServiceMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _emailServiceMock = new Mock<IEmailService>();
            _userService = new UserService(_context, _emailServiceMock.Object, TestDataHelper.CreateMockLocalizer().Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region GetAll Tests (Tüm Kullanıcıları Getirme)

        [Test]
        public void GetAll_WithUsers_ReturnsSuccessAndList()
        {
            var user1 = TestDataHelper.SeedUser(_context, "User One", "one@mail.com", "hash1");
            var user2 = TestDataHelper.SeedUser(_context, "User Two", "two@mail.com", "hash2");

            var response = _userService.GetAll();

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(2));
            Assert.That(response.Data.Any(u => u.Name == "User One"), Is.True);
        }

        [Test]
        public void GetAll_NoUsers_ReturnsSuccessAndEmptyList()
        {

            var response = _userService.GetAll();

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(0));
        }

        #endregion

        #region GetById Tests (ID'ye Göre Kullanıcı Getirme)

        [Test]
        public void GetById_ExistingUser_ReturnsSuccessWithUser()
        {
            var user = TestDataHelper.SeedUser(_context, "Test User", "test@mail.com", "hash");

            var response = _userService.GetById(user.Id);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(user.Id));
            Assert.That(response.Data.Name, Is.EqualTo("Test User"));
        }

        [Test]
        public void GetById_NonExistingUser_ReturnsFailWithErrorMessage()
        {

            var response = _userService.GetById(999);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Aradığınız kullanıcı bulunamadı."));
        }

        #endregion

        #region Add Tests (Kullanıcı Ekleme)

        [Test]
        public void Add_ValidUser_ReturnsSuccessAndSavesToDb()
        {
            var newUser = new User(0, "New User", "new@mail.com", "newhash");

            var response = _userService.Add(newUser);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Name, Is.EqualTo("New User"));

            var savedUser = _context.Users.FirstOrDefault(u => u.Email == "new@mail.com");
            Assert.That(savedUser, Is.Not.Null);
            Assert.That(savedUser.Name, Is.EqualTo("New User"));
        }

        #endregion

        #region Update Tests (Kullanıcı Güncelleme)

        [Test]
        public void Update_ExistingUser_ReturnsSuccessAndUpdatesDb()
        {
            var user = TestDataHelper.SeedUser(_context, "Old Name", "old@mail.com", "oldhash");

            var updatedData = new User(0, "New Name", "new@mail.com", "newhash");

            var response = _userService.Update(user.Id, updatedData);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Name, Is.EqualTo("New Name"));
            Assert.That(response.Data.Email, Is.EqualTo("new@mail.com"));
            Assert.That(response.Data.PasswordHash, Is.EqualTo("newhash"));

            var updatedUserInDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            Assert.That(updatedUserInDb, Is.Not.Null);
            Assert.That(updatedUserInDb.Name, Is.EqualTo("New Name"));
        }

        [Test]
        public void Update_NonExistingUser_ReturnsFailWithErrorMessage()
        {
            var updatedData = new User(0, "New Name", "new@mail.com", "newhash");

            var response = _userService.Update(999, updatedData);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Güncellenecek kullanıcı bulunamadı."));
        }

        #endregion

        #region Delete Tests (Silme Testleri)

        [Test]
        public void Delete_ExistingUser_ReturnsSuccessAndRemovesFromDb()
        {
            var user = TestDataHelper.SeedUser(_context, "Test User", "testuser@email.com", "password");

            var response = _userService.Delete(user.Id);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.True);

            var deletedUser = _context.Users.FirstOrDefault(r => r.Id == user.Id);
            Assert.That(deletedUser, Is.Null);
        }

        [Test]
        public void Delete_NonExistingUser_ReturnsFailWithErrorMessage()
        {
            var response = _userService.Delete(999);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Data, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Silinecek kullanıcı bulunamadı."));
        }

        #endregion

        #region Expenditure Tests (Harcama Yönetimi)

        [Test]
        public void AddExpenditure_ValidData_ReturnsSuccessAndCreatesRecord()
        {
            var user = TestDataHelper.SeedUser(_context, "Exp User", "exp@mail.com", "pass");
            var restaurant = TestDataHelper.SeedRestaurant(_context, "Lezzet Kebap", "Kebap", 41.0M, 29.0M, 200M);

            var dto = new CreateExpenditureDto
            {
                RestaurantId = restaurant.Id,
                Amount = 150.50M,
                SpentAt = DateTime.UtcNow
            };

            var response = _userService.AddExpenditure(user.Id, dto);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Amount, Is.EqualTo(150.50M));
            Assert.That(response.Data.RestaurantName, Is.EqualTo("Lezzet Kebap"));
        }

        [Test]
        public void GetExpenditures_ReturnsUserExpendituresList()
        {
            var user = TestDataHelper.SeedUser(_context, "Exp List User", "explist@mail.com", "pass");
            var dto = new CreateExpenditureDto { Amount = 100M };
            _userService.AddExpenditure(user.Id, dto);

            var response = _userService.GetExpenditures(user.Id);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.Count, Is.EqualTo(1));
            Assert.That(response.Data[0].Amount, Is.EqualTo(100M));
        }

        [Test]
        public void DeleteExpenditure_ExistingRecord_RemovesExpenditure()
        {
            var user = TestDataHelper.SeedUser(_context, "Del Exp User", "delexp@mail.com", "pass");
            var dto = new CreateExpenditureDto { Amount = 100M };
            var addRes = _userService.AddExpenditure(user.Id, dto);

            var delRes = _userService.DeleteExpenditure(user.Id, addRes.Data.Id);

            Assert.That(delRes.IsSuccess, Is.True);
            Assert.That(_context.UserExpenditures.Count(), Is.EqualTo(0));
        }

        #endregion

        #region Profile & Password Code Tests

        [Test]
        public void UpdateProfileName_ValidName_UpdatesUserName()
        {
            var user = TestDataHelper.SeedUser(_context, "Old Name", "profile@mail.com", "pass");

            var response = _userService.UpdateProfileName(user.Id, "New Profile Name");

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Data.Name, Is.EqualTo("New Profile Name"));
        }

        [Test]
        public async Task SendPasswordResetCodeAsync_SendsEmailWithCode()
        {
            var user = TestDataHelper.SeedUser(_context, "Code User", "code@mail.com", "pass");

            var response = await _userService.SendPasswordResetCodeAsync(user.Id);

            Assert.That(response.IsSuccess, Is.True);
            var updatedUser = _context.Users.First(u => u.Id == user.Id);
            Assert.That(updatedUser.VerificationCode, Is.Not.Null);
            _emailServiceMock.Verify(e => e.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ChangePasswordWithCode_ValidCode_ChangesPassword()
        {
            var user = TestDataHelper.SeedUser(_context, "Pass User", "passchange@mail.com", "oldpass123");
            user.SetVerificationCode("123456", 5);
            _context.SaveChanges();

            var response = _userService.ChangePasswordWithCode(user.Id, "123456", "newpass123");

            Assert.That(response.IsSuccess, Is.True);
            var updatedUser = _context.Users.First(u => u.Id == user.Id);
            Assert.That(BCrypt.Net.BCrypt.Verify("newpass123", updatedUser.PasswordHash), Is.True);
            Assert.That(updatedUser.VerificationCode, Is.Null);
        }

        [Test]
        public void ChangePasswordWithCode_InvalidCode_ReturnsFail()
        {
            var user = TestDataHelper.SeedUser(_context, "Pass Fail User", "passfail@mail.com", "oldpass123");
            user.SetVerificationCode("123456", 5);
            _context.SaveChanges();

            var response = _userService.ChangePasswordWithCode(user.Id, "999999", "newpass123");

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Girdiğiniz doğrulama kodu hatalı!"));
        }

        #endregion
    }
}