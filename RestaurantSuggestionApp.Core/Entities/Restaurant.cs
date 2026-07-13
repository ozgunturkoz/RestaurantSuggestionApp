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

       // public List<UserExpenditure> Expenditures { get; set; } = new List<UserExpenditure>();
       // public List<Comment> Comments { get; set; } = new List<Comment>();

        public Restaurant(string name, string category, decimal latitude, decimal longitude, decimal averagePrice) //constructor
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Restaurant name cannot be empty or whitespace.", nameof(name));
            }
            Name = name;
            Category = category;
            Latitude = latitude;
            Longitude = longitude;
            AveragePrice = averagePrice;
        }
    }
}
