namespace RestaurantSuggestionApp.Core.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        
        public decimal AveragePrice { get; set; }

        
        public ICollection<UserExpenditure>? Expenditures { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
