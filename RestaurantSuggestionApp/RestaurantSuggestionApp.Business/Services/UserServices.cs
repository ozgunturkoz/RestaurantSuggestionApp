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
    // =========================================================================================
    // BU ARAYÜZ (INTERFACE) NE İŞE YARAR?
    // Kullanıcı yönetimi (profil güncelleme, harcamalar, favoriler, şifre ve e-posta değiştirme)
    // işlemlerinin listesini ve imzasını barındıran sözleşmedir.
    // =========================================================================================
    public interface IUserService
    {
        // Tüm kullanıcıları listeler (Admin/Yönetim)
        BaseResponse<List<User>> GetAll();

        // Tek bir kullanıcıyı ID numarasına göre getirir
        BaseResponse<User> GetById(int id);

        // Yeni bir kullanıcı kaydeder
        BaseResponse<User> Add(User newUser);

        // Var olan kullanıcı bilgilerini günceller
        BaseResponse<User> Update(int id, User updatedUser);

        // Kullanıcıyı sistemden siler
        BaseResponse<bool> Delete(int id);

        // Kullanıcının favoriye eklediği restoranları listeler
        BaseResponse<List<UserFavorite>> GetFavorites(int userId);

        // Kullanıcının bir restoranı favorilerine eklemesini veya not yazmasını sağlar
        BaseResponse<UserFavorite> AddFavorite(int userId, int restaurantId, string? customNote);

        // Bir restoranı kullanıcının favorilerinden çıkarır
        BaseResponse<bool> RemoveFavorite(int userId, int restaurantId);

        // Kullanıcının geçmiş restoran harcamalarını listeler
        BaseResponse<List<UserExpenditureResponseDto>> GetExpenditures(int userId);

        // Kullanıcıya yeni bir harcama kaydı ekler
        BaseResponse<UserExpenditureResponseDto> AddExpenditure(int userId, CreateExpenditureDto dto);

        // Kullanıcının yanlış girdiği bir harcamayı siler
        BaseResponse<bool> DeleteExpenditure(int userId, int expenditureId);

        // Kullanıcının profilindeki adını günceller
        BaseResponse<User> UpdateProfileName(int userId, string name);

        // Giriş yapmış kullanıcının şifresini değiştirebilmesi için e-postasına onay kodu yollar
        Task<BaseResponse<bool>> SendPasswordResetCodeAsync(int userId);

        // E-postaya gelen kod ile kullanıcının şifresini günceller
        BaseResponse<bool> ChangePasswordWithCode(int userId, string code, string newPassword);

        // Kullanıcının yeni bir e-posta adresine geçebilmesi için yeni adrese onay kodu yollar
        Task<BaseResponse<bool>> SendEmailChangeCodeAsync(int userId, string newEmail);

        // Yeni e-postaya gelen kod ve mevcut şifre ile e-posta adresini günceller
        BaseResponse<bool> ChangeEmailWithCode(int userId, string newEmail, string code, string currentPassword);
    }

    // =========================================================================================
    // BU SINIF (CLASS) NE İŞE YARAR?
    // Kullanıcı işlemlerinin tüm iş kurallarını, veritabanı kayıtlarını ve güvenlik adımlarını yürütür.
    // =========================================================================================
    public class UserService : IUserService
    {
        // Veritabanı tablolarına erişim sağlayan DbContext nesnesi
        private readonly AppDbContext _context;

        // Doğrulama e-postalarını kullanıcılara ileten e-posta servisi
        private readonly IEmailService _emailService;

        // Mesajları kullanıcının dil tercihine göre (TR / EN) çeviren yerelleştirme nesnesi
        private readonly IStringLocalizer _localizer;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // UserService nesnesi üretildiği anda gerekli veritabanı ve servis bağımlılıklarını bağlar.
        // -------------------------------------------------------------------------------------
        public UserService(AppDbContext context, IEmailService emailService, IStringLocalizer localizer)
        {
            _context = context;
            _emailService = emailService;
            _localizer = localizer;
        }

        // -------------------------------------------------------------------------------------
        // Tüm Kullanıcıları Getir
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // ID Numarasına Göre Tek Bir Kullanıcıyı Getir
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Yeni Kullanıcı Ekle
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Kullanıcı Bilgilerini Güncelle
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Kullanıcıyı Sil
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Kullanıcının Favori Restoranlarını Listele
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Restoranı Favorilere Ekle veya Favori Notunu Güncelle
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Restoranı Favorilerden Çıkar
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Kullanıcının Harcama Geçmişini Listele
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Yeni Bir Harcama Kaydı Ekle
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Harcama Kaydını Sil
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Profil Adını Güncelle
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Profil İçinden Şifre Değiştirmek İçin E-Postaya Doğrulama Kodu Gönder
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Kod İle Şifre Değiştir
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Yeni E-Posta Adresine Doğrulama Kodu Gönder
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Kod ve Mevcut Şifre İle E-Posta Değişikliğini Onayla
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Kullanıcıyı ID ile bulur, yoksa hata fırlatır
        // -------------------------------------------------------------------------------------
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
