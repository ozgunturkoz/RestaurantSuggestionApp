using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantSuggestionApp.Core.Request.Restaurant
{
    public class UpdateRestaurantRequest
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
