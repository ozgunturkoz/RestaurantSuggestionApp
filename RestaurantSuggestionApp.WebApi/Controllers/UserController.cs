using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Helpers;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.Core.Dtos.Responses;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Giriş yapmış kullanıcının kendi profil bilgilerini getirir
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = User.GetUserId();
            if (!userId.HasValue)
            {
                return Unauthorized(BaseResponse<User>.Fail("Geçersiz veya süresi dolmuş token."));
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
            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = userDto.Password ?? ""
            };

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
            var currentUserId = User.GetUserId();
            if (currentUserId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, BaseResponse<User>.Fail("Başka bir kullanıcının profilini güncelleme yetkiniz yok."));
            }

            var updateUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = userDto.Password ?? ""
            };

            var result = _userService.Update(id, updateUser);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, BaseResponse<bool>.Fail("Başka bir kullanıcının hesabını silme yetkiniz yok."));
            }

            var result = _userService.Delete(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
