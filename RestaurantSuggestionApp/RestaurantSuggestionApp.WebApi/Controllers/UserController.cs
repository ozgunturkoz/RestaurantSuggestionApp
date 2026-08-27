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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IStringLocalizer _localizer;

        public UserController(IUserService userService, IStringLocalizer localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

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

        private User MapToUser(UserDto dto)
        {
            var password = dto.Password ?? throw new ArgumentException(_localizer["PasswordRequired"]);
            return new User(dto.Name, dto.Email, password);
        }

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
