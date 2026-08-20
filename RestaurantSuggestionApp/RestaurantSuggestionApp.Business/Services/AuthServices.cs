using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer _localizer;

        public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService, IStringLocalizer localizer)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _localizer = localizer;
        }

        public async Task<BaseResponse<LoginResponseDto>> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidInput"]);
                }

                var cleanEmail = email.Trim().ToLower();
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == cleanEmail);

                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidPassword"]);
                }

                var randomCode = new Random().Next(100000, 999999).ToString();

                user.SetVerificationCode(randomCode, expireMinutes: 10);
                _context.SaveChanges();

                // 1. Terminal / Konsol ekranına bas
                Console.WriteLine("\n=======================================================");
                Console.WriteLine($"🔑 [GİRİŞ KODU] Email: {user.Email} ---> KOD: {randomCode}");
                Console.WriteLine("=======================================================\n");

                // 2. Aynı zamanda gerçek e-posta gönder
                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Giriş Doğrulama Kodunuz - Sofra Restoran",
                        $"<h3>Merhaba {user.Name},</h3><p>Giriş yapabilmek için doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{randomCode}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>"
                    );
                }
                catch (Exception mailEx)
                {
                    Console.WriteLine($"E-posta gönderme uyarısı (Mail sunucusu): {mailEx.Message}");
                }

                return BaseResponse<LoginResponseDto>.Success(new LoginResponseDto
                {
                    RequiresVerification = true,
                    Email = user.Email
                });
            }
            catch (Exception)
            {
                return BaseResponse<LoginResponseDto>.Fail(_localizer["LoginError"]);
            }
        }

        public async Task<BaseResponse<string>> VerifyLogin(string email, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                {
                    return BaseResponse<string>.Fail(_localizer["EmailAndCodeRequired"]);
                }

                var cleanEmail = email.Trim().ToLower();
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == cleanEmail);

                if (user == null)
                {
                    return BaseResponse<string>.Fail(_localizer["UserNotFound"]);
                }

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code)
                {
                    return BaseResponse<string>.Fail(_localizer["InvalidCode"]);
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<string>.Fail(_localizer["InvalidToken"]);
                }

                user.ClearVerificationCode();
                _context.SaveChanges();

                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Sofra Restoran'a Hoş Geldiniz!",
                        $"<h3>Merhaba {user.Name},</h3><p>E-posta adresiniz başarıyla doğrulandı. Hesabınız aktif edildi!</p><p>Sofra Restoran Akıllı Öneri Sistemini keyifle kullanabilirsiniz.</p>"
                    );
                }
                catch (Exception mailEx)
                {
                    Console.WriteLine($"Hoşgeldin e-postası gönderme uyarısı: {mailEx.Message}");
                }

                var token = GenerateJwtToken(user);
                return BaseResponse<string>.Success(token);
            }
            catch (Exception)
            {
                return BaseResponse<string>.Fail(_localizer["VerificationError"]);
            }
        }

        public async Task<BaseResponse<LoginResponseDto>> Register(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["RegisterFieldsRequired"]);
                }

                if (_context.Users.Any(u => u.Email == email))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["EmailAlreadyExists"]);
                }

                var newUser = new User(name, email, password);

                var randomCode = new Random().Next(100000, 999999).ToString();
                newUser.SetVerificationCode(randomCode, expireMinutes: 10);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                // 1. Terminal / Konsol ekranına bas
                Console.WriteLine("\n=======================================================");
                Console.WriteLine($"🔑 [KAYIT DOĞRULAMA KODU] Email: {newUser.Email} ---> KOD: {randomCode}");
                Console.WriteLine("=======================================================\n");

                // 2. Aynı zamanda gerçek e-posta gönder
                try
                {
                    await _emailService.SendEmailAsync(
                        newUser.Email,
                        "Sofra Restoran - Hesabınızı Doğrulayın",
                        $"<h3>Aramıza Hoş Geldiniz {newUser.Name}!</h3><p>Kayıt işleminizi tamamlamak için doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{randomCode}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>"
                    );
                }
                catch (Exception mailEx)
                {
                    Console.WriteLine($"E-posta gönderme uyarısı (Mail sunucusu): {mailEx.Message}");
                }

                return BaseResponse<LoginResponseDto>.Success(new LoginResponseDto
                {
                    RequiresVerification = true,
                    Email = newUser.Email
                });
            }
            catch (Exception)
            {
                return BaseResponse<LoginResponseDto>.Fail(_localizer["RegisterError"]);
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
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("restaurantId", user.RestaurantId?.ToString() ?? "")
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