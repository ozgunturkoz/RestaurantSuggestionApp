namespace RestaurantSuggestionApp.Business.Helpers
{
    public static class BudgetHelper
    {
        public static int GetBudgetIndex(decimal amount)
        {
            if (amount < 150) return 1;
            if (amount <= 250) return 2;
            if (amount <= 450) return 3;
            if (amount <= 700) return 4;
            return 5;
        }

        /// <summary>
        /// Kullanıcının ortalama öğün harcaması ile restoranın ortalama fiyatı arasındaki 
        /// yakınlaşma yüzdesini (0-100) hesaplar.
        /// </summary>
        public static double CalculateProximityScore(decimal userAverage, decimal restaurantPrice)
        {
            if (userAverage <= 0 || restaurantPrice <= 0) return 50.0;

            // Sapma Oranı = |Fiyat - Ortalama| / Ortalama
            double percentageDifference = (double)(Math.Abs(restaurantPrice - userAverage) / userAverage);

            // Skoru %100 üzerinden hesaplayıp 0-100 aralığına sıkıştırıyoruz
            double score = 100.0 * (1.0 - percentageDifference);

            if (score < 0) score = 0;
            if (score > 100) score = 100;

            return Math.Round(score, 1);
        }
    }
}

