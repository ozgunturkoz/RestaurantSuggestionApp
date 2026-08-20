using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantSuggestionApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizationResourcesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeExpiresAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocalizationResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Culture = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResourceKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationResources", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LocalizationResources",
                columns: new[] { "Id", "Culture", "ResourceKey", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, "tr", "UserNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(7493), "Kullanıcı bulunamadı." },
                    { 2, "tr", "InvalidPassword", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8033), "E-posta veya şifre hatalı!" },
                    { 3, "tr", "InvalidInput", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8034), "E-posta ve şifre boş bırakılamaz." },
                    { 4, "tr", "InvalidCode", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8035), "Girdiğiniz doğrulama kodu hatalı!" },
                    { 5, "tr", "LoginSuccess", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8036), "Giriş başarılı." },
                    { 6, "tr", "RegisterSuccess", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8040), "Kayıt işlemi başarılı." },
                    { 7, "tr", "RestaurantNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8041), "Restoran bulunamadı." },
                    { 8, "tr", "EmailAlreadyExists", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8042), "Bu e-posta adresi zaten kullanımda." },
                    { 9, "tr", "Unauthorized", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8043), "Bu işlem için yetkiniz bulunmamaktadır." },
                    { 10, "tr", "InvalidToken", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8045), "Geçersiz veya süresi dolmuş oturum." },
                    { 11, "tr", "ExpenditureAdded", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8084), "Harcama başarıyla eklendi." },
                    { 12, "tr", "FavoriteAdded", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8085), "Restoran favorilere eklendi." },
                    { 13, "tr", "FavoriteRemoved", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8086), "Restoran favorilerden çıkarıldı." },
                    { 14, "tr", "FavoriteNoteUpdated", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8087), "Favori notu güncellendi." },
                    { 15, "tr", "RestaurantFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8088), "Restoranlar getirilirken hata oluştu." },
                    { 16, "tr", "RestaurantGetByIdError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8089), "Restoran getirilirken hata oluştu." },
                    { 17, "tr", "RestaurantAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8090), "Restoran eklenirken hata oluştu." },
                    { 18, "tr", "RestaurantUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8093), "Restoran güncellenirken hata oluştu." },
                    { 19, "tr", "RestaurantDeleteError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8094), "Restoran silinirken hata oluştu." },
                    { 20, "tr", "NearbyRestaurantsError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8095), "Yakın restoranlar hesaplanırken bir hata oluştu." },
                    { 21, "tr", "RestaurantToUpdateNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8096), "Güncellenecek restoran bulunamadı." },
                    { 22, "tr", "RestaurantToDeleteNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8097), "Silinecek restoran bulunamadı." },
                    { 23, "tr", "FavoriteRestaurantNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8098), "Favoriye eklenecek restoran bulunamadı." },
                    { 24, "tr", "FavoriteNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8099), "Kaldırılacak favori kaydı bulunamadı." },
                    { 25, "tr", "FavoriteAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8101), "Favori eklenirken hata oluştu." },
                    { 26, "tr", "FavoriteRemoveError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8102), "Favori kaldırılırken hata oluştu." },
                    { 27, "tr", "FavoritesFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8103), "Favoriler getirilirken hata oluştu." },
                    { 28, "tr", "UsersFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8104), "Kullanıcılar getirilirken hata oluştu." },
                    { 29, "tr", "UserGetByIdError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8105), "Kullanıcı getirilirken hata oluştu." },
                    { 30, "tr", "UserAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8107), "Kullanıcı eklenirken hata oluştu." },
                    { 31, "tr", "UserUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8108), "Kullanıcı güncellenirken hata oluştu." },
                    { 32, "tr", "UserDeleteError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8109), "Kullanıcı silinirken hata oluştu." },
                    { 33, "tr", "UserToUpdateNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8110), "Güncellenecek kullanıcı bulunamadı." },
                    { 34, "tr", "UserToDeleteNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8112), "Silinecek kullanıcı bulunamadı." },
                    { 35, "tr", "ExpendituresFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8113), "Harcamalar getirilirken hata oluştu." },
                    { 36, "tr", "ExpenditureAmountInvalid", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8114), "Harcama tutarı 0'dan büyük olmalıdır." },
                    { 37, "tr", "ExpenditureAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8115), "Harcama eklenirken hata oluştu." },
                    { 38, "tr", "ExpenditureNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8116), "Silinecek harcama kaydı bulunamadı." },
                    { 39, "tr", "ExpenditureDeleteError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8117), "Harcama silinirken hata oluştu." },
                    { 40, "tr", "ProfileUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8118), "Profil güncellenirken hata oluştu." },
                    { 41, "tr", "PasswordResetCodeSendError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8119), "Şifre sıfırlama kodu gönderilirken hata oluştu." },
                    { 42, "tr", "PasswordAndCodeRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8120), "Doğrulama kodu ve yeni şifre boş bırakılamaz." },
                    { 43, "tr", "PasswordMinLength", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8121), "Şifre en az 6 karakter olmalıdır." },
                    { 44, "tr", "CodeExpired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8122), "Doğrulama kodunun süresi dolmuş. Lütfen tekrar kod isteyin." },
                    { 45, "tr", "PasswordUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8123), "Şifre güncellenirken hata oluştu." },
                    { 46, "tr", "InvalidEmailAddress", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8124), "Lütfen geçerli bir yeni e-posta adresi giriniz." },
                    { 47, "tr", "EmailChangeCodeSendError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8126), "E-posta doğrulama kodu gönderilirken hata oluştu." },
                    { 48, "tr", "AllFieldsRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8137), "Tüm alanların doldurulması zorunludur." },
                    { 49, "tr", "CurrentPasswordIncorrect", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8139), "Girdiğiniz mevcut şifre yanlış!" },
                    { 50, "tr", "EmailUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8141), "E-posta adresi güncellenirken hata oluştu." },
                    { 51, "tr", "EmailAndCodeRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8142), "E-posta ve doğrulama kodu boş bırakılamaz." },
                    { 52, "tr", "RegisterFieldsRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8144), "Ad, e-posta ve şifre alanları boş bırakılamaz." },
                    { 53, "tr", "LoginError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8167), "Giriş işlemi sırasında bir hata oluştu." },
                    { 54, "tr", "VerificationError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8168), "Doğrulama işlemi sırasında hata oluştu." },
                    { 55, "tr", "RegisterError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8169), "Kayıt işlemi sırasında bir hata oluştu." },
                    { 56, "tr", "UnauthorizedProfileUpdate", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8171), "Başka bir kullanıcının profilini güncelleme yetkiniz yok." },
                    { 57, "tr", "UnauthorizedAccountDelete", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8172), "Başka bir kullanıcının hesabını silme yetkiniz yok." },
                    { 58, "tr", "PasswordRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8173), "Şifre alanı boş bırakılamaz." },
                    { 59, "tr", "nav.discover", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8175), "Keşfet" },
                    { 60, "tr", "nav.hello", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8176), "MERHABA" },
                    { 61, "tr", "nav.logout", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8177), "Çıkış Yap" },
                    { 62, "tr", "profile.backToDiscover", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8178), "Keşfet Sayfasına Dön" },
                    { 63, "tr", "profile.myProfile", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8179), "Profilim" },
                    { 64, "tr", "sidebar.radiusPrefix", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8180), "Yarıçap" },
                    { 65, "tr", "sidebar.title", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8182), "Yakınındaki restoranlar" },
                    { 66, "tr", "sidebar.subtitle", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8185), "Konumuna göre listelenen restoranlar" },
                    { 67, "tr", "sidebar.radiusLabel", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8186), "Mesafe Sınırı (Yarıçap)" },
                    { 68, "tr", "sidebar.loading", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8187), "Yükleniyor..." },
                    { 69, "tr", "sidebar.distance", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8188), "Mesafe" },
                    { 70, "tr", "sidebar.noRestaurants", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8189), "Seçilen yarıçapta etrafınızda restoran bulunamadı." },
                    { 71, "tr", "badge.recommendation", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8190), "%{score} Multinet profilinize ve konumunuza uygun" },
                    { 72, "tr", "map.loading", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8192), "Harita Yükleniyor..." },
                    { 73, "tr", "map.userMarker", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8193), "Buradasınız" },
                    { 74, "tr", "map.category", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8194), "Kategori" },
                    { 75, "tr", "map.price", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8196), "Fiyat" },
                    { 76, "tr", "map.walk", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8197), "Yürüyüş" },
                    { 77, "tr", "map.match", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8198), "Uyum" },
                    { 78, "tr", "map.locationScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8198), "Konum" },
                    { 79, "tr", "map.budgetScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8199), "Bütçe" },
                    { 80, "tr", "filter.search", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8200), "Restoran Ara" },
                    { 81, "tr", "filter.searchPlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8201), "Restoran adı yazın..." },
                    { 82, "tr", "filter.category", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8202), "Kategori" },
                    { 83, "tr", "filter.allCategories", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8203), "Tümü" },
                    { 84, "tr", "filter.price", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8204), "Fiyat Aralığı" },
                    { 85, "tr", "filter.allPrices", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8205), "Tümü" },
                    { 86, "tr", "filter.noResults", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8206), "Filtrelere uygun restoran bulunamadı." },
                    { 87, "tr", "filter.clearFilters", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8207), "Filtreleri Temizle" },
                    { 88, "tr", "filter.resultCount", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8208), "{count} restoran listeleniyor" },
                    { 89, "tr", "detail.category", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8209), "Kategori" },
                    { 90, "tr", "detail.price", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8210), "Ortalama Fiyat" },
                    { 91, "tr", "detail.distance", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8212), "Yürüme Mesafesi" },
                    { 92, "tr", "detail.duration", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8213), "Yürüme Süresi" },
                    { 93, "tr", "detail.score", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8214), "Öneri Skoru" },
                    { 94, "tr", "detail.locationScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8215), "Konum Skoru" },
                    { 95, "tr", "detail.budgetScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8238), "Bütçe Skoru" },
                    { 96, "tr", "detail.favoriteNote", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8239), "Favori Notunuz" },
                    { 97, "tr", "detail.favoriteNotePlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8240), "Bu mekan hakkında bir not ekleyin..." },
                    { 98, "tr", "detail.saveNote", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8241), "Notu Kaydet" },
                    { 99, "tr", "detail.addFavorite", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8242), "Favorilere Ekle" },
                    { 100, "tr", "detail.removeFavorite", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8243), "Favorilerden Çıkar" },
                    { 101, "tr", "detail.close", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8244), "Kapat" },
                    { 102, "tr", "detail.noteSaved", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8244), "Not kaydedildi!" },
                    { 103, "tr", "detail.addedToFavorites", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8245), "Favorilere eklendi!" },
                    { 104, "tr", "detail.removedFromFavorites", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8246), "Favorilerden çıkarıldı." },
                    { 105, "tr", "login.sub", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8248), "Multinet entegre öneri sistemi" },
                    { 106, "tr", "login.taglineTitle", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8249), "Bakiyene ve damak tadına göre, yakınındaki doğru öğün." },
                    { 107, "tr", "login.taglineSub", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8250), "Harcama alışkanlığın ve konumunla eşleşen restoranlar — her gün öğle molasında." },
                    { 108, "tr", "login.welcome", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8251), "Tekrar hoş geldin" },
                    { 109, "tr", "login.lead", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8252), "Öğle molası önerilerin seni bekliyor." },
                    { 110, "tr", "login.email", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8253), "E-posta" },
                    { 111, "tr", "login.emailPlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8254), "deniz@sirket.com" },
                    { 112, "tr", "login.password", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8255), "Şifre" },
                    { 113, "tr", "login.submit", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8256), "Giriş yap" },
                    { 114, "tr", "login.noAccount", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8257), "Hesabın yok mu?" },
                    { 115, "tr", "login.registerLink", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8258), "Kayıt ol" },
                    { 116, "tr", "login.errorMsg", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8259), "E-posta adresi veya şifre hatalı!" },
                    { 117, "tr", "register.title", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8260), "Hesabını oluştur" },
                    { 118, "tr", "register.lead", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8261), "Akıllı restoran önerilerini hemen keşfet." },
                    { 119, "tr", "register.name", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8262), "Ad Soyad" },
                    { 120, "tr", "register.namePlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8263), "Deniz Yılmaz" },
                    { 121, "tr", "register.email", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8264), "E-posta" },
                    { 122, "tr", "register.emailPlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8265), "deniz@sirket.com" },
                    { 123, "tr", "register.password", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8266), "Şifre" },
                    { 124, "tr", "register.submit", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8267), "Kayıt ol" },
                    { 125, "tr", "register.hasAccount", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8268), "Zaten hesabın var mı?" },
                    { 126, "tr", "register.loginLink", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8269), "Giriş yap" },
                    { 127, "tr", "register.errorMsg", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8270), "Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol edin." },
                    { 128, "en", "UserNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8271), "User not found." },
                    { 129, "en", "InvalidPassword", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8272), "Invalid email or password!" },
                    { 130, "en", "InvalidInput", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8274), "Email and password cannot be empty." },
                    { 131, "en", "InvalidCode", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8275), "Invalid verification code!" },
                    { 132, "en", "LoginSuccess", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8276), "Login successful." },
                    { 133, "en", "RegisterSuccess", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8277), "Registration completed successfully." },
                    { 134, "en", "RestaurantNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8278), "Restaurant not found." },
                    { 135, "en", "EmailAlreadyExists", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8279), "This email address is already in use." },
                    { 136, "en", "Unauthorized", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8280), "You are not authorized for this operation." },
                    { 137, "en", "InvalidToken", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8302), "Invalid or expired session." },
                    { 138, "en", "ExpenditureAdded", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8303), "Expenditure added successfully." },
                    { 139, "en", "FavoriteAdded", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8311), "Restaurant added to favorites." },
                    { 140, "en", "FavoriteRemoved", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8312), "Restaurant removed from favorites." },
                    { 141, "en", "FavoriteNoteUpdated", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8314), "Favorite note updated." },
                    { 142, "en", "RestaurantFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8315), "An error occurred while fetching restaurants." },
                    { 143, "en", "RestaurantGetByIdError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8316), "An error occurred while fetching the restaurant." },
                    { 144, "en", "RestaurantAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8317), "An error occurred while adding the restaurant." },
                    { 145, "en", "RestaurantUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8318), "An error occurred while updating the restaurant." },
                    { 146, "en", "RestaurantDeleteError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8318), "An error occurred while deleting the restaurant." },
                    { 147, "en", "NearbyRestaurantsError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8319), "An error occurred while calculating nearby restaurants." },
                    { 148, "en", "RestaurantToUpdateNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8321), "Restaurant to update not found." },
                    { 149, "en", "RestaurantToDeleteNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8321), "Restaurant to delete not found." },
                    { 150, "en", "FavoriteRestaurantNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8322), "Restaurant to add to favorites not found." },
                    { 151, "en", "FavoriteNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8323), "Favorite record to remove not found." },
                    { 152, "en", "FavoriteAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8324), "An error occurred while adding favorite." },
                    { 153, "en", "FavoriteRemoveError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8325), "An error occurred while removing favorite." },
                    { 154, "en", "FavoritesFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8326), "An error occurred while fetching favorites." },
                    { 155, "en", "UsersFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8327), "An error occurred while fetching users." },
                    { 156, "en", "UserGetByIdError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8328), "An error occurred while fetching the user." },
                    { 157, "en", "UserAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8329), "An error occurred while adding the user." },
                    { 158, "en", "UserUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8330), "An error occurred while updating the user." },
                    { 159, "en", "UserDeleteError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8331), "An error occurred while deleting the user." },
                    { 160, "en", "UserToUpdateNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8332), "User to update not found." },
                    { 161, "en", "UserToDeleteNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8333), "User to delete not found." },
                    { 162, "en", "ExpendituresFetchError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8334), "An error occurred while fetching expenditures." },
                    { 163, "en", "ExpenditureAmountInvalid", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8335), "Expenditure amount must be greater than 0." },
                    { 164, "en", "ExpenditureAddError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8335), "An error occurred while adding expenditure." },
                    { 165, "en", "ExpenditureNotFound", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8336), "Expenditure record to delete not found." },
                    { 166, "en", "ExpenditureDeleteError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8337), "An error occurred while deleting expenditure." },
                    { 167, "en", "ProfileUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8338), "An error occurred while updating profile." },
                    { 168, "en", "PasswordResetCodeSendError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8339), "An error occurred while sending password reset code." },
                    { 169, "en", "PasswordAndCodeRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8340), "Verification code and new password cannot be empty." },
                    { 170, "en", "PasswordMinLength", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8342), "Password must be at least 6 characters long." },
                    { 171, "en", "CodeExpired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8343), "Verification code expired. Please request a new code." },
                    { 172, "en", "PasswordUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8344), "An error occurred while updating password." },
                    { 173, "en", "InvalidEmailAddress", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8346), "Please enter a valid new email address." },
                    { 174, "en", "EmailChangeCodeSendError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8347), "An error occurred while sending email change code." },
                    { 175, "en", "AllFieldsRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8348), "All fields are required." },
                    { 176, "en", "CurrentPasswordIncorrect", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8349), "Current password entered is incorrect!" },
                    { 177, "en", "EmailUpdateError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8350), "An error occurred while updating email address." },
                    { 178, "en", "EmailAndCodeRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8371), "Email and verification code cannot be empty." },
                    { 179, "en", "RegisterFieldsRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8372), "Name, email, and password fields are required." },
                    { 180, "en", "LoginError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8373), "An error occurred during login." },
                    { 181, "en", "VerificationError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8375), "An error occurred during verification." },
                    { 182, "en", "RegisterError", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8376), "An error occurred during registration." },
                    { 183, "en", "UnauthorizedProfileUpdate", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8377), "You do not have permission to update another user's profile." },
                    { 184, "en", "UnauthorizedAccountDelete", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8378), "You do not have permission to delete another user's account." },
                    { 185, "en", "PasswordRequired", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8379), "Password field cannot be empty." },
                    { 186, "en", "nav.discover", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8381), "Discover" },
                    { 187, "en", "nav.hello", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8382), "HELLO" },
                    { 188, "en", "nav.logout", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8383), "Logout" },
                    { 189, "en", "profile.backToDiscover", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8384), "Back to Discover Page" },
                    { 190, "en", "profile.myProfile", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8385), "My Profile" },
                    { 191, "en", "sidebar.radiusPrefix", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8386), "Radius" },
                    { 192, "en", "sidebar.title", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8387), "Restaurants near you" },
                    { 193, "en", "sidebar.subtitle", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8388), "Restaurants listed by your location" },
                    { 194, "en", "sidebar.radiusLabel", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8389), "Distance Limit (Radius)" },
                    { 195, "en", "sidebar.loading", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8390), "Loading..." },
                    { 196, "en", "sidebar.distance", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8391), "Distance" },
                    { 197, "en", "sidebar.noRestaurants", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8392), "No restaurants found around you within the selected radius." },
                    { 198, "en", "badge.recommendation", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8393), "%{score} matches your Multinet profile and location" },
                    { 199, "en", "map.loading", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8394), "Loading Map..." },
                    { 200, "en", "map.userMarker", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8395), "You are here" },
                    { 201, "en", "map.category", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8396), "Category" },
                    { 202, "en", "map.price", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8397), "Price" },
                    { 203, "en", "map.walk", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8398), "Walk" },
                    { 204, "en", "map.match", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8399), "Match" },
                    { 205, "en", "map.locationScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8399), "Location" },
                    { 206, "en", "map.budgetScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8401), "Budget" },
                    { 207, "en", "filter.search", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8402), "Search Restaurant" },
                    { 208, "en", "filter.searchPlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8402), "Type restaurant name..." },
                    { 209, "en", "filter.category", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8403), "Category" },
                    { 210, "en", "filter.allCategories", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8404), "All" },
                    { 211, "en", "filter.price", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8405), "Price Range" },
                    { 212, "en", "filter.allPrices", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8406), "All" },
                    { 213, "en", "filter.noResults", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8407), "No restaurants match the filters." },
                    { 214, "en", "filter.clearFilters", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8408), "Clear Filters" },
                    { 215, "en", "filter.resultCount", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8409), "{count} restaurants listed" },
                    { 216, "en", "detail.category", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8410), "Category" },
                    { 217, "en", "detail.price", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8411), "Average Price" },
                    { 218, "en", "detail.distance", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8412), "Walking Distance" },
                    { 219, "en", "detail.duration", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8414), "Walking Duration" },
                    { 220, "en", "detail.score", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8415), "Recommendation Score" },
                    { 221, "en", "detail.locationScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8416), "Location Score" },
                    { 222, "en", "detail.budgetScore", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8417), "Budget Score" },
                    { 223, "en", "detail.favoriteNote", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8418), "Your Favorite Note" },
                    { 224, "en", "detail.favoriteNotePlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8419), "Add a note about this venue..." },
                    { 225, "en", "detail.saveNote", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8420), "Save Note" },
                    { 226, "en", "detail.addFavorite", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8421), "Add to Favorites" },
                    { 227, "en", "detail.removeFavorite", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8422), "Remove from Favorites" },
                    { 228, "en", "detail.close", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8423), "Close" },
                    { 229, "en", "detail.noteSaved", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8425), "Note saved!" },
                    { 230, "en", "detail.addedToFavorites", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8426), "Added to favorites!" },
                    { 231, "en", "detail.removedFromFavorites", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8427), "Removed from favorites." },
                    { 232, "en", "login.sub", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8428), "Multinet integrated recommendation system" },
                    { 233, "en", "login.taglineTitle", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8429), "The right meal near you, based on your balance and taste." },
                    { 234, "en", "login.taglineSub", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8430), "Restaurants matching your spending habits and location — every day at lunch." },
                    { 235, "en", "login.welcome", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8431), "Welcome back" },
                    { 236, "en", "login.lead", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8432), "Your lunch break recommendations are waiting." },
                    { 237, "en", "login.email", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8432), "Email" },
                    { 238, "en", "login.emailPlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8433), "deniz@company.com" },
                    { 239, "en", "login.password", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8434), "Password" },
                    { 240, "en", "login.submit", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8435), "Sign in" },
                    { 241, "en", "login.noAccount", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8436), "Don't have an account?" },
                    { 242, "en", "login.registerLink", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8437), "Register" },
                    { 243, "en", "login.errorMsg", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8438), "Invalid email address or password!" },
                    { 244, "en", "register.title", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8439), "Create your account" },
                    { 245, "en", "register.lead", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8440), "Discover smart restaurant recommendations now." },
                    { 246, "en", "register.name", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8441), "Full Name" },
                    { 247, "en", "register.namePlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8441), "Deniz Yilmaz" },
                    { 248, "en", "register.email", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8443), "Email" },
                    { 249, "en", "register.emailPlaceholder", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8444), "deniz@company.com" },
                    { 250, "en", "register.password", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8445), "Password" },
                    { 251, "en", "register.submit", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8447), "Register" },
                    { 252, "en", "register.hasAccount", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8448), "Already have an account?" },
                    { 253, "en", "register.loginLink", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8449), "Sign in" },
                    { 254, "en", "register.errorMsg", new DateTime(2026, 8, 20, 10, 9, 53, 125, DateTimeKind.Utc).AddTicks(8450), "Registration failed. Please check your information." }
                });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 1,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 3,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 4,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 5,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 6,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 7,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 8,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 9,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 10,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 11,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 12,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 13,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 14,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 15,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 16,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 17,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 18,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 19,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 20,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 21,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 22,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 23,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 24,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 25,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 26,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 27,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 28,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 29,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 30,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 31,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 32,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 33,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 34,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 35,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 36,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 37,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 38,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 39,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 40,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 41,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 42,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 43,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 44,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 45,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 46,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 47,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 48,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 49,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 50,
                column: "OwnerId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "RestaurantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "RestaurantId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RestaurantId",
                table: "Users",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_Culture_ResourceKey",
                table: "LocalizationResources",
                columns: new[] { "Culture", "ResourceKey" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Restaurants_RestaurantId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "LocalizationResources");

            migrationBuilder.DropIndex(
                name: "IX_Users_RestaurantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpiresAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");
        }
    }
}
