using System;

namespace RestaurantSuggestionApp.Core.Entities
{
    public class LocalizationResource
    {
        public int Id { get; set; }
        public string Culture { get; set; } = string.Empty;
        public string ResourceKey { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public LocalizationResource() { }

        public LocalizationResource(int id, string culture, string resourceKey, string value)
        {
            Id = id;
            Culture = culture;
            ResourceKey = resourceKey;
            Value = value;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
