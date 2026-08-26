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
    // =========================================================================================
    // BU ARAYÜZ (INTERFACE) NE İŞE YARAR?
    // Bir sözleşme veya menü gibidir. Kimlik doğrulama (Giriş yapma, Kayıt olma, Şifremi unuttum,
    // E-posta doğrulama) için hangi işlemlerin yapılabileceğini dış dünyaya duyurur.
    // =========================================================================================
    public interface IAuthService
    {
        // Kullanıcının e-posta ve şifreyle sisteme giriş yapmasını sağlayan işlem
        Task<BaseResponse<LoginResponseDto>> Login(string email, string password);

        // Giriş yaparken e-postaya gönderilen 6 haneli güvenlik kodunu onaylayan işlem
        Task<BaseResponse<string>> VerifyLogin(string email, string code);

        // Yeni bir kullanıcının sisteme ilk kaydını oluşturan işlem
        Task<BaseResponse<LoginResponseDto>> Register(string name, string email, string password, string confirmPassword);

        // Kayıt sonrası e-postaya gelen aktivasyon kodunu onaylayan işlem
        Task<BaseResponse<string>> VerifyRegister(string email, string code);

        // Şifresini unutan kullanıcıya e-posta ile sıfırlama kodu gönderen işlem
        Task<BaseResponse<bool>> ForgotPassword(string email);

        // E-postaya gelen kod ile yeni şifreyi belirleyip kaydeden işlem
        Task<BaseResponse<bool>> ResetPasswordWithCode(string email, string code, string newPassword, string confirmPassword);
    }

    // =========================================================================================
    // BU SINIF (CLASS) NE İŞE YARAR?
    // Yukarıdaki arayüzün (IAuthService) söz verdiği tüm kimlik doğrulama işlemlerinin
    // gerçek mutfağıdır; veritabanı sorgularını yapar, şifreleri kontrol eder, e-postalar atar.
    // =========================================================================================
    public class AuthService : IAuthService
    {
        // Veritabanı ile konuşmamızı sağlayan köprü nesnemiz
        private readonly AppDbContext _context;

        // appsettings.json dosyasındaki ayarları (JWT şifresi, süreler vb.) okumamızı sağlayan nesne
        private readonly IConfiguration _configuration;

        // Kullanıcılara doğrulama e-postaları atmamızı sağlayan servis
        private readonly IEmailService _emailService;

        // Hata ve başarı mesajlarını kullanıcının diline (TR / EN) çeviren yerelleştirme nesnesi
        private readonly IStringLocalizer _localizer;

        // Arka planda güvenli ve bağımsız e-posta göndermek için yeni servis havuzu açan fabrika
        private readonly IServiceScopeFactory _scopeFactory;

        // E-posta adresinin geçerli bir kalıba (örnek: isim@alanadi.com) uyup uymadığını denetleyen regex kuralı
        private static readonly System.Text.RegularExpressions.Regex EmailRegex = new System.Text.RegularExpressions.Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // AuthService sınıfından bir nesne üretildiği anda çalışır ve ihtiyacımız olan
        // veritabanı, e-posta ve çeviri araçlarını sınıfa teslim eder (Dependency Injection).
        // -------------------------------------------------------------------------------------
        public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService, IStringLocalizer localizer, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _localizer = localizer;
            _scopeFactory = scopeFactory;
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Girilen metnin gerçek bir e-posta formatında olup olmadığını kontrol eder.
        // -------------------------------------------------------------------------------------
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email.Trim());
        }

        // -------------------------------------------------------------------------------------
        // 1. GİRİŞ YAPMA İŞLEMİ (Login):
        // Kullanıcı e-posta ve şifresini yazar. Bilgiler doğruysa e-postasına 6 haneli kod atar.
        // -------------------------------------------------------------------------------------
        public async Task<BaseResponse<LoginResponseDto>> Login(string email, string password)
        {
            try
            {
                // Alanlar boş mu kontrol ediyoruz
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidInput"]);
                }

                // E-posta formatı doğru mu kontrol ediyoruz
                if (!IsValidEmail(email))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidEmailFormat"]);
                }

                // E-postadaki boşlukları temizleyip küçük harfe çeviriyoruz
                var cleanEmail = email.Trim().ToLower();

                // Veritabanında bu e-postaya sahip kullanıcıyı arıyoruz
                var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == cleanEmail);

                // Kullanıcı yoksa veya girilen şifre veritabanındaki kriptolu şifreyle uyuşmuyorsa hata veriyoruz
                if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["InvalidPassword"]);
                }

                // 100000 ile 999999 arasında 6 haneli rastgele bir güvenlik kodu üretiyoruz
                var randomCode = new Random().Next(100000, 999999).ToString();

                // Kodu kullanıcının hesabına 10 dakika geçerli olacak şekilde kaydediyoruz
                user.SetVerificationCode(randomCode, expireMinutes: 10);
                _context.SaveChanges();

                // E-posta şablonunu kullanıcının seçtiği dilde hazırlıyoruz
                var toEmail = user.Email;
                var subject = _localizer["LoginVerificationEmailSubject"].Value;
                var body = _localizer["LoginVerificationEmailBody", user.Name, randomCode].Value;

                // E-posta gönderimini kullanıcının beklemesini engellemek için arka plan görevinde başlatıyoruz
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
                        // E-posta gönderim hatası olursa sistemin çökmemesi için sessizce yakalanır
                    }
                });

                // Kullanıcıya doğrulama adımı gerektiğini bildiriyoruz
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

        // -------------------------------------------------------------------------------------
        // 2. GİRİŞ KODUNU DOĞRULAMA (VerifyLogin):
        // Kullanıcı e-postasına gelen 6 haneli kodu yazar. Kod doğruysa JWT oturum anahtarını üretir.
        // -------------------------------------------------------------------------------------
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

                // Girilen kod kayıtlı kodla uyuşuyor mu kontrol ediyoruz
                if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != code.Trim())
                {
                    return BaseResponse<string>.Fail(_localizer["InvalidCode"]);
                }

                // Kodun 10 dakikalık süresi dolmuş mu kontrol ediyoruz
                if (user.VerificationCodeExpiresAt == null || user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    return BaseResponse<string>.Fail(_localizer["CodeExpired"]);
                }

                // Kod bir defa kullanıldıktan sonra temizlenir
                user.ClearVerificationCode();
                _context.SaveChanges();

                // Kullanıcıya hoş geldiniz e-postası hazırlıyoruz
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
                        // Hata durumunda güvenle devam edilir
                    }
                });

                // Kullanıcı için güvenli JWT Token (dijital kimlik kartı) üretiyoruz
                var token = GenerateJwtToken(user);
                return BaseResponse<string>.Success(token);
            }
            catch (Exception)
            {
                return BaseResponse<string>.Fail(_localizer["VerificationError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 3. YENİ ÜYE KAYDI (Register):
        // Ad, e-posta ve şifre ile yeni kullanıcı hesabı açar ve e-postasına onay kodu iletir.
        // -------------------------------------------------------------------------------------
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

                // Bu e-posta ile daha önce kayıt olunmuş mu kontrol ediyoruz
                if (_context.Users.Any(u => u.Email.ToLower() == cleanEmail))
                {
                    return BaseResponse<LoginResponseDto>.Fail(_localizer["EmailAlreadyExists"]);
                }

                // Yeni kullanıcı varlığı oluşturuyoruz (şifre otomatik olarak BCrypt ile şifrelenir)
                var newUser = new User(name.Trim(), cleanEmail, password);

                // 6 haneli onay kodu üretiyoruz
                var randomCode = new Random().Next(100000, 999999).ToString();
                newUser.SetVerificationCode(randomCode, expireMinutes: 10);

                // Kullanıcıyı veritabanına ekleyip kaydediyoruz
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
                        // Hata durumunda devam edilir
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

        // -------------------------------------------------------------------------------------
        // 4. KAYIT KODUNU ONAYLAMA (VerifyRegister):
        // Kayıt onay kodunu denetler ve başarılıysa kullanıcının oturum açmasını sağlar.
        // -------------------------------------------------------------------------------------
        public async Task<BaseResponse<string>> VerifyRegister(string email, string code)
        {
            return await VerifyLogin(email, code);
        }

        // -------------------------------------------------------------------------------------
        // 5. ŞİFREMİ UNUTTUM KOD GÖNDERME (ForgotPassword):
        // Şifresini unutan kullanıcıya e-posta ile 6 haneli tek kullanımlık sıfırlama kodu iletir.
        // -------------------------------------------------------------------------------------
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

                // Kullanıcı bulunamazsa hata mesajı dönüyoruz
                if (user == null)
                {
                    return BaseResponse<bool>.Fail(_localizer["UserNotFound"]);
                }

                // 6 haneli şifre sıfırlama kodu üretiyoruz
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
                        // Hata durumunda devam edilir
                    }
                });

                return BaseResponse<bool>.Success(true, _localizer["ForgotPasswordSuccess"]);
            }
            catch (Exception)
            {
                return BaseResponse<bool>.Fail(_localizer["PasswordResetCodeSendError"]);
            }
        }

        // -------------------------------------------------------------------------------------
        // 6. ŞİFREYİ KOD İLE SIFIRLAMA (ResetPasswordWithCode):
        // Kullanıcının e-postasına gelen kodu ve yeni şifresini alıp şifresini güvenle günceller.
        // -------------------------------------------------------------------------------------
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

                // Yeni şifreyi BCrypt algoritması ile kırılması imkansız hale getirip kaydediyoruz
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

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Kullanıcı giriş yaptığında sisteme güvenle istek atabilmesi için
        // imzalı bir JWT Token (Dijital Kimlik Kartı) oluşturur.
        // -------------------------------------------------------------------------------------
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            // Token içerisine kullanıcının ID'sini, adını ve e-postasını şifreli mühür olarak ekliyoruz
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("restaurantId", user.RestaurantId?.ToString() ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token 7 gün geçerlidir
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}