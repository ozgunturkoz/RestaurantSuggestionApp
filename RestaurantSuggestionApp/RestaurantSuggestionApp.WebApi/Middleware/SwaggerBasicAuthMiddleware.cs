using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantSuggestionApp.WebApi.Middleware;

public class SwaggerBasicAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerBasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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

                    if (username == "ozgun" && password == "inventivstaj")
                    {
                        await _next(context);
                        return;
                    }
                }
                catch
                {
                }
            }

            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger UI Restricted Area\"";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Yetkisiz Erisim. Lutfen gecerli kullanici adi ve sifre girin.");
            return;
        }

        await _next(context);
    }
}
