using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Request.User;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetById(id);
            if (result == null)
                return NotFound("Aradığınız kullanıcı bulunamadı.");

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest createUserRequest)
        {
            var newUser = new User
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                PasswordHash = createUserRequest.Password 
            };

            var result = _userService.Add(newUser);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var updateUser = new User
            {
                Name = updateUserRequest.Name,
                Email = updateUserRequest.Email,
                PasswordHash = updateUserRequest.Password
            };

            var result = _userService.Update(id, updateUser);
            if (result == null)
                return NotFound("Güncellenecek kullanıcı bulunamadı.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok("İşlem başarılı.");
        }
    }
}