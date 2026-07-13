using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IUserService
    {
        List<User> GetAll();
        User GetById(int id);
        User Add(User newUser);
        User Update(int id, User updatedUser);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Add(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public User Update(int id, User updatedUser)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existing != null)
            {
                existing.Name = updatedUser.Name;
                existing.Email = updatedUser.Email;
                if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
                {
                    existing.PasswordHash = updatedUser.PasswordHash;
                }
                _context.SaveChanges();
            }
            return existing;
        }

        public void Delete(int id)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Id == id);
            if (existing != null)
            {
                _context.Users.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}