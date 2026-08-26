using System;
using System.Collections.Generic;

namespace RestaurantSuggestionApp.Core.Entities
{
    // =========================================================================================
    // BU VARLIK (ENTITY) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Veritabanındaki `Users` (Kullanıcılar) tablosunu temsil eder.
    // Sisteme kaydolan her kişinin adını, e-posta adresini, güvenli şifrelenmiş parolasını (BCrypt Hash)
    // ve iki adımlı doğrulama (2FA) / şifre sıfırlama için üretilen 6 haneli geçici güvenlik kodunu saklar.
    // =========================================================================================
    public class User
    {
        // Kullanıcının benzersiz kimlik numarası (ID)
        public int Id { get; private set; }

        // Kullanıcının adı ve soyadı
        public string Name { get; private set; } = string.Empty;

        // Kullanıcının e-posta adresi (giriş için kullanılır)
        public string Email { get; private set; } = string.Empty;

        // Kullanıcının açık şifresi ASLA düz metin olarak tutulmaz; kırılması imkansız BCrypt ile şifrelenmiş hali tutulur
        public string PasswordHash { get; private set; } = string.Empty;

        // E-postaya gönderilen 6 haneli geçici doğrulama kodu
        public string? VerificationCode { get; private set; }

        // Doğrulama kodunun son geçerlilik tarihi ve saati (örn: 10 dakika sonra)
        public DateTime? VerificationCodeExpiresAt { get; private set; }

        // Eğer kullanıcı bir restoran sahibi veya yöneticisiyse ilişkili restoran ID'si
        public int? RestaurantId { get; private set; }
        public Restaurant? Restaurant { get; set; }

        // Kullanıcının sistemdeki tüm harcama hareketleri
        public ICollection<UserExpenditure>? Expenditures { get; set; }
        
        // Kullanıcının favorilerine eklediği restoranlar
        public ICollection<UserFavorite>? Favorites { get; set; }

        // Boş Kurucu Metot (Entity Framework için gereklidir)
        public User() { }

        // Yeni üye kaydı sırasında kullanılan Kurucu Metot (Şifreyi otomatik BCrypt ile hash'ler)
        public User(string name, string email, string password)
        {
            UpdateProfile(name, email);
            SetPasswordHash(BCrypt.Net.BCrypt.HashPassword(password));
        }

        // Başlangıç Seed verileri veya var olan kullanıcıları yüklemek için kullanılan Kurucu Metot
        public User(int id, string name, string email, string passwordHash)
        {
            if (id != 0)
            {
                Id = id;
            }
            UpdateProfile(name, email);
            SetPasswordHash(passwordHash);
        }

        // Restoran atama metodu
        public void AssignRestaurant(Restaurant restaurant)
        {
            if (restaurant != null)
            {
                RestaurantId = restaurant.Id;
                Restaurant = restaurant;
            }
        }

        // 6 haneli doğrulama kodu ve geçerlilik süresi belirleyen metot
        public void SetVerificationCode(string code, int expireMinutes = 5)
        {
            VerificationCode = code;
            VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);
        }

        // Doğrulama kodu kullanıldıktan sonra temizleyen metot
        public void ClearVerificationCode()
        {
            VerificationCode = null;
            VerificationCodeExpiresAt = null;
        }

        // Kullanıcı nesnesini güncelleyen metot
        public void Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            UpdateProfile(user.Name, user.Email);
        }

        // Profil adını ve e-postasını güvenlik kontrolleriyle güncelleyen metot
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

        // E-posta adresini güncelleyen metot
        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Geçerli bir e-posta adresi giriniz.");
            }
            Email = email.Trim().ToLower();
        }

        // Şifrelenmiş parolayı (Hash) atayan metot
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
