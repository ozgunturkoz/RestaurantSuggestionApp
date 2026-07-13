using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;


namespace RestaurantSuggestionApp.Business.Services
{
    public interface IAuthService
    {
        User Login(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public User Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || user.PasswordHash != password)
            {
                return null; 
            }
            return user; 
        }
    }
}