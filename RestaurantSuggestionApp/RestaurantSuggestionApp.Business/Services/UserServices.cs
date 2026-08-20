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
                var existing = _context.UserFavorites
                    .FirstOrDefault(f => f.UserId == userId && f.RestaurantId == restaurantId);

                if (existing != null)
                {
                    existing.UpdateNote(customNote ?? existing.CustomNote);
                    _context.SaveChanges();
                    return BaseResponse<UserFavorite>.Success(existing);
                }

                var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
                if (restaurant == null)
                {
                    return BaseResponse<UserFavorite>.Fail(_localizer["FavoriteRestaurantNotFound"]);
                }

                var favorite = new UserFavorite(0, userId, restaurantId, customNote);

                _context.UserFavorites.Add(favorite);
                _context.SaveChanges();

                favorite.Restaurant = restaurant;
                return BaseResponse<UserFavorite>.Success(favorite);
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
                var favorite = _context.UserFavorites
                    .FirstOrDefault(f => f.UserId == userId && f.RestaurantId == restaurantId);

                if (favorite == null)
                {
                    return BaseResponse<bool>.Fail(_localizer["FavoriteNotFound"]);
                }

                _context.UserFavorites.Remove(favorite);
                _context.SaveChanges();
                return BaseResponse<bool>.Success(true);
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
                var expenditures = _context.UserExpenditures
                    .Include(e => e.Restaurant)
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.SpentAt)
                    .Select(e => new UserExpenditureResponseDto
                    {
                        Id = e.Id,
                        UserId = e.UserId,
                        RestaurantId = e.RestaurantId,
                        RestaurantName = e.Restaurant != null ? e.Restaurant.Name : "Diğer Harcama",
                        RestaurantCategory = e.Restaurant != null ? e.Restaurant.Category : "Genel",
                        Amount = e.Amount,
                        SpentAt = e.SpentAt
                    })
                    .ToList();

                return BaseResponse<List<UserExpenditureResponseDto>>.Success(expenditures);
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

                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                Restaurant? restaurant = null;
                if (dto.RestaurantId.HasValue && dto.RestaurantId.Value > 0)
                {
                    restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == dto.RestaurantId.Value);
                }

                var expenditure = new UserExpenditure(
                    0,
                    userId,
                    dto.RestaurantId > 0 ? dto.RestaurantId : null,
                    dto.Amount,
                    dto.SpentAt ?? DateTime.UtcNow
                );

                _context.UserExpenditures.Add(expenditure);
                _context.SaveChanges();

                var responseDto = new UserExpenditureResponseDto
                {
                    Id = expenditure.Id,
                    UserId = expenditure.UserId,
                    RestaurantId = expenditure.RestaurantId,
                    RestaurantName = restaurant != null ? restaurant.Name : "Diğer Harcama",
                    RestaurantCategory = restaurant != null ? restaurant.Category : "Genel",
                    Amount = expenditure.Amount,
                    SpentAt = expenditure.SpentAt
                };

                return BaseResponse<UserExpenditureResponseDto>.Success(responseDto);
            }
            catch (ArgumentException ex)
            {
                return BaseResponse<UserExpenditureResponseDto>.Fail(ex.Message);
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
                user.UpdateProfile(name, user.Email);
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

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Şifre Değiştirme Kodunuz - Sofra Restoran",
                    $"<h3>Merhaba {user.Name},</h3><p>Şifrenizi değiştirmek için kullanabileceğiniz doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{randomCode}</b></p><p>Bu kod 5 dakika süreyle geçerlidir.</p>"
                );

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

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code)
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

                var emailBody = $@"
                    <h2>Sofra - E-Posta Adresi Değişikliği</h2>
                    <p>Hesabınızın e-posta adresini <strong>{formattedNewEmail}</strong> olarak değiştirmek üzeresiniz.</p>
                    <p>Doğrulama Kodunuz: <strong style='font-size:18px;color:#E0A458;'>{code}</strong></p>
                    <p>Bu kod 5 dakika boyunca geçerlidir.</p>";

                await _emailService.SendEmailAsync(formattedNewEmail, "Sofra - E-Posta Değişikliği Doğrulama Kodu", emailBody);

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

                var existingUserWithEmail = _context.Users.Any(u => u.Email == formattedNewEmail && u.Id != userId);
                if (existingUserWithEmail)
                {
                    return BaseResponse<bool>.Fail(_localizer["EmailAlreadyExists"]);
                }

                var user = GetUserOrThrow(userId, _localizer["UserNotFound"]);

                // Doğrulama kodu kontrolü
                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code.Trim())
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidCode"]);
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<bool>.Fail(_localizer["CodeExpired"]);
                }

                // Şifre kontrolü
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                {
                    return BaseResponse<bool>.Fail(_localizer["CurrentPasswordIncorrect"]);
                }

                user.UpdateEmail(formattedNewEmail);
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
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new ArgumentException(errorMessage);
            }
            return user;
        }
    }
}


