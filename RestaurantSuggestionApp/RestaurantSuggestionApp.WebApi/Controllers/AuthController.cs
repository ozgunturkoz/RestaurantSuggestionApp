using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.DTOs.Requests;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("AuthRateLimit")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        
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
    }
}