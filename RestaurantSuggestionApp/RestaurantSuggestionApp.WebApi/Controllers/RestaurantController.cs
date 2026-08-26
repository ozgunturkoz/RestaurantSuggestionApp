using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.DTOs.Requests;
using RestaurantSuggestionApp.Core.Entities;

namespace RestaurantSuggestionApp.WebApi.Controllers
{
    // =========================================================================================
    // BU CONTROLLER (KONTROLCÜ) NE İŞE YARAR?
    // Haritada ve Keşfet sayfasındaki restoranları yönetir.
    // Kullanıcının harita üzerindeki konumunu (Enlem/Boylam) ve istediği arama yarıçapını (1 km, 2 km, 5 km)
    // alıp, arkadaki yapay zeka ve konum servislerini çalıştırarak en uygun restoran listesini ekrana sunar.
    // =========================================================================================
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        // Restoran iş mantıklarını yürüten servisimiz
        private readonly IRestaurantService _restaurantService;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // Controller üretildiğinde RestaurantService nesnesini içine alır.
        // -------------------------------------------------------------------------------------
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        // -------------------------------------------------------------------------------------
        // 1. TÜM RESTORANLARI GETİR: GET /api/Restaurant
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // 2. ID İLE RESTORAN DETAYI GETİR: GET /api/Restaurant/{id}
        // -------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _restaurantService.GetById(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 3. YENİ RESTORAN EKLE: POST /api/Restaurant
        // -------------------------------------------------------------------------------------
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

        // -------------------------------------------------------------------------------------
        // 4. RESTORAN GÜNCELLE: PUT /api/Restaurant/{id}
        // -------------------------------------------------------------------------------------
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] RestaurantDto restaurantDto)
        {
            var newRestaurant = MapToRestaurant(restaurantDto);

            var result = _restaurantService.Update(id, newRestaurant);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 5. RESTORAN SİL: DELETE /api/Restaurant/{id}
        // -------------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _restaurantService.Delete(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // 6. YAKINDAKİ VE EN UYGUN RESTORANLARI LİSTELE: GET /api/Restaurant/nearby
        // Harita sayfasının kalbidir. Kullanıcının enlemini, boylamını ve yarıçapını alır,
        // yürüyüş mesafesi ve bütçeye göre en yüksek puanlı mekanları sıralı olarak döner.
        // -------------------------------------------------------------------------------------
        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] double radiusInKm = 5)
        {
            int? userId = GetCurrentUserId();

            var result = await _restaurantService.GetNearbyAsync(latitude, longitude, radiusInKm, userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: Token'dan giriş yapmış kullanıcının ID numarasını alır
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

        // -------------------------------------------------------------------------------------
        // Yardımcı Metot: DTO nesnesini Restaurant varlığına çevirir
        // -------------------------------------------------------------------------------------
        private Restaurant MapToRestaurant(RestaurantDto dto)
        {
            return new Restaurant(dto.Name, dto.Category, dto.Latitude, dto.Longitude, dto.AveragePrice);
        }
    }
}
