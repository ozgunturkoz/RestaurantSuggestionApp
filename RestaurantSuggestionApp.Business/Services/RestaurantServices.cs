using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;

namespace RestaurantSuggestionApp.Business.Services
{

    public interface IRestaurantService
    {
        List<Restaurant> GetAll();
        Restaurant GetById(int id);
        Restaurant Add(Restaurant newRestaurant);
        Restaurant Update(int id, Restaurant updatedRestaurant);
        void Delete(int id);
    }

    public class RestaurantServices
    {
        
        
        public class RestaurantService : IRestaurantService
        {
            private readonly AppDbContext _context;

            public RestaurantService(AppDbContext context)
            {
                _context = context;
            }

            public List<Restaurant> GetAll()
            {
                return _context.Restaurants.ToList();
            }

            public Restaurant GetById(int id)
            {
                return _context.Restaurants.FirstOrDefault(r => r.Id == id);
            }

            public Restaurant Add(Restaurant newRestaurant)
            {
                _context.Restaurants.Add(newRestaurant);
                _context.SaveChanges();
                return newRestaurant;
            }

            public Restaurant Update(int id, Restaurant updatedRestaurant)
            {
                var existing = _context.Restaurants.FirstOrDefault(r => r.Id == id);
                if (existing != null)
                {
                    existing.Name = updatedRestaurant.Name;
                    existing.Category = updatedRestaurant.Category;
                    existing.AveragePrice = updatedRestaurant.AveragePrice;
                    existing.Latitude = updatedRestaurant.Latitude;
                    existing.Longitude = updatedRestaurant.Longitude;
                    _context.SaveChanges();
                }
                return existing; 
            }

            public void Delete(int id)
            {
                var existing = _context.Restaurants.FirstOrDefault(r => r.Id == id);
                if (existing != null)
                {
                    _context.Restaurants.Remove(existing);
                    _context.SaveChanges();
                }
            }
        }
    }
}