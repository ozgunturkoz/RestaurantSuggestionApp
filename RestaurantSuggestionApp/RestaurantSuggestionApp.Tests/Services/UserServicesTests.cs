using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.Tests.Helpers;

namespace RestaurantSuggestionApp.Tests.Services
{
    [TestFixture]
    public class UserServicesTests
    {
        private AppDbContext _context;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _userService = new UserService(_context);
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
            var user = TestDataHelper.SeedUser(_context, "Test User", "testuser@email.com", "password");

            var response = _userService.Delete(999);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Data, Is.False);
            Assert.That(response.ErrorMessage, Is.EqualTo("Silinecek kullanıcı bulunamadı."));
        }


        #endregion
    }
}