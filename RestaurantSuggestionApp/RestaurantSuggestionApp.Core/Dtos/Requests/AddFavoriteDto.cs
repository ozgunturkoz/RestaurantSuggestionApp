namespace RestaurantSuggestionApp.Core.DTOs.Requests
{
    public class AddFavoriteDto
    {
        public int RestaurantId { get; set; }
        public string? CustomNote { get; set; }
    }
}
