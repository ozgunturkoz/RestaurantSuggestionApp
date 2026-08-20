namespace RestaurantSuggestionApp.Business.Helpers
{
    public static class BudgetHelper
    {
        public static double CalculateProximityScore(decimal userAverage, decimal restaurantPrice)
        {
            if (userAverage <= 0 || restaurantPrice <= 0) return 50.0;

            double percentageDifference = (double)(Math.Abs(restaurantPrice - userAverage) / userAverage);

            double score = 100.0 * (1.0 - percentageDifference);

            if (score < 0) score = 0;
            if (score > 100) score = 100;

            return Math.Round(score, 1);
        }
    }
}

