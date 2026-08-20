using Microsoft.EntityFrameworkCore;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.DataAccess
{
    public static class DbContextSeed
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            
            var users = new List<User>
            {
                new User(1, "Ozgur",  "ozgur@email.com",  "hashed_password_1"),
                new User(2, "Ahmet",  "ahmet@email.com",  "hashed_password_2"),
                new User(3, "Zeynep", "zeynep@email.com", "hashed_password_3")
            };
            modelBuilder.Entity<User>().HasData(users);

            
            var restaurants = new List<Restaurant>();
            var categories = new[] { "Kebab & Grill", "World Cuisine", "Seafood", "Cafe & Dessert", "Home Cooking" };

            
            var regions = new[]
            {
                new { Name = "Kadikoy",   Lat = 40.9901, Lng = 29.0256 },
                new { Name = "Besiktas",  Lat = 41.0428, Lng = 29.0075 },
                new { Name = "Sisli",     Lat = 41.0602, Lng = 28.9877 },
                new { Name = "Fatih",     Lat = 41.0165, Lng = 28.9502 },
                new { Name = "Uskudar",   Lat = 41.0267, Lng = 29.0153 }
            };

            int restaurantId = 1;
            
            var random = new Random(42);

            for (int i = 0; i < 50; i++)
            {
                var region   = regions[i % regions.Length];
                var category = categories[i % categories.Length];

                
                decimal latOffset = (decimal)(random.NextDouble() - 0.5) * 0.02m;
                decimal lngOffset = (decimal)(random.NextDouble() - 0.5) * 0.02m;

                restaurants.Add(new Restaurant(
                    restaurantId,
                    $"{region.Name} {category} Restaurant {restaurantId}",
                    category,
                    (decimal)region.Lat + latOffset,
                    (decimal)region.Lng + lngOffset,
                    random.Next(150, 850)
                ));
                restaurantId++;
            }
            modelBuilder.Entity<Restaurant>().HasData(restaurants);

            var expenditures = new List<UserExpenditure>();
            int expenditureId = 1;
            var seedBaseDate  = new DateTime(2026, 1, 1);

            foreach (var user in users)
            {
                for (int j = 0; j < 5; j++)
                {
                    var targetRestaurant = restaurants[random.Next(restaurants.Count)];
                    decimal amount = Math.Round(targetRestaurant.AveragePrice * (decimal)(0.8 + random.NextDouble() * 0.4), 2);
                    expenditures.Add(new UserExpenditure(
                        expenditureId,
                        user.Id,
                        targetRestaurant.Id,
                        amount,
                        seedBaseDate.AddDays(-random.Next(1, 30))
                    ));
                    expenditureId++;
                }
            }
            modelBuilder.Entity<UserExpenditure>().HasData(expenditures);

            var locResources = new List<LocalizationResource>
            {
                new LocalizationResource(1, "tr", "UserNotFound", "Kullanıcı bulunamadı."),
                new LocalizationResource(2, "tr", "InvalidPassword", "E-posta veya şifre hatalı!"),
                new LocalizationResource(3, "tr", "InvalidInput", "E-posta ve şifre boş bırakılamaz."),
                new LocalizationResource(4, "tr", "InvalidCode", "Girdiğiniz doğrulama kodu hatalı!"),
                new LocalizationResource(5, "tr", "LoginSuccess", "Giriş başarılı."),
                new LocalizationResource(6, "tr", "RegisterSuccess", "Kayıt işlemi başarılı."),
                new LocalizationResource(7, "tr", "RestaurantNotFound", "Restoran bulunamadı."),
                new LocalizationResource(8, "tr", "EmailAlreadyExists", "Bu e-posta adresi zaten kullanımda."),
                new LocalizationResource(9, "tr", "Unauthorized", "Bu işlem için yetkiniz bulunmamaktadır."),
                new LocalizationResource(10, "tr", "InvalidToken", "Geçersiz veya süresi dolmuş oturum."),
                new LocalizationResource(11, "tr", "ExpenditureAdded", "Harcama başarıyla eklendi."),
                new LocalizationResource(12, "tr", "FavoriteAdded", "Restoran favorilere eklendi."),
                new LocalizationResource(13, "tr", "FavoriteRemoved", "Restoran favorilerden çıkarıldı."),
                new LocalizationResource(14, "tr", "FavoriteNoteUpdated", "Favori notu güncellendi."),
                new LocalizationResource(15, "tr", "RestaurantFetchError", "Restoranlar getirilirken hata oluştu."),
                new LocalizationResource(16, "tr", "RestaurantGetByIdError", "Restoran getirilirken hata oluştu."),
                new LocalizationResource(17, "tr", "RestaurantAddError", "Restoran eklenirken hata oluştu."),
                new LocalizationResource(18, "tr", "RestaurantUpdateError", "Restoran güncellenirken hata oluştu."),
                new LocalizationResource(19, "tr", "RestaurantDeleteError", "Restoran silinirken hata oluştu."),
                new LocalizationResource(20, "tr", "NearbyRestaurantsError", "Yakın restoranlar hesaplanırken bir hata oluştu."),
                new LocalizationResource(21, "tr", "RestaurantToUpdateNotFound", "Güncellenecek restoran bulunamadı."),
                new LocalizationResource(22, "tr", "RestaurantToDeleteNotFound", "Silinecek restoran bulunamadı."),
                new LocalizationResource(23, "tr", "FavoriteRestaurantNotFound", "Favoriye eklenecek restoran bulunamadı."),
                new LocalizationResource(24, "tr", "FavoriteNotFound", "Kaldırılacak favori kaydı bulunamadı."),
                new LocalizationResource(25, "tr", "FavoriteAddError", "Favori eklenirken hata oluştu."),
                new LocalizationResource(26, "tr", "FavoriteRemoveError", "Favori kaldırılırken hata oluştu."),
                new LocalizationResource(27, "tr", "FavoritesFetchError", "Favoriler getirilirken hata oluştu."),
                new LocalizationResource(28, "tr", "UsersFetchError", "Kullanıcılar getirilirken hata oluştu."),
                new LocalizationResource(29, "tr", "UserGetByIdError", "Kullanıcı getirilirken hata oluştu."),
                new LocalizationResource(30, "tr", "UserAddError", "Kullanıcı eklenirken hata oluştu."),
                new LocalizationResource(31, "tr", "UserUpdateError", "Kullanıcı güncellenirken hata oluştu."),
                new LocalizationResource(32, "tr", "UserDeleteError", "Kullanıcı silinirken hata oluştu."),
                new LocalizationResource(33, "tr", "UserToUpdateNotFound", "Güncellenecek kullanıcı bulunamadı."),
                new LocalizationResource(34, "tr", "UserToDeleteNotFound", "Silinecek kullanıcı bulunamadı."),
                new LocalizationResource(35, "tr", "ExpendituresFetchError", "Harcamalar getirilirken hata oluştu."),
                new LocalizationResource(36, "tr", "ExpenditureAmountInvalid", "Harcama tutarı 0'dan büyük olmalıdır."),
                new LocalizationResource(37, "tr", "ExpenditureAddError", "Harcama eklenirken hata oluştu."),
                new LocalizationResource(38, "tr", "ExpenditureNotFound", "Silinecek harcama kaydı bulunamadı."),
                new LocalizationResource(39, "tr", "ExpenditureDeleteError", "Harcama silinirken hata oluştu."),
                new LocalizationResource(40, "tr", "ProfileUpdateError", "Profil güncellenirken hata oluştu."),
                new LocalizationResource(41, "tr", "PasswordResetCodeSendError", "Şifre sıfırlama kodu gönderilirken hata oluştu."),
                new LocalizationResource(42, "tr", "PasswordAndCodeRequired", "Doğrulama kodu ve yeni şifre boş bırakılamaz."),
                new LocalizationResource(43, "tr", "PasswordMinLength", "Şifre en az 6 karakter olmalıdır."),
                new LocalizationResource(44, "tr", "CodeExpired", "Doğrulama kodunun süresi dolmuş. Lütfen tekrar kod isteyin."),
                new LocalizationResource(45, "tr", "PasswordUpdateError", "Şifre güncellenirken hata oluştu."),
                new LocalizationResource(46, "tr", "InvalidEmailAddress", "Lütfen geçerli bir yeni e-posta adresi giriniz."),
                new LocalizationResource(47, "tr", "EmailChangeCodeSendError", "E-posta doğrulama kodu gönderilirken hata oluştu."),
                new LocalizationResource(48, "tr", "AllFieldsRequired", "Tüm alanların doldurulması zorunludur."),
                new LocalizationResource(49, "tr", "CurrentPasswordIncorrect", "Girdiğiniz mevcut şifre yanlış!"),
                new LocalizationResource(50, "tr", "EmailUpdateError", "E-posta adresi güncellenirken hata oluştu."),
                new LocalizationResource(51, "tr", "EmailAndCodeRequired", "E-posta ve doğrulama kodu boş bırakılamaz."),
                new LocalizationResource(52, "tr", "RegisterFieldsRequired", "Ad, e-posta ve şifre alanları boş bırakılamaz."),
                new LocalizationResource(53, "tr", "LoginError", "Giriş işlemi sırasında bir hata oluştu."),
                new LocalizationResource(54, "tr", "VerificationError", "Doğrulama işlemi sırasında hata oluştu."),
                new LocalizationResource(55, "tr", "RegisterError", "Kayıt işlemi sırasında bir hata oluştu."),
                new LocalizationResource(56, "tr", "UnauthorizedProfileUpdate", "Başka bir kullanıcının profilini güncelleme yetkiniz yok."),
                new LocalizationResource(57, "tr", "UnauthorizedAccountDelete", "Başka bir kullanıcının hesabını silme yetkiniz yok."),
                new LocalizationResource(58, "tr", "PasswordRequired", "Şifre alanı boş bırakılamaz."),
                new LocalizationResource(59, "tr", "nav.discover", "Keşfet"),
                new LocalizationResource(60, "tr", "nav.hello", "MERHABA"),
                new LocalizationResource(61, "tr", "nav.logout", "Çıkış Yap"),
                new LocalizationResource(62, "tr", "profile.backToDiscover", "Keşfet Sayfasına Dön"),
                new LocalizationResource(63, "tr", "profile.myProfile", "Profilim"),
                new LocalizationResource(64, "tr", "sidebar.radiusPrefix", "Yarıçap"),
                new LocalizationResource(65, "tr", "sidebar.title", "Yakınındaki restoranlar"),
                new LocalizationResource(66, "tr", "sidebar.subtitle", "Konumuna göre listelenen restoranlar"),
                new LocalizationResource(67, "tr", "sidebar.radiusLabel", "Mesafe Sınırı (Yarıçap)"),
                new LocalizationResource(68, "tr", "sidebar.loading", "Yükleniyor..."),
                new LocalizationResource(69, "tr", "sidebar.distance", "Mesafe"),
                new LocalizationResource(70, "tr", "sidebar.noRestaurants", "Seçilen yarıçapta etrafınızda restoran bulunamadı."),
                new LocalizationResource(71, "tr", "badge.recommendation", "%{score} Multinet profilinize ve konumunuza uygun"),
                new LocalizationResource(72, "tr", "map.loading", "Harita Yükleniyor..."),
                new LocalizationResource(73, "tr", "map.userMarker", "Buradasınız"),
                new LocalizationResource(74, "tr", "map.category", "Kategori"),
                new LocalizationResource(75, "tr", "map.price", "Fiyat"),
                new LocalizationResource(76, "tr", "map.walk", "Yürüyüş"),
                new LocalizationResource(77, "tr", "map.match", "Uyum"),
                new LocalizationResource(78, "tr", "map.locationScore", "Konum"),
                new LocalizationResource(79, "tr", "map.budgetScore", "Bütçe"),
                new LocalizationResource(80, "tr", "filter.search", "Restoran Ara"),
                new LocalizationResource(81, "tr", "filter.searchPlaceholder", "Restoran adı yazın..."),
                new LocalizationResource(82, "tr", "filter.category", "Kategori"),
                new LocalizationResource(83, "tr", "filter.allCategories", "Tümü"),
                new LocalizationResource(84, "tr", "filter.price", "Fiyat Aralığı"),
                new LocalizationResource(85, "tr", "filter.allPrices", "Tümü"),
                new LocalizationResource(86, "tr", "filter.noResults", "Filtrelere uygun restoran bulunamadı."),
                new LocalizationResource(87, "tr", "filter.clearFilters", "Filtreleri Temizle"),
                new LocalizationResource(88, "tr", "filter.resultCount", "{count} restoran listeleniyor"),
                new LocalizationResource(89, "tr", "detail.category", "Kategori"),
                new LocalizationResource(90, "tr", "detail.price", "Ortalama Fiyat"),
                new LocalizationResource(91, "tr", "detail.distance", "Yürüme Mesafesi"),
                new LocalizationResource(92, "tr", "detail.duration", "Yürüme Süresi"),
                new LocalizationResource(93, "tr", "detail.score", "Öneri Skoru"),
                new LocalizationResource(94, "tr", "detail.locationScore", "Konum Skoru"),
                new LocalizationResource(95, "tr", "detail.budgetScore", "Bütçe Skoru"),
                new LocalizationResource(96, "tr", "detail.favoriteNote", "Favori Notunuz"),
                new LocalizationResource(97, "tr", "detail.favoriteNotePlaceholder", "Bu mekan hakkında bir not ekleyin..."),
                new LocalizationResource(98, "tr", "detail.saveNote", "Notu Kaydet"),
                new LocalizationResource(99, "tr", "detail.addFavorite", "Favorilere Ekle"),
                new LocalizationResource(100, "tr", "detail.removeFavorite", "Favorilerden Çıkar"),
                new LocalizationResource(101, "tr", "detail.close", "Kapat"),
                new LocalizationResource(102, "tr", "detail.noteSaved", "Not kaydedildi!"),
                new LocalizationResource(103, "tr", "detail.addedToFavorites", "Favorilere eklendi!"),
                new LocalizationResource(104, "tr", "detail.removedFromFavorites", "Favorilerden çıkarıldı."),
                new LocalizationResource(105, "tr", "login.sub", "Multinet entegre öneri sistemi"),
                new LocalizationResource(106, "tr", "login.taglineTitle", "Bakiyene ve damak tadına göre, yakınındaki doğru öğün."),
                new LocalizationResource(107, "tr", "login.taglineSub", "Harcama alışkanlığın ve konumunla eşleşen restoranlar — her gün öğle molasında."),
                new LocalizationResource(108, "tr", "login.welcome", "Tekrar hoş geldin"),
                new LocalizationResource(109, "tr", "login.lead", "Öğle molası önerilerin seni bekliyor."),
                new LocalizationResource(110, "tr", "login.email", "E-posta"),
                new LocalizationResource(111, "tr", "login.emailPlaceholder", "deniz@sirket.com"),
                new LocalizationResource(112, "tr", "login.password", "Şifre"),
                new LocalizationResource(113, "tr", "login.submit", "Giriş yap"),
                new LocalizationResource(114, "tr", "login.noAccount", "Hesabın yok mu?"),
                new LocalizationResource(115, "tr", "login.registerLink", "Kayıt ol"),
                new LocalizationResource(116, "tr", "login.errorMsg", "E-posta adresi veya şifre hatalı!"),
                new LocalizationResource(117, "tr", "register.title", "Hesabını oluştur"),
                new LocalizationResource(118, "tr", "register.lead", "Akıllı restoran önerilerini hemen keşfet."),
                new LocalizationResource(119, "tr", "register.name", "Ad Soyad"),
                new LocalizationResource(120, "tr", "register.namePlaceholder", "Deniz Yılmaz"),
                new LocalizationResource(121, "tr", "register.email", "E-posta"),
                new LocalizationResource(122, "tr", "register.emailPlaceholder", "deniz@sirket.com"),
                new LocalizationResource(123, "tr", "register.password", "Şifre"),
                new LocalizationResource(124, "tr", "register.submit", "Kayıt ol"),
                new LocalizationResource(125, "tr", "register.hasAccount", "Zaten hesabın var mı?"),
                new LocalizationResource(126, "tr", "register.loginLink", "Giriş yap"),
                new LocalizationResource(127, "tr", "register.errorMsg", "Kayıt işlemi başarısız. Lütfen bilgilerinizi kontrol edin."),
                new LocalizationResource(128, "en", "UserNotFound", "User not found."),
                new LocalizationResource(129, "en", "InvalidPassword", "Invalid email or password!"),
                new LocalizationResource(130, "en", "InvalidInput", "Email and password cannot be empty."),
                new LocalizationResource(131, "en", "InvalidCode", "Invalid verification code!"),
                new LocalizationResource(132, "en", "LoginSuccess", "Login successful."),
                new LocalizationResource(133, "en", "RegisterSuccess", "Registration completed successfully."),
                new LocalizationResource(134, "en", "RestaurantNotFound", "Restaurant not found."),
                new LocalizationResource(135, "en", "EmailAlreadyExists", "This email address is already in use."),
                new LocalizationResource(136, "en", "Unauthorized", "You are not authorized for this operation."),
                new LocalizationResource(137, "en", "InvalidToken", "Invalid or expired session."),
                new LocalizationResource(138, "en", "ExpenditureAdded", "Expenditure added successfully."),
                new LocalizationResource(139, "en", "FavoriteAdded", "Restaurant added to favorites."),
                new LocalizationResource(140, "en", "FavoriteRemoved", "Restaurant removed from favorites."),
                new LocalizationResource(141, "en", "FavoriteNoteUpdated", "Favorite note updated."),
                new LocalizationResource(142, "en", "RestaurantFetchError", "An error occurred while fetching restaurants."),
                new LocalizationResource(143, "en", "RestaurantGetByIdError", "An error occurred while fetching the restaurant."),
                new LocalizationResource(144, "en", "RestaurantAddError", "An error occurred while adding the restaurant."),
                new LocalizationResource(145, "en", "RestaurantUpdateError", "An error occurred while updating the restaurant."),
                new LocalizationResource(146, "en", "RestaurantDeleteError", "An error occurred while deleting the restaurant."),
                new LocalizationResource(147, "en", "NearbyRestaurantsError", "An error occurred while calculating nearby restaurants."),
                new LocalizationResource(148, "en", "RestaurantToUpdateNotFound", "Restaurant to update not found."),
                new LocalizationResource(149, "en", "RestaurantToDeleteNotFound", "Restaurant to delete not found."),
                new LocalizationResource(150, "en", "FavoriteRestaurantNotFound", "Restaurant to add to favorites not found."),
                new LocalizationResource(151, "en", "FavoriteNotFound", "Favorite record to remove not found."),
                new LocalizationResource(152, "en", "FavoriteAddError", "An error occurred while adding favorite."),
                new LocalizationResource(153, "en", "FavoriteRemoveError", "An error occurred while removing favorite."),
                new LocalizationResource(154, "en", "FavoritesFetchError", "An error occurred while fetching favorites."),
                new LocalizationResource(155, "en", "UsersFetchError", "An error occurred while fetching users."),
                new LocalizationResource(156, "en", "UserGetByIdError", "An error occurred while fetching the user."),
                new LocalizationResource(157, "en", "UserAddError", "An error occurred while adding the user."),
                new LocalizationResource(158, "en", "UserUpdateError", "An error occurred while updating the user."),
                new LocalizationResource(159, "en", "UserDeleteError", "An error occurred while deleting the user."),
                new LocalizationResource(160, "en", "UserToUpdateNotFound", "User to update not found."),
                new LocalizationResource(161, "en", "UserToDeleteNotFound", "User to delete not found."),
                new LocalizationResource(162, "en", "ExpendituresFetchError", "An error occurred while fetching expenditures."),
                new LocalizationResource(163, "en", "ExpenditureAmountInvalid", "Expenditure amount must be greater than 0."),
                new LocalizationResource(164, "en", "ExpenditureAddError", "An error occurred while adding expenditure."),
                new LocalizationResource(165, "en", "ExpenditureNotFound", "Expenditure record to delete not found."),
                new LocalizationResource(166, "en", "ExpenditureDeleteError", "An error occurred while deleting expenditure."),
                new LocalizationResource(167, "en", "ProfileUpdateError", "An error occurred while updating profile."),
                new LocalizationResource(168, "en", "PasswordResetCodeSendError", "An error occurred while sending password reset code."),
                new LocalizationResource(169, "en", "PasswordAndCodeRequired", "Verification code and new password cannot be empty."),
                new LocalizationResource(170, "en", "PasswordMinLength", "Password must be at least 6 characters long."),
                new LocalizationResource(171, "en", "CodeExpired", "Verification code expired. Please request a new code."),
                new LocalizationResource(172, "en", "PasswordUpdateError", "An error occurred while updating password."),
                new LocalizationResource(173, "en", "InvalidEmailAddress", "Please enter a valid new email address."),
                new LocalizationResource(174, "en", "EmailChangeCodeSendError", "An error occurred while sending email change code."),
                new LocalizationResource(175, "en", "AllFieldsRequired", "All fields are required."),
                new LocalizationResource(176, "en", "CurrentPasswordIncorrect", "Current password entered is incorrect!"),
                new LocalizationResource(177, "en", "EmailUpdateError", "An error occurred while updating email address."),
                new LocalizationResource(178, "en", "EmailAndCodeRequired", "Email and verification code cannot be empty."),
                new LocalizationResource(179, "en", "RegisterFieldsRequired", "Name, email, and password fields are required."),
                new LocalizationResource(180, "en", "LoginError", "An error occurred during login."),
                new LocalizationResource(181, "en", "VerificationError", "An error occurred during verification."),
                new LocalizationResource(182, "en", "RegisterError", "An error occurred during registration."),
                new LocalizationResource(183, "en", "UnauthorizedProfileUpdate", "You do not have permission to update another user's profile."),
                new LocalizationResource(184, "en", "UnauthorizedAccountDelete", "You do not have permission to delete another user's account."),
                new LocalizationResource(185, "en", "PasswordRequired", "Password field cannot be empty."),
                new LocalizationResource(186, "en", "nav.discover", "Discover"),
                new LocalizationResource(187, "en", "nav.hello", "HELLO"),
                new LocalizationResource(188, "en", "nav.logout", "Logout"),
                new LocalizationResource(189, "en", "profile.backToDiscover", "Back to Discover Page"),
                new LocalizationResource(190, "en", "profile.myProfile", "My Profile"),
                new LocalizationResource(191, "en", "sidebar.radiusPrefix", "Radius"),
                new LocalizationResource(192, "en", "sidebar.title", "Restaurants near you"),
                new LocalizationResource(193, "en", "sidebar.subtitle", "Restaurants listed by your location"),
                new LocalizationResource(194, "en", "sidebar.radiusLabel", "Distance Limit (Radius)"),
                new LocalizationResource(195, "en", "sidebar.loading", "Loading..."),
                new LocalizationResource(196, "en", "sidebar.distance", "Distance"),
                new LocalizationResource(197, "en", "sidebar.noRestaurants", "No restaurants found around you within the selected radius."),
                new LocalizationResource(198, "en", "badge.recommendation", "%{score} matches your Multinet profile and location"),
                new LocalizationResource(199, "en", "map.loading", "Loading Map..."),
                new LocalizationResource(200, "en", "map.userMarker", "You are here"),
                new LocalizationResource(201, "en", "map.category", "Category"),
                new LocalizationResource(202, "en", "map.price", "Price"),
                new LocalizationResource(203, "en", "map.walk", "Walk"),
                new LocalizationResource(204, "en", "map.match", "Match"),
                new LocalizationResource(205, "en", "map.locationScore", "Location"),
                new LocalizationResource(206, "en", "map.budgetScore", "Budget"),
                new LocalizationResource(207, "en", "filter.search", "Search Restaurant"),
                new LocalizationResource(208, "en", "filter.searchPlaceholder", "Type restaurant name..."),
                new LocalizationResource(209, "en", "filter.category", "Category"),
                new LocalizationResource(210, "en", "filter.allCategories", "All"),
                new LocalizationResource(211, "en", "filter.price", "Price Range"),
                new LocalizationResource(212, "en", "filter.allPrices", "All"),
                new LocalizationResource(213, "en", "filter.noResults", "No restaurants match the filters."),
                new LocalizationResource(214, "en", "filter.clearFilters", "Clear Filters"),
                new LocalizationResource(215, "en", "filter.resultCount", "{count} restaurants listed"),
                new LocalizationResource(216, "en", "detail.category", "Category"),
                new LocalizationResource(217, "en", "detail.price", "Average Price"),
                new LocalizationResource(218, "en", "detail.distance", "Walking Distance"),
                new LocalizationResource(219, "en", "detail.duration", "Walking Duration"),
                new LocalizationResource(220, "en", "detail.score", "Recommendation Score"),
                new LocalizationResource(221, "en", "detail.locationScore", "Location Score"),
                new LocalizationResource(222, "en", "detail.budgetScore", "Budget Score"),
                new LocalizationResource(223, "en", "detail.favoriteNote", "Your Favorite Note"),
                new LocalizationResource(224, "en", "detail.favoriteNotePlaceholder", "Add a note about this venue..."),
                new LocalizationResource(225, "en", "detail.saveNote", "Save Note"),
                new LocalizationResource(226, "en", "detail.addFavorite", "Add to Favorites"),
                new LocalizationResource(227, "en", "detail.removeFavorite", "Remove from Favorites"),
                new LocalizationResource(228, "en", "detail.close", "Close"),
                new LocalizationResource(229, "en", "detail.noteSaved", "Note saved!"),
                new LocalizationResource(230, "en", "detail.addedToFavorites", "Added to favorites!"),
                new LocalizationResource(231, "en", "detail.removedFromFavorites", "Removed from favorites."),
                new LocalizationResource(232, "en", "login.sub", "Multinet integrated recommendation system"),
                new LocalizationResource(233, "en", "login.taglineTitle", "The right meal near you, based on your balance and taste."),
                new LocalizationResource(234, "en", "login.taglineSub", "Restaurants matching your spending habits and location — every day at lunch."),
                new LocalizationResource(235, "en", "login.welcome", "Welcome back"),
                new LocalizationResource(236, "en", "login.lead", "Your lunch break recommendations are waiting."),
                new LocalizationResource(237, "en", "login.email", "Email"),
                new LocalizationResource(238, "en", "login.emailPlaceholder", "deniz@company.com"),
                new LocalizationResource(239, "en", "login.password", "Password"),
                new LocalizationResource(240, "en", "login.submit", "Sign in"),
                new LocalizationResource(241, "en", "login.noAccount", "Don't have an account?"),
                new LocalizationResource(242, "en", "login.registerLink", "Register"),
                new LocalizationResource(243, "en", "login.errorMsg", "Invalid email address or password!"),
                new LocalizationResource(244, "en", "register.title", "Create your account"),
                new LocalizationResource(245, "en", "register.lead", "Discover smart restaurant recommendations now."),
                new LocalizationResource(246, "en", "register.name", "Full Name"),
                new LocalizationResource(247, "en", "register.namePlaceholder", "Deniz Yilmaz"),
                new LocalizationResource(248, "en", "register.email", "Email"),
                new LocalizationResource(249, "en", "register.emailPlaceholder", "deniz@company.com"),
                new LocalizationResource(250, "en", "register.password", "Password"),
                new LocalizationResource(251, "en", "register.submit", "Register"),
                new LocalizationResource(252, "en", "register.hasAccount", "Already have an account?"),
                new LocalizationResource(253, "en", "register.loginLink", "Sign in"),
                new LocalizationResource(254, "en", "register.errorMsg", "Registration failed. Please check your information.")
            };
            modelBuilder.Entity<LocalizationResource>().HasData(locResources);
        }
    }
}
