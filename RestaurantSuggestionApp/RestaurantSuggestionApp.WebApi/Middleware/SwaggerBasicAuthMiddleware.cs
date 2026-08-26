using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantSuggestionApp.WebApi.Middleware;

// =========================================================================================
// BU ARA KATMAN (MIDDLEWARE) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Swagger ekranı, sistemin tüm API kapılarını gösteren teknik bir paneldir.
// Dışarıdan yetkisiz kişilerin bu teknik panele erişip kurcalamasını engellemek için
// kapıya bir güvenlik görevlisi gibi "Kullanıcı Adı ve Şifre" soran Basic Authentication koruması koyar.
// =========================================================================================
public class SwaggerBasicAuthMiddleware
{
    // Sonraki işlem adımına geçmeyi sağlayan temsilci (delegate)
    private readonly RequestDelegate _next;

    // -------------------------------------------------------------------------------------
    // Kurucu Metot (Constructor):
    // Middleware zincirindeki bir sonraki adımı teslim alır.
    // -------------------------------------------------------------------------------------
    public SwaggerBasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // -------------------------------------------------------------------------------------
    // İstekleri Denetleyen Ana Metot:
    // Eğer istek "/swagger" adresine gidiyorsa kullanıcı adı ve şifre kontrolü yapar.
    // -------------------------------------------------------------------------------------
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            string? authHeader = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                    var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(':', 2);

                    var username = credentials.Length > 0 ? credentials[0] : "";
                    var password = credentials.Length > 1 ? credentials[1] : "";

                    // Doğru kullanıcı adı ve şifre girildiyse kapıyı aç ve içeri al
                    if (username == "ozgun" && password == "inventivstaj")
                    {
                        await _next(context);
                        return;
                    }
                }
                catch
                {
                    // Şifre çözme hatası olursa geçersiz sayılır
                }
            }

            // Yetkisiz girişte tarayıcıya "Kullanıcı Adı / Şifre" kutucuğu çıkartır (HTTP 401)
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger UI Restricted Area\"";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Yetkisiz Erisim. Lutfen gecerli kullanici adi ve sifre girin.");
            return;
        }

        // Swagger harici normal isteklerde hiçbir engelleme yapmadan devam eder
        await _next(context);
    }
}
