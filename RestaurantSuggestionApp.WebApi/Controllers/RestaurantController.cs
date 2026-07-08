using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Dtos;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _restaurantService.GetById(id);
            if (result == null)
                return NotFound("Aradığınız restoran bulunamadı.");

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] RestaurantDto restaurantDto)
        {
            var newRestaurant = new Restaurant
            {
                Name = restaurantDto.Name,
                Category = restaurantDto.Category,
                Latitude = restaurantDto.Latitude,
                Longitude = restaurantDto.Longitude,
                AveragePrice = restaurantDto.AveragePrice
            };

            var result = _restaurantService.Add(newRestaurant);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RestaurantDto restaurantDto)
        {
            var newRestaurant = new Restaurant
            {
                Name = restaurantDto.Name,
                Category = restaurantDto.Category,
                Latitude = restaurantDto.Latitude,
                Longitude = restaurantDto.Longitude,
                AveragePrice = restaurantDto.AveragePrice
            };

            var result = _restaurantService.Update(id, newRestaurant);
            if (result == null)
                return NotFound("Güncellenecek restoran bulunamadı.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _restaurantService.Delete(id);
            return Ok("İşlem başarılı.");
        }
    }
}
