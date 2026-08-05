namespace RestaurantSuggestionApp.Core.Entities
{
    public class UserExpenditure
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; } 

        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; } 

        public decimal Amount { get; private set; }

        public DateTime SpentAt { get; set; } = DateTime.UtcNow;

        public UserExpenditure() { }

        public UserExpenditure(int id, int userId, int? restaurantId, decimal amount, DateTime spentAt)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            SetAmount(amount);
            SpentAt = spentAt;
        }

        public void SetAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Harcama tutarı 0'dan büyük olmalıdır.");

            Amount = amount;
        }
    }
}

