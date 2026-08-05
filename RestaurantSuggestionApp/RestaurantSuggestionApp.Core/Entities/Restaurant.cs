namespace RestaurantSuggestionApp.Core.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; private set; } = string.Empty;

        public string Category { get; private set; } = string.Empty;

        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        public decimal AveragePrice { get; private set; }

        public ICollection<UserExpenditure>? Expenditures { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        public Restaurant() { }

        public Restaurant(int id, string name, string category, decimal lat, decimal lng, decimal averagePrice)
        {
            Id = id;
            UpdateDetails(name, category, averagePrice);
            UpdateLocation(lat, lng);
        }

        public void UpdateDetails(string name, string category, decimal averagePrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Restoran adı boş olamaz.");

            if (averagePrice <= 0)
                throw new ArgumentException("Ortalama fiyat 0'dan büyük olmalıdır.");

            Name = name.Trim();
            Category = category.Trim();
            AveragePrice = averagePrice;
        }

        public void UpdateLocation(decimal lat, decimal lng)
        {
            if (lat < -90 || lat > 90 || lng < -180 || lng > 180)
                throw new ArgumentException("Geçersiz enlem/boylam koordinatı.");

            Latitude = lat;
            Longitude = lng;
        }
    }
}

