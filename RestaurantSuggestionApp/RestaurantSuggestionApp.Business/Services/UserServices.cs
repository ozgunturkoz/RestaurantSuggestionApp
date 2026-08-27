using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IUserService
    {
        BaseResponse<List<User>> GetAll();

        BaseResponse<User> GetById(int id);

        BaseResponse<User> Add(User newUser);

        BaseResponse<User> Update(int id, User updatedUser);

        BaseResponse<bool> Delete(int id);

        BaseResponse<List<UserFavorite>> GetFavorites(int userId);

        BaseResponse<UserFavorite> AddFavorite(int userId, int restaurantId, string? customNote);

        BaseResponse<bool> RemoveFavorite(int userId, int restaurantId);

        BaseResponse<List<UserExpenditureResponseDto>> GetExpenditures(int userId);

        BaseResponse<UserExpenditureResponseDto> AddExpenditure(int userId, CreateExpenditureDto dto);

        BaseResponse<bool> DeleteExpenditure(int userId, int expenditureId);

        BaseResponse<User> UpdateProfileName(int userId, string name);

        Task<BaseResponse<bool>> SendPasswordResetCodeAsync(int userId);

        BaseResponse<bool> ChangePasswordWithCode(int userId, string code, string newPassword);

        Task<BaseResponse<bool>> SendEmailChangeCodeAsync(int userId, string newEmail);

        BaseResponse<bool> ChangeEmailWithCode(int userId, string newEmail, string code, string currentPassword);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        private readonly IEmailService _emailService;

        private readonly IStringLocalizer _localizer;

        public UserService(AppDbContext context, IEmailService emailService, IStringLocalizer localizer)
        {
            _context = context;
            _emailService = emailService;
            _localizer = localizer;
        }

        public BaseResponse<List<User>> GetAll()
        {
            try
            {
                var users = _context.Users.ToList();
                return BaseResponse<List<User>>.Success(users);
            }
            catch (Exception)
            {
                return BaseResponse<List<User>>.Fail(_localizer["UsersFetchError"]);
            }
        }

        public BaseResponse<User> GetById(int id)
        {
            try
            {
                var user = GetUserOrThrow(id, _localizer["UserNotFound"]);
                return BaseResponse<User>.Success(user);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<User>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<User>.Fail(_localizer["UserGetByIdError"]);
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
            catch (Exception)
            {
                return BaseResponse<User>.Fail(_localizer["UserAddError"]);
            }
        }

        public BaseResponse<User> Update(int id, User updatedUser)
        {
            try
            {
                var existing = GetUserOrThrow(id, _localizer["UserToUpdateNotFound"]);

                existing.UpdateProfile(updatedUser.Name, updatedUser.Email);

                if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
                {
                    existing.SetPasswordHash(updatedUser.PasswordHash);
                }
                _context.SaveChanges();

                return BaseResponse<User>.Success(existing);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<User>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<User>.Fail(_localizer["UserUpdateError"]);
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var existing = GetUserOrThrow(id, _localizer["UserToDeleteNotFound"]);

                _context.Users.Remove(existing);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["UserDeleteError"]);
            }
        }

        public BaseResponse<List<UserFavorite>> GetFavorites(int userId)
        {
            try
            {
                var favorites = _context.UserFavorites
                    .Include(f => f.Restaurant)
                    .Where(f => f.UserId == userId)
                    .OrderByDescending(f => f.CreatedDate)
                    .ToList();

                return BaseResponse<List<UserFavorite>>.Success(favorites);
            }
            catch (Exception)
            {
                return BaseResponse<List<UserFavorite>>.Fail(_localizer["FavoritesFetchError"]);
            }
        }

        public BaseResponse<UserFavorite> AddFavorite(int userId, int restaurantId, string? customNote)
        {
            try
            {
                var restaurant = _context.Restaurants.Find(restaurantId);
                if (restaurant == null)
                {
                    return BaseResponse<UserFavorite>.Fail(_localizer["FavoriteRestaurantNotFound"]);
                }

                var existingFav = _context.UserFavorites
                    .FirstOrDefault(f => f.UserId == userId && f.RestaurantId == restaurantId);

                if (existingFav != null)
                {
                    existingFav.UpdateNote(customNote);
                    _context.SaveChanges();
                    return BaseResponse<UserFavorite>.Success(existingFav, _localizer["FavoriteNoteUpdated"]);
                }

                var newFav = new UserFavorite(userId, restaurantId, customNote);
                _context.UserFavorites.Add(newFav);
                _context.SaveChanges();

                return BaseResponse<UserFavorite>.Success(newFav, _localizer["FavoriteAdded"]);
            }
            catch (Exception)
            {
                return BaseResponse<UserFavorite>.Fail(_localizer["FavoriteAddError"]);
            }
        }

        public BaseResponse<bool> RemoveFavorite(int userId, int restaurantId)
        {
            try
            {
                var existingFav = _context.UserFavorites
                    .FirstOrDefault(f => f.UserId == userId && f.RestaurantId == restaurantId);

                if (existingFav == null)
                {
                    return BaseResponse<bool>.Fail(_localizer["FavoriteNotFound"]);
                }

                _context.UserFavorites.Remove(existingFav);
                _context.SaveChanges();

                return BaseResponse<bool>.Success(true, _localizer["FavoriteRemoved"]);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["FavoriteRemoveError"]);
            }
        }

        public BaseResponse<List<UserExpenditureResponseDto>> GetExpenditures(int userId)
        {
            try
            {
                var list = _context.UserExpenditures
                    .Include(e => e.Restaurant)
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.SpentAt)
                    .Select(e => new UserExpenditureResponseDto
                    {
                        Id = e.Id,
                        UserId = e.UserId,
                        RestaurantId = e.RestaurantId,
                        RestaurantName = e.Restaurant != null ? e.Restaurant.Name : null,
                        RestaurantCategory = e.Restaurant != null ? e.Restaurant.Category : null,
                        Amount = e.Amount,
                        SpentAt = e.SpentAt
                    })
                    .ToList();

                return BaseResponse<List<UserExpenditureResponseDto>>.Success(list);
            }
            catch (Exception)
            {
                return BaseResponse<List<UserExpenditureResponseDto>>.Fail(_localizer["ExpendituresFetchError"]);
            }
        }

        public BaseResponse<UserExpenditureResponseDto> AddExpenditure(int userId, CreateExpenditureDto dto)
        {
            try
            {
                if (dto.Amount <= 0)
                {
                    return BaseResponse<UserExpenditureResponseDto>.Fail(_localizer["ExpenditureAmountInvalid"]);
                }

                var expenditure = new UserExpenditure(
                    userId: userId,
                    restaurantId: dto.RestaurantId,
                    amount: dto.Amount,
                    spentAt: dto.SpentAt ?? DateTime.UtcNow
                );

                _context.UserExpenditures.Add(expenditure);
                _context.SaveChanges();

                string? restName = null;
                string? restCat = null;
                if (dto.RestaurantId.HasValue)
                {
                    var r = _context.Restaurants.Find(dto.RestaurantId.Value);
                    restName = r?.Name;
                    restCat = r?.Category;
                }

                var responseDto = new UserExpenditureResponseDto
                {
                    Id = expenditure.Id,
                    UserId = expenditure.UserId,
                    RestaurantId = expenditure.RestaurantId,
                    RestaurantName = restName,
                    RestaurantCategory = restCat,
                    Amount = expenditure.Amount,
                    SpentAt = expenditure.SpentAt
                };

                return BaseResponse<UserExpenditureResponseDto>.Success(responseDto, _localizer["ExpenditureAdded"]);
            }
            catch (Exception)
            {
                return BaseResponse<UserExpenditureResponseDto>.Fail(_localizer["ExpenditureAddError"]);
            }
        }

        public BaseResponse<bool> DeleteExpenditure(int userId, int expenditureId)
        {
            try
            {
                var expenditure = _context.UserExpenditures
                    .FirstOrDefault(e => e.Id == expenditureId && e.UserId == userId);

                if (expenditure == null)
                {
                    return BaseResponse<bool>.Fail(_localizer["ExpenditureNotFound"]);
                }

                _context.UserExpenditures.Remove(expenditure);
                _context.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["ExpenditureDeleteError"]);
            }
        }

        public BaseResponse<User> UpdateProfileName(int userId, string name)
        {
            try
            {
                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                user.UpdateProfile(name.Trim(), user.Email);
                _context.SaveChanges();

                return BaseResponse<User>.Success(user);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<User>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<User>.Fail(_localizer["ProfileUpdateError"]);
            }
        }

        public async Task<BaseResponse<bool>> SendPasswordResetCodeAsync(int userId)
        {
            try
            {
                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                var randomCode = new Random().Next(100000, 999999).ToString();
                user.SetVerificationCode(randomCode, expireMinutes: 5);
                _context.SaveChanges();

                var subject = _localizer["PasswordChangeEmailSubject"].Value;
                var body = _localizer["PasswordChangeEmailBody", user.Name, randomCode].Value;

                await _emailService.SendEmailAsync(user.Email, subject, body);

                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["PasswordResetCodeSendError"]);
            }
        }

        public BaseResponse<bool> ChangePasswordWithCode(int userId, string code, string newPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(newPassword))
                {
                    return BaseResponse<bool>.Fail(_localizer["PasswordAndCodeRequired"]);
                }

                if (newPassword.Length < 6)
                {
                    return BaseResponse<bool>.Fail(_localizer["PasswordMinLength"]);
                }

                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code.Trim())
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidCode"]);
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<bool>.Fail(_localizer["CodeExpired"]);
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.SetPasswordHash(hashedPassword);
                user.ClearVerificationCode();
                _context.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["PasswordUpdateError"]);
            }
        }

        public async Task<BaseResponse<bool>> SendEmailChangeCodeAsync(int userId, string newEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidEmailAddress"]);
                }

                var formattedNewEmail = newEmail.Trim().ToLower();

                var existingUserWithEmail = _context.Users.Any(u => u.Email == formattedNewEmail && u.Id != userId);
                if (existingUserWithEmail)
                {
                    return BaseResponse<bool>.Fail(_localizer["EmailAlreadyExists"]);
                }

                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                var random = new Random();
                var code = random.Next(100000, 999999).ToString();

                user.SetVerificationCode(code, 5);
                _context.SaveChanges();

                var subject = _localizer["EmailChangeEmailSubject"].Value;
                var body = _localizer["EmailChangeEmailBody", user.Name, formattedNewEmail, code].Value;

                await _emailService.SendEmailAsync(formattedNewEmail, subject, body);

                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["EmailChangeCodeSendError"]);
            }
        }

        public BaseResponse<bool> ChangeEmailWithCode(int userId, string newEmail, string code, string currentPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(currentPassword))
                {
                    return BaseResponse<bool>.Fail(_localizer["AllFieldsRequired"]);
                }

                var formattedNewEmail = newEmail.Trim().ToLower();

                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code.Trim())
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidCode"]);
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<bool>.Fail(_localizer["CodeExpired"]);
                }

                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                {
                    return BaseResponse<bool>.Fail(_localizer["CurrentPasswordIncorrect"]);
                }

                var existingUserWithEmail = _context.Users.Any(u => u.Email == formattedNewEmail && u.Id != userId);
                if (existingUserWithEmail)
                {
                    return BaseResponse<bool>.Fail(_localizer["EmailAlreadyExists"]);
                }

                user.UpdateProfile(user.Name, formattedNewEmail);
                user.ClearVerificationCode();
                _context.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<bool>.Fail(ex.Message);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["EmailUpdateError"]);
            }
        }

        private User GetUserOrThrow(int id, string errorMessage)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new ArgumentException(errorMessage);
            }
            return user;
        }
    }
}
