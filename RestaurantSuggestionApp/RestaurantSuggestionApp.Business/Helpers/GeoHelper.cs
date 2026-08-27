using System;

namespace RestaurantSuggestionApp.Business.Helpers
{
    public static class GeoHelper
    {
        public static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double rEarth = 6371.0;
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            return rEarth * c * 1000.0;
        }

        public static double ToRadians(double val)
        {
            return (Math.PI / 180.0) * val;
        }
    }
}
