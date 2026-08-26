using System;

namespace RestaurantSuggestionApp.Business.Helpers
{
    // =========================================================================================
    // BU SINIF NE İŞE YARAR? 
    // Dünya küre şeklinde olduğu için harita üzerindeki iki nokta (enlem ve boylam) arasındaki
    // kuş uçuşu gerçek mesafeyi düz cetvelle ölçemeyiz.
    // Bu sınıf, dünyanın eğriliğini hesaba katan meşhur "Haversine Trigonometri Formülü"nü kullanarak
    // kullanıcının restoranla arasındaki mesafeyi metre cinsinden kusursuz olarak hesaplar.
    // =========================================================================================
    public static class GeoHelper
    {
        // -------------------------------------------------------------------------------------
        // İki GPS koordinatı (Enlem1, Boylam1) ve (Enlem2, Boylam2) arasındaki mesafeyi metre olarak bulur.
        // -------------------------------------------------------------------------------------
        public static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double rEarth = 6371.0; // Dünyanın ortalama yarıçapı (Kilometre)
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            return rEarth * c * 1000.0; // Kilometreyi metreye çeviriyoruz (örn: 450 metre)
        }

        // -------------------------------------------------------------------------------------
        // Derece cinsinden gelen açıyı trigonometrik hesaplar için radyana çevirir.
        // -------------------------------------------------------------------------------------
        public static double ToRadians(double val)
        {
            return (Math.PI / 180.0) * val;
        }
    }
}
