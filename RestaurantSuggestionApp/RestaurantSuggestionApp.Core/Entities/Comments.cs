namespace RestaurantSuggestionApp.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int Rating { get; private set; }

        public string Text { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Comment() { }

        public Comment(int id, int userId, int restaurantId, int rating, string text)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            EditComment(text, rating);
        }

        public void EditComment(string text, int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Değerlendirme puanı 1 ile 5 arasında olmalıdır.");

            if (string.IsNullOrWhiteSpace(text) || text.Length < 3)
                throw new ArgumentException("Yorum metni en az 3 karakter olmalıdır.");

            Text = text.Trim();
            Rating = rating;
        }
    }
}

