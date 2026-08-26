using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.WebApi.Localizer;

namespace RestaurantSuggestionApp.WebApi.Controllers;

// =========================================================================================
// BU CONTROLLER (KONTROLCÜ) NE İŞE YARAR?
// Frontend (Vue/Nuxt) uygulamasının tek bir istekte seçili dile (TR veya EN) ait tüm kelime
// ve cümle sözlüğünü veritabanından çekmesini sağlar.
// Böylece web sitesi ilk açıldığında veya dil değiştirildiğinde tüm butonlar ve başlıklar anında çevrilir.
// =========================================================================================
[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    // Veritabanı ve RAM önbelleğindeki çevirileri yöneten yerelleştirme servisi
    private readonly IStringLocalizer _localizer;

    // -------------------------------------------------------------------------------------
    // Kurucu Metot (Constructor):
    // Controller ayağa kalktığında IStringLocalizer nesnesini içeri alır.
    // -------------------------------------------------------------------------------------
    public LocalizationController(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    // -------------------------------------------------------------------------------------
    // 1. DİL SÖZLÜĞÜNÜ GETİR: GET /api/Localization?lang=tr veya GET /api/Localization?lang=en
    // İstenen dilin tüm kelime ve cümle karşılıklarını JSON sözlük formatında ön yüze teslim eder.
    // -------------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult GetLocalization([FromQuery] string? lang)
    {
        if (!string.IsNullOrWhiteSpace(lang))
        {
            var culture = new CultureInfo(lang);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        // İlgili dilin tüm kelime anahtarlarını ve çevirilerini sözlük haline getiriyoruz
        var dictionary = _localizer.GetAllStrings(includeParentCultures: true)
                                   .ToDictionary(s => s.Name, s => s.Value);

        return Ok(dictionary);
    }

    // -------------------------------------------------------------------------------------
    // 2. DİL SÖZLÜĞÜNÜ YOL PARAMETRESİYLE GETİR: GET /api/Localization/{lang}
    // -------------------------------------------------------------------------------------
    [HttpGet("{lang}")]
    public IActionResult GetLocalizationByLang(string lang)
    {
        return GetLocalization(lang);
    }

    // -------------------------------------------------------------------------------------
    // 3. ÖNBELLEĞİ YENİLE: POST /api/Localization/refresh-cache
    // Veritabanında yeni bir çeviri eklendiğinde RAM'deki eski önbelleği temizler.
    // -------------------------------------------------------------------------------------
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
