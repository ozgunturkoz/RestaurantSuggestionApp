using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResponseDto>> Login(string email, string password);
        Task<BaseResponse<string>> VerifyLogin(string email, string code);
        Task<BaseResponse<LoginResponseDto>> Register(string name, string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<BaseResponse<LoginResponseDto>> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<LoginResponseDto>.Fail("E-posta ve şifre boş bırakılamaz.");
                }

                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return BaseResponse<LoginResponseDto>.Fail("E-posta veya şifre hatalı!");
                }

                var randomCode = new Random().Next(100000, 999999).ToString();

                user.SetVerificationCode(randomCode, expireMinutes: 5);
                _context.SaveChanges();

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Giriş Doğrulama Kodunuz - Sofra Restoran",
                    $"<h3>Merhaba {user.Name},</h3><p>Giriş yapabilmek için doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{randomCode}</b></p><p>Bu kod 5 dakika süreyle geçerlidir.</p>"
                );

                return BaseResponse<LoginResponseDto>.Success(new LoginResponseDto
                {
                    RequiresVerification = true,
                    Email = user.Email
                });
            }
            catch (Exception ex)
            {
                return BaseResponse<LoginResponseDto>.Fail($"Giriş işlemi sırasında bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<BaseResponse<string>> VerifyLogin(string email, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                {
                    return BaseResponse<string>.Fail("E-posta ve doğrulama kodu boş bırakılamaz.");
                }

                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return BaseResponse<string>.Fail("Kullanıcı bulunamadı.");
                }

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code)
                {
                    return BaseResponse<string>.Fail("Girdiğiniz doğrulama kodu hatalı!");
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<string>.Fail("Doğrulama kodunun süresi dolmuş. Lütfen tekrar kodu isteyin.");
                }

                user.ClearVerificationCode();
                _context.SaveChanges();

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Sofra Restoran'a Hoş Geldiniz!",
                    $"<h3>Merhaba {user.Name},</h3><p>E-posta adresiniz başarıyla doğrulandı. Hesabınız aktif edildi!</p><p>Sofra Restoran Akıllı Öneri Sistemini keyifle kullanabilirsiniz.</p>"
                );

                var token = GenerateJwtToken(user);
                return BaseResponse<string>.Success(token);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.Fail($"Doğrulama işlemi sırasında hata oluştu: {ex.Message}");
            }
        }

        public async Task<BaseResponse<LoginResponseDto>> Register(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<LoginResponseDto>.Fail("Ad, e-posta ve şifre alanları boş bırakılamaz.");
                }

                if (_context.Users.Any(u => u.Email == email))
                {
                    return BaseResponse<LoginResponseDto>.Fail("Bu e-posta adresi zaten kullanılmaktadır!");
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                var newUser = new User(0, name, email, hashedPassword);

                var randomCode = new Random().Next(100000, 999999).ToString();
                newUser.SetVerificationCode(randomCode, expireMinutes: 10);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                await _emailService.SendEmailAsync(
                    newUser.Email,
                    "Sofra Restoran - Hesabınızı Doğrulayın",
                    $"<h3>Aramıza Hoş Geldiniz {newUser.Name}!</h3><p>Kayıt işleminizi tamamlamak için doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{randomCode}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>"
                );

                return BaseResponse<LoginResponseDto>.Success(new LoginResponseDto
                {
                    RequiresVerification = true,
                    Email = newUser.Email
                });
            }
            catch (Exception ex)
            {
                return BaseResponse<LoginResponseDto>.Fail($"Kayıt işlemi sırasında bir hata oluştu: {ex.Message}");
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