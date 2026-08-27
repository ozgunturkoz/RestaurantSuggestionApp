using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        Task<BaseResponse<LoginResponseDto>> Register(string name, string email, string password, string confirmPassword);

        Task<BaseResponse<string>> VerifyRegister(string email, string code);

        Task<BaseResponse<bool>> ForgotPassword(string email);

        Task<BaseResponse<bool>> ResetPasswordWithCode(string email, string code, string newPassword, string confirmPassword);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly IEmailService _emailService;

        private readonly IStringLocalizer _localizer;

        private readonly IServiceScopeFactory _scopeFactory;

        private static readonly System.Text.RegularExpressions.Regex EmailRegex = new System.Text.RegularExpressions.Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService, IStringLocalizer localizer, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _localizer = localizer;
            _scopeFactory = scopeFactory;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email.Trim());
        }

        public async Task<BaseResponse<LoginResponseDto>> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidInput"]);
                }

                if (!IsValidEmail(email))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidEmailFormat"]);
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

                var toEmail = user.Email;
                var subject = _localizer["LoginVerificationEmailSubject"].Value;
                var body = _localizer["LoginVerificationEmailBody", user.Name, randomCode].Value;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        await emailService.SendEmailAsync(toEmail, subject, body);
                    }
                    catch
                    {
                    }
                });

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

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code.Trim())
                {
                    return BaseResponse<string>.Fail(_localizer["InvalidCode"]);
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<string>.Fail(_localizer["CodeExpired"]);
                }

                user.ClearVerificationCode();
                _context.SaveChanges();

                var toEmail = user.Email;
                var subject = _localizer["WelcomeEmailSubject"].Value;
                var body = _localizer["WelcomeEmailBody", user.Name].Value;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        await emailService.SendEmailAsync(toEmail, subject, body);
                    }
                    catch
                    {
                    }
                });

                var token = GenerateJwtToken(user);
                return BaseResponse<string>.Success(token);
            }
            catch (Exception)
            {
                return BaseResponse<string>.Fail(_localizer["VerificationError"]);
            }
        }

        public async Task<BaseResponse<LoginResponseDto>> Register(string name, string email, string password, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["RegisterFieldsRequired"]);
                }

                if (!IsValidEmail(email))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidEmailFormat"]);
                }

                if (password.Length < 6)
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["PasswordMinLength"]);
                }

                if (password != confirmPassword)
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["PasswordsDoNotMatch"]);
                }

                var cleanEmail = email.Trim().ToLower();

                if (_context.Users.Any(u => u.Email.ToLower() == cleanEmail))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["EmailAlreadyExists"]);
                }

                var newUser = new User(name.Trim(), cleanEmail, password);

                var randomCode = new Random().Next(100000, 999999).ToString();
                newUser.SetVerificationCode(randomCode, expireMinutes: 10);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                var toEmail = newUser.Email;
                var subject = _localizer["RegisterVerificationEmailSubject"].Value;
                var body = _localizer["RegisterVerificationEmailBody", newUser.Name, randomCode].Value;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        await emailService.SendEmailAsync(toEmail, subject, body);
                    }
                    catch
                    {
                    }
                });

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

        public async Task<BaseResponse<string>> VerifyRegister(string email, string code)
        {
            return await VerifyLogin(email, code);
        }

        public async Task<BaseResponse<bool>> ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidInput"]);
                }

                if (!IsValidEmail(email))
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidEmailFormat"]);
                }

                var cleanEmail = email.Trim().ToLower();
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == cleanEmail);

                if (user == null)
                {
                    return BaseResponse<bool>.Fail(_localizer["UserNotFound"]);
                }

                var randomCode = new Random().Next(100000, 999999).ToString();
                user.SetVerificationCode(randomCode, expireMinutes: 10);
                _context.SaveChanges();

                var toEmail = user.Email;
                var subject = _localizer["ForgotPasswordEmailSubject"].Value;
                var body = _localizer["ForgotPasswordEmailBody", user.Name, randomCode].Value;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        await emailService.SendEmailAsync(toEmail, subject, body);
                    }
                    catch
                    {
                    }
                });

                return BaseResponse<bool>.Success(true, _localizer["ForgotPasswordSuccess"]);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["PasswordResetCodeSendError"]);
            }
        }

        public async Task<BaseResponse<bool>> ResetPasswordWithCode(string email, string code, string newPassword, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return BaseResponse<bool>.Fail(_localizer["AllFieldsRequired"]);
                }

                if (!IsValidEmail(email))
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidEmailFormat"]);
                }

                if (newPassword.Length < 6)
                {
                    return BaseResponse<bool>.Fail(_localizer["PasswordMinLength"]);
                }

                if (newPassword != confirmPassword)
                {
                    return BaseResponse<bool>.Fail(_localizer["PasswordsDoNotMatch"]);
                }

                var cleanEmail = email.Trim().ToLower();
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == cleanEmail);

                if (user == null)
                {
                    return BaseResponse<bool>.Fail(_localizer["UserNotFound"]);
                }

                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code.Trim())
                {
                    return BaseResponse<bool>.Fail(_localizer["InvalidCode"]);
                }

                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<bool>.Fail(_localizer["CodeExpired"]);
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.SetPasswordHash(hashedPassword);
                user.ClearVerificationCode();
                _context.SaveChanges();

                return BaseResponse<bool>.Success(true, _localizer["PasswordResetSuccess"]);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["PasswordUpdateError"]);
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
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
