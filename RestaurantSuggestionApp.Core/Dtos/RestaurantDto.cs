using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantSuggestionApp.Core.Dtos
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
