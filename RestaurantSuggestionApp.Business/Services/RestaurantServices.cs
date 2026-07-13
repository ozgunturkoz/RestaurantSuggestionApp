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

        List<Restaurant> GetNearby(decimal latitude, decimal longitude, double radiusInKm);
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

            public List<Restaurant> GetNearby(decimal latitude, decimal longitude, double radiusInKm)
            {
                double userLat = (double)latitude;
                double userLng = (double)longitude;

                
                double kmPerLatitudeDegree = 111.0;
                double deltaLat = radiusInKm / kmPerLatitudeDegree;

                double userLatRadians = userLat * Math.PI / 180.0;
                double kmPerLongitudeDegree = 111.0 * Math.Cos(userLatRadians);
                double deltaLng = radiusInKm / kmPerLongitudeDegree;

                
                decimal minLat = (decimal)(userLat - deltaLat);
                decimal maxLat = (decimal)(userLat + deltaLat);
                decimal minLng = (decimal)(userLng - deltaLng);
                decimal maxLng = (decimal)(userLng + deltaLng);

                
                var candidateRestaurants = _context.Restaurants
                    .Where(r => r.Latitude >= minLat && r.Latitude <= maxLat &&
                                r.Longitude >= minLng && r.Longitude <= maxLng)
                    .ToList(); 

                
                double rEarth = 6371.0; 
                return candidateRestaurants.Where(rest =>
                {
                    double restLat = (double)rest.Latitude;
                    double restLng = (double)rest.Longitude;

                    double dLat = ToRadians(restLat - userLat);
                    double dLon = ToRadians(restLng - userLng);

                    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                               Math.Cos(ToRadians(userLat)) * Math.Cos(ToRadians(restLat)) *
                               Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                    double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
                    double distance = rEarth * c;

                    return distance <= radiusInKm;
                }).ToList();
            }

            private static double ToRadians(double val)
            {
                return (Math.PI / 180.0) * val;
            }


        }
    }
}