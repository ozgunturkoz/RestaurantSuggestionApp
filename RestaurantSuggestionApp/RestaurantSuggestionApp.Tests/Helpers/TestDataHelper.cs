using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;

namespace RestaurantSuggestionApp.Tests.Helpers
{
    public static class TestDataHelper
    {
        /// <summary>
        /// Testler için hızlıca şifresi hashlenmiş ortak bir kullanıcı kaydı oluşturup DB'ye kaydeder.
        /// </summary>
        public static User SeedUser(AppDbContext context, string name = "Test User", string email = "test@mail.com", string password = "sifre123")
        {
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        /// <summary>
        /// Testler için hızlıca ortak bir restoran kaydı oluşturup DB'ye kaydeder.
        /// </summary>
        public static Restaurant SeedRestaurant(AppDbContext context, string name = "Test Restaurant", string category = "Kebap", decimal lat = 41.0082M, decimal lng = 28.9784M, decimal avgPrice = 100.0M)
        {
            var restaurant = new Restaurant
            {
                Name = name,
                Category = category,
                Latitude = lat,
                Longitude = lng,
                AveragePrice = avgPrice
            };

            context.Restaurants.Add(restaurant);
            context.SaveChanges();
            return restaurant;
        }
    }
}
