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

            // -------------------------------------------------------------
            // 3. UserExpenditures Seed Data
            // -------------------------------------------------------------
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
        }
    }
}
