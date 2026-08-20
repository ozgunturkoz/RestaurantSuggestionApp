using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace RestaurantSuggestionApp.WebApi.Localizer;

public class SqlStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMemoryCache _cache;

    public SqlStringLocalizerFactory(IServiceScopeFactory scopeFactory, IMemoryCache cache)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new SqlStringLocalizer(_scopeFactory, _cache);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new SqlStringLocalizer(_scopeFactory, _cache);
    }
}
