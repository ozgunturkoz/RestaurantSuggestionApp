namespace RestaurantSuggestionApp.Core.DTOs.Responses
{
    public class RestaurantDistance
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AveragePrice { get; set; }
        public double? WalkingDistanceInMeters { get; set; }
        public double? WalkingDurationInSeconds { get; set; }

        public double? RecommendationScore { get; set; }
        public double? LocationScore { get; set; }
        public double? BudgetScore { get; set; }
    }
}
