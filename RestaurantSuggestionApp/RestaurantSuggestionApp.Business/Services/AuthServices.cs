using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IAuthService
    {
        BaseResponse<string> Login(string email, string password);
        BaseResponse<string> Register(string name, string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public BaseResponse<string> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<string>.Fail("E-posta ve şifre boş bırakılamaz.");
                }

                // 1. Adım: E-postaya sahip kullanıcıyı bul
                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                // 2. Adım: Kullanıcı yoksa veya şifre eşleşmiyorsa hata dön
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return BaseResponse<string>.Fail("E-posta veya şifre hatalı!");
                }

                // 3. Adım: Giriş başarılıysa JWT Token üret
                var token = GenerateJwtToken(user);
                return BaseResponse<string>.Success(token);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.Fail($"Giriş işlemi sırasında bir hata oluştu: {ex.Message}");
            }
        }

        public BaseResponse<string> Register(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<string>.Fail("Ad, e-posta ve şifre alanları boş bırakılamaz.");
                }

                // 1. Adım: E-posta adresi sistemde zaten var mı?
                if (_context.Users.Any(u => u.Email == email))
                {
                    return BaseResponse<string>.Fail("Bu e-posta adresi zaten kullanılmaktadır!");
                }

                // 2. Adım: Şifreyi BCrypt ile güvenli bir şekilde hashleme
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // 3. Adım: Yeni kullanıcı nesnesini oluşturma
                var newUser = new User(0, name, email, hashedPassword);

                // 4. Adım: Veritabanına kaydetme
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // 5. Adım: Kayıt olan kullanıcıya hemen giriş yapabilmesi için JWT Token üretme
                var token = GenerateJwtToken(newUser);
                return BaseResponse<string>.Success(token);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.Fail($"Kayıt işlemi sırasında bir hata oluştu: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}