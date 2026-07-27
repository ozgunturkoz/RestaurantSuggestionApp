using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Helpers;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _restaurantService.GetAll();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _restaurantService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] RestaurantDto restaurantDto)
        {
            var newRestaurant = MapToRestaurant(restaurantDto);

            var result = _restaurantService.Add(newRestaurant);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RestaurantDto restaurantDto)
        {
            var newRestaurant = MapToRestaurant(restaurantDto);

            var result = _restaurantService.Update(id, newRestaurant);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _restaurantService.Delete(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] double radiusInKm = 5)
        {
            int? userId = User.GetUserId();

            var result = await _restaurantService.GetNearbyAsync(latitude, longitude, radiusInKm, userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        private Restaurant MapToRestaurant(RestaurantDto dto)
        {
            return new Restaurant
            {
                Name = dto.Name,
                Category = dto.Category,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                AveragePrice = dto.AveragePrice
            };
        }
    }
}
