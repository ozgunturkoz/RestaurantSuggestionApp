namespace RestaurantSuggestionApp.Core.DTOs.Requests
{
    public class RestaurantDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
