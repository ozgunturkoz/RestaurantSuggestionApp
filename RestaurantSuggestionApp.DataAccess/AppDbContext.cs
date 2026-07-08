using Microsoft.EntityFrameworkCore;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<UserExpenditure> UserExpenditures { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Latitude)
                .HasPrecision(9, 6);

            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Longitude)
                .HasPrecision(9, 6);

            
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.AveragePrice)
                .HasPrecision(18, 2);

            
            modelBuilder.Entity<UserExpenditure>()
                .Property(u => u.Amount)
                .HasPrecision(18, 2);

            
            modelBuilder.SeedData();
            base.OnModelCreating(modelBuilder);
        }
    }
}
