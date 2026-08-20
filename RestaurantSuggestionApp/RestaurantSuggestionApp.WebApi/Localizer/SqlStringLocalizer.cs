using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.DataAccess;

namespace RestaurantSuggestionApp.WebApi.Localizer;

public class SqlStringLocalizer : IStringLocalizer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMemoryCache _cache;
    private readonly string? _culture;

    public SqlStringLocalizer(IServiceScopeFactory scopeFactory, IMemoryCache cache, string? culture = null)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
        _culture = culture;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            if (format == null)
            {
                return new LocalizedString(name, name, resourceNotFound: true);
            }
            try
            {
                var value = string.Format(format, arguments);
                return new LocalizedString(name, value, resourceNotFound: false);
            }
            catch
            {
                return new LocalizedString(name, format, resourceNotFound: false);
            }
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var dict = GetDictionary();
        return dict.Select(kv => new LocalizedString(kv.Key, kv.Value, resourceNotFound: false));
    }

    public Dictionary<string, string> GetDictionary(string? cultureName = null)
    {
        var culture = cultureName ?? _culture ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
        culture = culture.ToLowerInvariant();

        var cacheKey = $"localization_dict_{culture}";

        // 1. Check IMemoryCache (Instant O(1) Memory Lookup)
        if (_cache.TryGetValue(cacheKey, out Dictionary<string, string>? cachedDict) && cachedDict != null)
        {
            return cachedDict;
        }

        // 2. Cache Miss: Query Database via AppDbContext Scope
        var dictionary = LoadFromDatabase(culture);

        // 3. Populate MemoryCache (Keep in RAM for 24 hours)
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(24));

        _cache.Set(cacheKey, dictionary, cacheOptions);

        return dictionary;
    }

    public void ClearCache(string? cultureName = null)
    {
        if (!string.IsNullOrWhiteSpace(cultureName))
        {
            _cache.Remove($"localization_dict_{cultureName.ToLowerInvariant()}");
        }
        else
        {
            _cache.Remove("localization_dict_tr");
            _cache.Remove("localization_dict_en");
        }
    }

    private string? GetString(string key)
    {
        var dict = GetDictionary();
        if (dict.TryGetValue(key, out var val))
        {
            return val;
        }

        // Fallback to "tr" culture if current culture doesn't have the key
        var trDict = GetDictionary("tr");
        if (trDict.TryGetValue(key, out var trVal))
        {
            return trVal;
        }

        return null;
    }

    private Dictionary<string, string> LoadFromDatabase(string culture)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var shortCulture = culture.Split('-')[0];

        var records = context.LocalizationResources
            .AsNoTracking()
            .Where(r => r.Culture == culture || r.Culture == shortCulture)
            .ToList();

        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in records)
        {
            if (!dict.ContainsKey(item.ResourceKey))
            {
                dict[item.ResourceKey] = item.Value;
            }
        }

        // Fallback to "tr" if empty
        if (dict.Count == 0 && shortCulture != "tr")
        {
            var fallbackRecords = context.LocalizationResources
                .AsNoTracking()
                .Where(r => r.Culture == "tr")
                .ToList();

            foreach (var item in fallbackRecords)
            {
                if (!dict.ContainsKey(item.ResourceKey))
                {
                    dict[item.ResourceKey] = item.Value;
                }
            }
        }

        return dict;
    }
}
