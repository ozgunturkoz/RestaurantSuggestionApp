using Microsoft.EntityFrameworkCore;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Business.ExternalServices;
using RestaurantSuggestionApp.DataAccess;

// EKLENEN KÜTÜPHANELER
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Kestrel HTTPS IP Bağlantı Ayarı (Telefon IP'lerinin 7085 portuna HTTPS ile erişmesini sağlar)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 7085, listenOptions =>
    {
        listenOptions.UseHttps();
    });
    options.Listen(System.Net.IPAddress.Any, 5062);
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); // Hafızada geçici veri tutmak için eklendi

// IP Bazlı Rate Limiter Tanımlaması
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,                 // 1 dakikada maks 5 istek
                Window = TimeSpan.FromMinutes(1) // 1 dakikalık zaman penceresi
            }));
});

// JWT AUTHENTICATION SERVISLERININ EKLENMESI
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, // Token süresi dolmuşsa isteği reddet
        ValidateIssuerSigningKey = true, // İmza doğrulaması yap
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IOsrmService, OsrmService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantSuggestionApp API", Version = "v1" });
    
    // Swagger UI üzerinde "Bearer Token" butonu (Authorize) ekliyoruz
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Lütfen token'ınızı 'Bearer {token}' formatında girin. Örneğin: Bearer eyJhbGci..."
    });

    c.AddSecurityRequirement((doc) => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (
                SELECT 1 FROM sys.tables 
                WHERE name = N'UserFavorites'
            )
            BEGIN
                CREATE TABLE UserFavorites (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    UserId INT NOT NULL,
                    RestaurantId INT NOT NULL,
                    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    CustomNote NVARCHAR(MAX) NULL,
                    CONSTRAINT FK_UserFavorites_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                    CONSTRAINT FK_UserFavorites_Restaurants FOREIGN KEY (RestaurantId) REFERENCES Restaurants(Id) ON DELETE CASCADE
                );
            END;
        ");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veritabanı sütun/tablo güncelleme uyarısı: {ex.Message}");
    }
}

// Swagger her ortamda açık tutuluyor
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantSuggestionApp API v1");
});

app.UseHttpsRedirection();
app.UseCors();

// Rate Limiter Middleware
app.UseRateLimiter();

// HTTP İstek Hattında Auth Sıralaması Çok Önemlidir:
app.UseAuthentication(); // 1. Önce kimlik doğrula (Kullanıcı kim?)
app.UseAuthorization();  // 2. Sonra yetki kontrolü yap (Bu kaynağa erişebilir mi?)

app.MapControllers();

app.Run();