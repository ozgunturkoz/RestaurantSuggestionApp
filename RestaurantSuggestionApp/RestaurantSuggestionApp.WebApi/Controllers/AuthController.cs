using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using System.Threading.Tasks;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    // =========================================================================================
    // BU CONTROLLER (KONTROLCÜ) NE İŞE YARAR?
    // Bir restorandaki karşılama görevlisi (Garson/Danışma) gibidir.
    // Kullanıcının internet tarayıcısından veya mobil uygulamasından gelen "Giriş yap",
    // "Kayıt ol", "Şifremi unuttum" gibi HTTP isteklerini kapıda karşılar, AuthService'e iletir
    // ve oradan aldığı sonucu kullanıcıya (Başarılı: 200 OK veya Hatalı: 400 BadRequest) olarak iletir.
    // =========================================================================================
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AuthRateLimit")] // Kötü niyetli kişilerin arka arkaya binlerce şifre denemesini engelleyen hız limiti koruması
    public class AuthController : ControllerBase
    {
        // Arka plandaki asıl işleri yapan servisimize erişim nesnesi
        private readonly IAuthService _authService;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // Controller ayağa kalktığında AuthService nesnesini otomatik olarak içine alır.
        // -------------------------------------------------------------------------------------
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // -------------------------------------------------------------------------------------
        // 1. GİRİŞ İSTEĞİ: POST /api/Auth/login
        // Kullanıcı adı ve şifresini kontrol eder, gerekirse e-postaya doğrulama kodu gönderir.
        // -------------------------------------------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto.Email, loginDto.Password);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 2. GİRİŞ DOĞRULAMA: POST /api/Auth/verify-login
        // Kullanıcının e-postasına gelen 6 haneli kodu doğrular ve oturum anahtarı (JWT) döner.
        // -------------------------------------------------------------------------------------
        [HttpPost("verify-login")]
        public async Task<IActionResult> VerifyLogin([FromBody] VerifyLoginDto verifyLoginDto)
        {
            var result = await _authService.VerifyLogin(verifyLoginDto.Email, verifyLoginDto.Code);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 3. YENİ KAYIT: POST /api/Auth/register
        // Yeni bir kullanıcı oluşturur ve e-postasına aktivasyon kodu iletir.
        // -------------------------------------------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto.Name, registerDto.Email, registerDto.Password, registerDto.ConfirmPassword);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 4. KAYIT DOĞRULAMA: POST /api/Auth/verify-register
        // Kayıt sonrası e-postaya gelen kodu kontrol edip hesabı etkinleştirir.
        // -------------------------------------------------------------------------------------
        [HttpPost("verify-register")]
        public async Task<IActionResult> VerifyRegister([FromBody] VerifyRegisterDto verifyRegisterDto)
        {
            var result = await _authService.VerifyRegister(verifyRegisterDto.Email, verifyRegisterDto.Code);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 5. ŞİFREMİ UNUTTUM KOD İSTEME: POST /api/Auth/forgot-password
        // Şifresini unutan kullanıcı e-posta adresini yazar, sistem o adrese 6 haneli kod gönderir.
        // -------------------------------------------------------------------------------------
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = await _authService.ForgotPassword(dto.Email);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 6. ŞİFREYİ SIFIRLAMA: POST /api/Auth/reset-password
        // E-postaya gelen 6 haneli kod ve yeni belirlenen şifre ile kullanıcının şifresini günceller.
        // -------------------------------------------------------------------------------------
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordWithCode(dto.Email, dto.Code, dto.NewPassword, dto.ConfirmPassword);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}