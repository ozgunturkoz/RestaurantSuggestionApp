using System;

namespace RestaurantSuggestionApp.Business.Helpers
{
    // =========================================================================================
    // BU SINIF NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Bu sınıf, kullanıcının cebindeki parayla restoranın menü fiyatları arasındaki bütçe uyumunu hesaplar.
    // Örneğin: Kullanıcı genelde öğle yemeğinde 300 TL harcıyorsa ve baktığı restoran da 300 TL ise
    // bütçe puanı %100 (tam uyum) olur. Restoran çok pahalıysa veya çok ucuzsa puan orantılı olarak düşer.
    // =========================================================================================
    public static class BudgetHelper
    {
        // -------------------------------------------------------------------------------------
        // Kullanıcının ortalama harcaması ile restoranın fiyatı arasındaki benzerlik puanını (0 - 100) hesaplar.
        // -------------------------------------------------------------------------------------
        public static double CalculateProximityScore(decimal userAverage, decimal restaurantPrice)
        {
            if (userAverage <= 0 || restaurantPrice <= 0) return 50.0;

            // Fiyatlar arasındaki yüzde farkını hesaplıyoruz
            double percentageDifference = (double)(Math.Abs(restaurantPrice - userAverage) / userAverage);

            // Fark ne kadar azsa puan o kadar 100'e yakın olur
            double score = 100.0 * (1.0 - percentageDifference);

            // Puanın 0'ın altına düşmesini veya 100'ü aşmasını engelliyoruz
            if (score < 0) score = 0;
            if (score > 100) score = 100;

            return Math.Round(score, 1);
        }
    }
}
