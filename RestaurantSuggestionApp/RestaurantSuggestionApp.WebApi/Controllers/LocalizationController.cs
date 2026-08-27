using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.WebApi.Localizer;

namespace RestaurantSuggestionApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    private readonly IStringLocalizer _localizer;

    public LocalizationController(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    [HttpGet]
    public IActionResult GetLocalization([FromQuery] string? lang)
    {
        if (!string.IsNullOrWhiteSpace(lang))
        {
            var culture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        var dictionary = _localizer.GetAllStrings(includeParentCultures: true)
                                   .ToDictionary(s => s.Name, s => s.Value);

        return Ok(dictionary);
    }

    [HttpGet("{lang}")]
    public IActionResult GetLocalizationByLang(string lang)
    {
        return GetLocalization(lang);
    }

    [HttpPost("refresh-cache")]
    public IActionResult RefreshCache([FromQuery] string? lang)
    {
        if (_localizer is SqlStringLocalizer sqlLocalizer)
        {
            sqlLocalizer.ClearCache(lang);
            return Ok(new { message = "Localization cache successfully refreshed from Database.", culture = lang ?? "all" });
        }

        return Ok(new { message = "Cache clear request processed." });
    }
}
