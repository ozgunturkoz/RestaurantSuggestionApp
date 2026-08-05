using System;
using System.Text.Json.Serialization;

namespace RestaurantSuggestionApp.Core.Entities
{
    public class UserFavorite
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant? Restaurant { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? CustomNote { get; private set; }

        public UserFavorite() { }

        public UserFavorite(int id, int userId, int restaurantId, string? customNote = null)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            UpdateNote(customNote);
        }

        public void UpdateNote(string? customNote)
        {
            CustomNote = customNote?.Trim();
        }
    }
}

