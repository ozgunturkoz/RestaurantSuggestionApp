using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.Entities;
using System;
namespace RestaurantSuggestionApp.Core.Entities
{
    public class User
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public string? VerificationCode { get; private set; }
        public DateTime? VerificationCodeExpiresAt { get; private set; }

      
        public int? RestaurantId { get; private set; }
        public Restaurant? Restaurant { get; set; }

        public ICollection<UserExpenditure>? Expenditures { get; set; }
        
        public ICollection<UserFavorite>? Favorites { get; set; }

        public User() { }

        public void AssignRestaurant(Restaurant restaurant)
        {
            if (restaurant != null)
            {
                RestaurantId = restaurant.Id;
                Restaurant = restaurant;
            }
        }

        public User(string name, string email, string password)
        {
            UpdateProfile(name, email);
            SetPasswordHash(BCrypt.Net.BCrypt.HashPassword(password));
        }

        public User(int id, string name, string email, string passwordHash)
        {
            if (id != 0)
            {
                Id = id;
            }
            UpdateProfile(name, email);
            SetPasswordHash(passwordHash);
        }

        


        public void SetVerificationCode(string code, int expireMinutes = 5)
        {
            VerificationCode = code;
            VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);
        }

        public void ClearVerificationCode()
        {
            VerificationCode = null;
            VerificationCodeExpiresAt = null;
        }

        public void Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            UpdateProfile(user.Name, user.Email);
        }

        public void UpdateProfile(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                throw new ArgumentException("Kullanıcı ismi en az 3 karakter olmalıdır.");
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Geçerli bir e-posta adresi giriniz.");
            }

            Name = name.Trim();
            Email = email.Trim().ToLower();
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Geçerli bir e-posta adresi giriniz.");
            }
            Email = email.Trim().ToLower();
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Şifre boş olamaz.");
            }
            PasswordHash = passwordHash;
        }

      

       
    }
}

