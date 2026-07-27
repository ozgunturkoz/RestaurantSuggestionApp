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
    }
}
