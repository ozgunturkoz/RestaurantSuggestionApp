namespace RestaurantSuggestionApp.Core.Entities
{
    public class User
    {
        public int Id { get; }

        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        
        public ICollection<UserExpenditure>? Expenditures { get; set; }
        public ICollection<Comment>? Comments { get; set; }

        public void Update(User user)
        {
            if (user.Name.Length<3)
            {
                throw new Exception("Kullanıcının ismi 3 harften kısa olamaz");
            }
            this.Name = user.Name;
            this.Email = user.Email;


        }

    }
}
