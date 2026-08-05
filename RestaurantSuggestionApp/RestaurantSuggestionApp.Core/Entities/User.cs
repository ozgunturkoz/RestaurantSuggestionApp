namespace RestaurantSuggestionApp.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public string? VerificationCode { get; private set; }
        public DateTime? VerificationCodeExpiresAt { get; private set; }

        public ICollection<UserExpenditure>? Expenditures { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<UserFavorite>? Favorites { get; set; }

        public User() { }

        public User(int id, string name, string email, string passwordHash)
        {
            Id = id;
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

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Şifre boş olamaz.");
            }
            PasswordHash = passwordHash;
        }

        public void Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            UpdateProfile(user.Name, user.Email);
        }
    }
}

