using System;
using System.Collections.Generic;

namespace RestaurantSuggestionApp.Core.Entities
{
    // =========================================================================================
    // BU VARLIK (ENTITY) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Veritabanındaki `Restaurants` (Restoranlar) tablosunu temsil eder.
    // Her bir restoranın adını, kategorisini (Ev Yemekleri, Kebap vb.), haritadaki GPS koordinatlarını
    // (Enlem/Boylam) ve menüdeki ortalama yemek fiyatını (TL) saklar.
    // =========================================================================================
    public class Restaurant
    {
        // Restoranın benzersiz kimlik numarası
        public int Id { get; set; }

        // Restoran adı (örn: "Çiya Sofrası")
        public string Name { get; private set; } = string.Empty;

        // Restoran türü/kategorisi (örn: "Home Cooking")
        public string Category { get; private set; } = string.Empty;

        // Harita GPS Enlem koordinatı (Latitude)
        public decimal Latitude { get; private set; }

        // Harita GPS Boylam koordinatı (Longitude)
        public decimal Longitude { get; private set; }

        // Menünün ortalama porsiyon/öğün fiyatı (TL)
        public decimal AveragePrice { get; private set; }

        // Restoran sahibinin kullanıcı ID'si (varsa)
        public int? OwnerId { get; private set; }
        public User? Owner { get; set; }

        // Bu restoranda yapılan kullanıcı harcamalarının listesi
        public ICollection<UserExpenditure>? Expenditures { get; set; }

        // Boş Kurucu Metot (Entity Framework için gereklidir)
        public Restaurant() { }

        // ID içeren Kurucu Metot (Seed verileri için)
        public Restaurant(int id, string name, string category, decimal lat, decimal lng, decimal averagePrice)
        {
            Id = id;
            UpdateDetails(name, category, averagePrice);
            UpdateLocation(lat, lng);
        }

        // Yeni restoran oluştururken kullanılan Kurucu Metot (Otomatik ID için)
        public Restaurant(string name, string category, decimal lat, decimal lng, decimal averagePrice)
        {
            UpdateDetails(name, category, averagePrice);
            UpdateLocation(lat, lng);
        }

        // Restoran sahibini atayan metot
        public void SetOwner(int ownerId)
        {
            OwnerId = ownerId;
        }

        // Restoran adını, kategorisini ve fiyatını güncelleyen güvenlik kontrollü metot
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

        // Restoranın GPS koordinatlarını güncelleyen güvenlik kontrollü metot
        public void UpdateLocation(decimal lat, decimal lng)
        {
            if (lat < -90 || lat > 90 || lng < -180 || lng > 180)
                throw new ArgumentException("Geçersiz enlem/boylam koordinatı.");

            Latitude = lat;
            Longitude = lng;
        }
    }
}
