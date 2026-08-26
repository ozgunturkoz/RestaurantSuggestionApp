using System;

namespace RestaurantSuggestionApp.Core.Entities
{
    // =========================================================================================
    // BU VARLIK (ENTITY) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Veritabanındaki `UserExpenditures` (Kullanıcı Harcamaları) tablosunu temsil eder.
    // Kullanıcının hangi restoranda, hangi tarihte ve ne kadar tutarda (TL) yemek yediğini saklar.
    // =========================================================================================
    public class UserExpenditure
    {
        // Harcama kaydının benzersiz numarası
        public int Id { get; set; }

        // Harcamayı yapan kullanıcının ID numarası
        public int UserId { get; set; }
        public User? User { get; set; } 

        // Harcama yapılan restoranın ID numarası (İsteğe bağlı)
        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; } 

        // Harcanan tutar (TL)
        public decimal Amount { get; private set; }

        // Harcamanın yapıldığı tarih ve saat
        public DateTime SpentAt { get; set; } = DateTime.UtcNow;

        // Boş Kurucu Metot (Entity Framework için gereklidir)
        public UserExpenditure() { }

        // ID içeren Kurucu Metot (Seed verileri için)
        public UserExpenditure(int id, int userId, int? restaurantId, decimal amount, DateTime spentAt)
        {
            Id = id;
            UserId = userId;
            RestaurantId = restaurantId;
            SetAmount(amount);
            SpentAt = spentAt;
        }

        // Yeni harcama oluştururken kullanılan Kurucu Metot (Otomatik ID için)
        public UserExpenditure(int userId, int? restaurantId, decimal amount, DateTime spentAt)
        {
            UserId = userId;
            RestaurantId = restaurantId;
            SetAmount(amount);
            SpentAt = spentAt;
        }

        // Harcama tutarını güncelleyen ve 0'dan büyük olmasını zorunlu kılan güvenlik metodu
        public void SetAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Harcama tutarı 0'dan büyük olmalıdır.");

            Amount = amount;
        }
    }
}
