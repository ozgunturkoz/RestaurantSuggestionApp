namespace RestaurantSuggestionApp.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        
        public ICollection<UserExpenditure>? Expenditures { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
