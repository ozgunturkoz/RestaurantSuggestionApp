using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;

namespace RestaurantSuggestionApp.Tests.Helpers
{
    public static class TestDataHelper
    {
        public static User SeedUser(AppDbContext context, string name = "Test User", string email = "test@mail.com", string password = "sifre123")
        {
            var user = new User(0, name, email, BCrypt.Net.BCrypt.HashPassword(password));

            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public static Restaurant SeedRestaurant(AppDbContext context, string name = "Test Restaurant", string category = "Kebap", decimal lat = 41.0082M, decimal lng = 28.9784M, decimal avgPrice = 100.0M)
        {
            var restaurant = new Restaurant(0, name, category, lat, lng, avgPrice);

            context.Restaurants.Add(restaurant);
            context.SaveChanges();
            return restaurant;
        }

        public static Moq.Mock<Microsoft.Extensions.Localization.IStringLocalizer> CreateMockLocalizer()
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
            {
                { "UserNotFound", "Aradığınız kullanıcı bulunamadı." },
                { "InvalidPassword", "E-posta veya şifre hatalı!" },
                { "InvalidInput", "E-posta ve şifre boş bırakılamaz." },
                { "InvalidCode", "Girdiğiniz doğrulama kodu hatalı!" },
                { "LoginSuccess", "Giriş başarılı." },
                { "RegisterSuccess", "Kayıt işlemi başarılı." },
                { "RestaurantNotFound", "Aradığınız restoran bulunamadı." },
                { "EmailAlreadyExists", "Bu e-posta adresi zaten kullanılmaktadır!" },
                { "Unauthorized", "Bu işlem için yetkiniz bulunmamaktadır." },
                { "InvalidToken", "Doğrulama kodunun süresi dolmuş. Lütfen tekrar kodu isteyin." },
                { "RestaurantFetchError", "Restoranlar getirilirken hata oluştu." },
                { "RestaurantGetByIdError", "Restoran getirilirken hata oluştu." },
                { "RestaurantAddError", "Restoran eklenirken hata oluştu." },
                { "RestaurantUpdateError", "Restoran güncellenirken hata oluştu." },
                { "RestaurantDeleteError", "Restoran silinirken hata oluştu." },
                { "NearbyRestaurantsError", "Yakın restoranlar hesaplanırken bir hata oluştu." },
                { "RestaurantToUpdateNotFound", "Güncellenecek restoran bulunamadı." },
                { "RestaurantToDeleteNotFound", "Silinecek restoran bulunamadı." },
                { "FavoriteRestaurantNotFound", "Favoriye eklenecek restoran bulunamadı." },
                { "FavoriteNotFound", "Kaldırılacak favori kaydı bulunamadı." },
                { "FavoriteAddError", "Favori eklenirken hata oluştu." },
                { "FavoriteRemoveError", "Favori kaldırılırken hata oluştu." },
                { "FavoritesFetchError", "Favoriler getirilirken hata oluştu." },
                { "UsersFetchError", "Kullanıcılar getirilirken hata oluştu." },
                { "UserGetByIdError", "Kullanıcı getirilirken hata oluştu." },
                { "UserAddError", "Kullanıcı eklenirken hata oluştu." },
                { "UserUpdateError", "Kullanıcı güncellenirken hata oluştu." },
                { "UserDeleteError", "Kullanıcı silinirken hata oluştu." },
                { "UserToUpdateNotFound", "Güncellenecek kullanıcı bulunamadı." },
                { "UserToDeleteNotFound", "Silinecek kullanıcı bulunamadı." },
                { "ExpendituresFetchError", "Harcamalar getirilirken hata oluştu." },
                { "ExpenditureAmountInvalid", "Harcama tutarı 0'dan büyük olmalıdır." },
                { "ExpenditureAddError", "Harcama eklenirken hata oluştu." },
                { "ExpenditureNotFound", "Silinecek harcama kaydı bulunamadı." },
                { "ExpenditureDeleteError", "Harcama silinirken hata oluştu." },
                { "ProfileUpdateError", "Profil güncellenirken hata oluştu." },
                { "PasswordResetCodeSendError", "Şifre sıfırlama kodu gönderilirken hata oluştu." },
                { "PasswordAndCodeRequired", "Doğrulama kodu ve yeni şifre boş bırakılamaz." },
                { "PasswordMinLength", "Şifre en az 6 karakter olmalıdır." },
                { "CodeExpired", "Doğrulama kodunun süresi dolmuş. Lütfen tekrar kod isteyin." },
                { "PasswordUpdateError", "Şifre güncellenirken hata oluştu." },
                { "InvalidEmailAddress", "Lütfen geçerli bir yeni e-posta adresi giriniz." },
                { "EmailChangeCodeSendError", "E-posta doğrulama kodu gönderilirken hata oluştu." },
                { "AllFieldsRequired", "Tüm alanların doldurulması zorunludur." },
                { "CurrentPasswordIncorrect", "Girdiğiniz mevcut şifre yanlış!" },
                { "EmailUpdateError", "E-posta adresi güncellenirken hata oluştu." },
                { "EmailAndCodeRequired", "E-posta ve doğrulama kodu boş bırakılamaz." },
                { "RegisterFieldsRequired", "Ad, e-posta ve şifre alanları boş bırakılamaz." },
                { "LoginError", "Giriş işlemi sırasında bir hata oluştu." },
                { "VerificationError", "Doğrulama işlemi sırasında hata oluştu." },
                { "RegisterError", "Kayıt işlemi sırasında bir hata oluştu." },
                { "UnauthorizedProfileUpdate", "Başka bir kullanıcının profilini güncelleme yetkiniz yok." },
                { "UnauthorizedAccountDelete", "Başka bir kullanıcının hesabını silme yetkiniz yok." },
                { "PasswordRequired", "Şifre alanı boş bırakılamaz." }
            };

            var mock = new Moq.Mock<Microsoft.Extensions.Localization.IStringLocalizer>();
            mock.Setup(l => l[Moq.It.IsAny<string>()]).Returns((string key) => new Microsoft.Extensions.Localization.LocalizedString(key, dict.TryGetValue(key, out var v) ? v : key));
            mock.Setup(l => l[Moq.It.IsAny<string>(), Moq.It.IsAny<object[]>()]).Returns((string key, object[] args) => new Microsoft.Extensions.Localization.LocalizedString(key, dict.TryGetValue(key, out var v) ? string.Format(v, args) : key));
            return mock;
        }
    }
}
