using System;
using System.Text.Json.Serialization;

namespace RestaurantSuggestionApp.Core.Entities
{
    // =========================================================================================
    // BU VARLIK (ENTITY) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Veritabanındaki `UserFavorites` (Favori Restoranlar) tablosunu temsil eder.
    // Bir kullanıcının beğendiği restoranları ve o restoran hakkında aldığı özel notları (örneğin: "Lahmacunu çok lezzetli") saklar.
    // =========================================================================================
    public class UserFavorite
    {
        // Favori kaydının benzersiz numarası
        public int Id { get; set; }

        // Favoriye ekleyen kullanıcının ID'si
        public int UserId { get; set; }

        // Döngüsel JSON hatasını önlemek için User nesnesini JSON çıktısında gizler
        [JsonIgnore]
        public User? User { get; set; }

        // Favoriye eklenen restoranın ID'si
        public int RestaurantId { get; set; }

        // Restoran bilgileri
        public Restaurant? Restaurant { get; set; }

        // Favoriye eklenme tarihi
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Kullanıcının mekan hakkında yazdığı özel hatırlatıcı not
        public string? CustomNote { get; private set; }

        // Boş Kurucu Metot (Entity Framework için gereklidir)
        public UserFavorite() { }

        // ID içeren Kurucu Metot (Seed verileri için)
        public UserFavorite(int id, int userId, int restaurantId, string? customNote = null)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            UpdateNote(customNote);
        }

        // Yeni favori kaydı açarken kullanılan Kurucu Metot
        public UserFavorite(int userId, int restaurantId, string? customNote = null)
        {
            UserId = userId;
            RestaurantId = restaurantId;
            UpdateNote(customNote);
        }

        // Favori notunu güncelleyen yardımcı metot
        public void UpdateNote(string? customNote)
        {
            CustomNote = customNote?.Trim();
        }
    }
}
