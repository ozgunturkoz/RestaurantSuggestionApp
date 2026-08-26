using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    // =========================================================================================
    // BU CONTROLLER (KONTROLCÜ) NE İŞE YARAR?
    // Giriş yapmış kullanıcının profil sayfasındaki tüm isteklerini (profil bilgilerini getirme,
    // adını değiştirme, harcamalarını listeleme/ekleme/silme, favori mekanlarını yönetme,
    // e-posta ve şifre değiştirme kodlarını alma) karşılayan güvenli uç noktadır.
    // [Authorize] etiketi sayesinde sisteme giriş yapmamış yabancılar buraya erişemez.
    // =========================================================================================
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        // Kullanıcı iş mantıklarını yürüten servisimiz
        private readonly IUserService _userService;

        // Mesajları TR / EN çeviren dil desteğimiz
        private readonly IStringLocalizer _localizer;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // Controller ayağa kalktığında servis ve dil yöneticisini içeri alır.
        // -------------------------------------------------------------------------------------
        public UserController(IUserService userService, IStringLocalizer localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

        // -------------------------------------------------------------------------------------
        // 1. KENDİ PROFİL BİLGİLERİMİ GETİR: GET /api/User/me
        // Giriş yapmış olan kullanıcının ID'sini dijital imzasından (Token) okur ve bilgilerini döner.
        // -------------------------------------------------------------------------------------
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<User>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.GetById(userId.Value);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 2. TÜM KULLANICILARI LİSTELE: GET /api/User
        // -------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 3. ID NUMARASINA GÖRE KULLANICI GETİR: GET /api/User/{id}
        // -------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 4. YENİ KULLANICI OLUŞTUR: POST /api/User
        // -------------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult Create([FromBody] UserDto userDto)
        {
            var newUser = MapToUser(userDto);

            var result = _userService.Add(newUser);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 5. KULLANICIYI GÜNCELLE: PUT /api/User/{id}
        // -------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserDto userDto)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, BaseResponse<User>.Fail(_localizer["UnauthorizedProfileUpdate"]));
            }

            var updateUser = MapToUser(userDto);

            var result = _userService.Update(id, updateUser);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 6. KULLANICIYI SİL: DELETE /api/User/{id}
        // -------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, BaseResponse<bool>.Fail(_localizer["UnauthorizedAccountDelete"]));
            }

            var result = _userService.Delete(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 7. FAVORİLERİMİ LİSTELE: GET /api/User/favorites
        // -------------------------------------------------------------------------------------
        [HttpGet("favorites")]
        public IActionResult GetFavorites()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<List<UserFavorite>>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.GetFavorites(userId.Value);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 8. FAVORİLERE EKLE VEYA NOT YAZ: POST /api/User/favorites
        // -------------------------------------------------------------------------------------
        [HttpPost("favorites")]
        public IActionResult AddFavorite([FromBody] AddFavoriteDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<UserFavorite>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.AddFavorite(userId.Value, dto.RestaurantId, dto.CustomNote);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 9. FAVORİLERDEN ÇIKAR: DELETE /api/User/favorites/{restaurantId}
        // -------------------------------------------------------------------------------------
        [HttpDelete("favorites/{restaurantId}")]
        public IActionResult RemoveFavorite(int restaurantId)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<bool>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.RemoveFavorite(userId.Value, restaurantId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 10. HARCAMA DÖKÜMÜMÜ GETİR: GET /api/User/expenditures
        // -------------------------------------------------------------------------------------
        [HttpGet("expenditures")]
        public IActionResult GetExpenditures()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<List<UserExpenditureResponseDto>>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.GetExpenditures(userId.Value);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 11. YENİ HARCAMA KAYDI EKLE: POST /api/User/expenditures
        // -------------------------------------------------------------------------------------
        [HttpPost("expenditures")]
        public IActionResult AddExpenditure([FromBody] CreateExpenditureDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<UserExpenditureResponseDto>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.AddExpenditure(userId.Value, dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 12. HARCAMA KAYDINI SİL: DELETE /api/User/expenditures/{id}
        // -------------------------------------------------------------------------------------
        [HttpDelete("expenditures/{id}")]
        public IActionResult DeleteExpenditure(int id)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<bool>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.DeleteExpenditure(userId.Value, id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 13. PROFİL ADINI GÜNCELLE: PUT /api/User/profile
        // -------------------------------------------------------------------------------------
        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<User>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.UpdateProfileName(userId.Value, dto.Name);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 14. PROFİL İÇİNDEN ŞİFRE DEĞİŞTİRME KODU İSTE: POST /api/User/send-password-code
        // -------------------------------------------------------------------------------------
        [HttpPost("send-password-code")]
        public async Task<IActionResult> SendPasswordCode()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<bool>.Fail(_localizer["InvalidToken"]));
            }

            var result = await _userService.SendPasswordResetCodeAsync(userId.Value);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 15. PROFİL İÇİNDEN KOD İLE ŞİFRE GÜNCELLE: POST /api/User/change-password-with-code
        // -------------------------------------------------------------------------------------
        [HttpPost("change-password-with-code")]
        public IActionResult ChangePasswordWithCode([FromBody] ChangePasswordWithCodeDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<bool>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.ChangePasswordWithCode(userId.Value, dto.Code, dto.NewPassword);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 16. E-POSTA DEĞİŞTİRME KODU İSTE: POST /api/User/send-email-change-code
        // -------------------------------------------------------------------------------------
        [HttpPost("send-email-change-code")]
        public async Task<IActionResult> SendEmailChangeCode([FromBody] SendEmailChangeCodeDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<bool>.Fail(_localizer["InvalidToken"]));
            }

            var result = await _userService.SendEmailChangeCodeAsync(userId.Value, dto.NewEmail);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 17. E-POSTA DEĞİŞİKLİĞİNİ ONAYLA: POST /api/User/change-email-with-code
        // -------------------------------------------------------------------------------------
        [HttpPost("change-email-with-code")]
        public IActionResult ChangeEmailWithCode([FromBody] ChangeEmailWithCodeDto dto)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<bool>.Fail(_localizer["InvalidToken"]));
            }

            var result = _userService.ChangeEmailWithCode(userId.Value, dto.NewEmail, dto.Code, dto.CurrentPassword);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: DTO nesnesini veritabanı User nesnesine çevirir
        // -------------------------------------------------------------------------------------
        private User MapToUser(UserDto dto)
        {
            var password = dto.Password ?? throw new ArgumentException(_localizer["PasswordRequired"]);
            return new User(dto.Name, dto.Email, password);
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: İstek atan kullanıcının Token içerisindeki ID'sini çözümler
        // -------------------------------------------------------------------------------------
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
