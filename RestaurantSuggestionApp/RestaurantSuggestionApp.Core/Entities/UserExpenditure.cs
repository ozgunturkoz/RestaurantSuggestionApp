namespace RestaurantSuggestionApp.Core.Entities
{
    public class UserExpenditure
    {
        public int Id { get; set; }

        
        public int UserId { get; set; }
        public User? User { get; set; } 

        
        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; } 

        
        public decimal Amount { get; set; }

        public DateTime SpentAt { get; set; }
    }
}
