using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Request.User;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; 

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var user = _authService.Login(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return BadRequest("E-posta veya şifre hatalı!");
            }

            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email
            });
        }
    }
}