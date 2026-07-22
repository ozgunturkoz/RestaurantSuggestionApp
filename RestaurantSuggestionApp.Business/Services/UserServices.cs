using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IUserService
    {
        BaseResponse<List<User>> GetAll();
        BaseResponse<User> GetById(int id);
        BaseResponse<User> Add(User newUser);
        BaseResponse<User> Update(int id, User updatedUser);
        BaseResponse<bool> Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public BaseResponse<List<User>> GetAll()
        {
            try
            {
                var users = _context.Users.ToList();
                return BaseResponse<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<User>>.Fail($"Kullanıcılar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<User> GetById(int id)
        {
            try
            {
                var user = GetUserOrThrow(id, "Aradığınız kullanıcı bulunamadı.");
                return BaseResponse<User>.Success(user);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<User>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse<User>.Fail($"Kullanıcı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<User> Add(User newUser)
        {
            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return BaseResponse<User>.Success(newUser);
            }
            catch (Exception ex)
            {
                return BaseResponse<User>.Fail($"Kullanıcı eklenirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<User> Update(int id, User updatedUser)
        {
            try
            {
                var existing = GetUserOrThrow(id, "Güncellenecek kullanıcı bulunamadı.");

                existing.Name = updatedUser.Name;
                existing.Email = updatedUser.Email;
                if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
                {
                    existing.PasswordHash = updatedUser.PasswordHash;
                }
                _context.SaveChanges();

                return BaseResponse<User>.Success(existing);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<User>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse<User>.Fail($"Kullanıcı güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var existing = GetUserOrThrow(id, "Silinecek kullanıcı bulunamadı.");

                _context.Users.Remove(existing);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Fail($"Kullanıcı silinirken hata oluştu: {ex.Message}");
            }
        }

        private User GetUserOrThrow(int id, string errorMessage)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new ArgumentException(errorMessage);
            }
            return user;
        }
    }
}
