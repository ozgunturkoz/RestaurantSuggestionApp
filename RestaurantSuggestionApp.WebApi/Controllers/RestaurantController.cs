using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Request.Restaurant;
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
        public IActionResult Create([FromBody] CreateRestaurantRequest createrestaurantrequest)
        {
            var newRestaurant = new Restaurant(createrestaurantrequest.Name, createrestaurantrequest.Category, createrestaurantrequest.Latitude, createrestaurantrequest.Longitude, createrestaurantrequest.AveragePrice);
            
            var result = _restaurantService.Add(newRestaurant);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateRestaurantRequest updaterestaurantrequest)
        {
            var updateRestaurant = new Restaurant(updaterestaurantrequest.Name, updaterestaurantrequest.Category, updaterestaurantrequest.Latitude, updaterestaurantrequest.Longitude, updaterestaurantrequest.AveragePrice);
            

            var result = _restaurantService.Update(id, updateRestaurant);
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

        [HttpGet("nearby")]
        public IActionResult GetNearby([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] double radiusInKm = 5)
        {
            var result = _restaurantService.GetNearby(latitude, longitude, radiusInKm);
            return Ok(result);
        }

        
    }
}
