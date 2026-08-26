using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantSuggestionApp.Business.ExternalServices;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.Core.Entities;
using RestaurantSuggestionApp.DataAccess;
using RestaurantSuggestionApp.WebApi.Localizer;
using RestaurantSuggestionApp.WebApi.Middleware;

// =========================================================================================
// PROGRAM.CS NEDİR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
// Bu dosya, tüm backend uygulamasının "Ana Çalıştırma Motoru ve Giriş Kapısıdır".
// Bir bina inşa edilirken elektrik, su ve güvenlik tesisatının döşenmesi gibi,
// uygulamanın veritabanı bağlantısı, şifreleme güvenliği, dil seçenekleri ve API ayarları burada yapılır.
// =========================================================================================

var builder = WebApplication.CreateBuilder(args);

// 1. ADIM: AYAR DOSYALARI (appsettings.json) YÜKLENİYOR
builder.Configuration.Sources.Clear();
var env = builder.Environment;
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                     .AddEnvironmentVariables();

// 2. ADIM: VERİTABANI VE BELLEK TABANLI ÇOKLU DİL (LOCALIZATION) SİSTEMİ
// Uygulamanın hem Türkçe hem İngilizce konuşabilmesi için yerelleştirme servisimizi bağlıyoruz.
builder.Services.AddSingleton<IStringLocalizerFactory, SqlStringLocalizerFactory>();
builder.Services.AddSingleton<SqlStringLocalizer>();
builder.Services.AddSingleton<IStringLocalizer>(sp => sp.GetRequiredService<SqlStringLocalizer>());
builder.Services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr"),
        new CultureInfo("tr-TR"),
        new CultureInfo("en"),
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider { QueryStringKey = "lang" },
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

// 3. ADIM: CONTROLLER'LAR, HTTP VE BELLEK ÖNBELLEĞİ (MEMORY CACHE) SERVİSLERİ
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); 

// 4. ADIM: GÜVENLİK VE SALDIRI KORUMASI (RATE LIMITING)
// Kötü niyetli kişilerin giriş ekranına saniyede yüzlerce şifre denemesi yapmasını engeller (Dakikada en fazla 5 deneme).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,                 
                Window = TimeSpan.FromMinutes(1) 
            }));
});

// 5. ADIM: KİMLİK DOĞRULAMA (JWT BEARER AUTHENTICATION)
// Kullanıcının sisteme giriş yaptıktan sonra aldığı dijital imzanın (Token) doğruluğunu kontrol eder.
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
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true, 
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// 6. ADIM: İŞ SERVİSLERİNİN BAĞLANMASI (DEPENDENCY INJECTION)
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHttpClient<IOsrmService, OsrmService>();

// 7. ADIM: CORS AYARLARI (Ön yüzün Backend ile rahat iletişim kurabilmesi)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 8. ADIM: VERİTABANI BAĞLANTISI (SQL SERVER)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));

// 9. ADIM: SWAGGER API DOKÜMANTASYON EKRANI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantSuggestionApp API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Lütfen token'ınızı 'Bearer {token}' formatında girin. Örneğin: Bearer eyJhbGci..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =========================================================================================
// UYGULAMA İNŞA EDİLİYOR VE ÇALIŞTIRILIYOR (APPLICATION PIPELINE)
// =========================================================================================
var app = builder.Build();

// Otomatik Veritabanı Migrasyonu ve Tablo Kontrolü
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        dbContext.Database.ExecuteSqlRaw(@"
            IF OBJECT_ID(N'[UserFavorites]', N'U') IS NULL
            BEGIN
                CREATE TABLE [UserFavorites] (
                    [Id] int NOT NULL IDENTITY,
                    [UserId] int NOT NULL,
                    [RestaurantId] int NOT NULL,
                    [CreatedDate] datetime2 NOT NULL,
                    [CustomNote] nvarchar(max) NULL,
                    CONSTRAINT [PK_UserFavorites] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_UserFavorites_Restaurants_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurants] ([Id]) ON DELETE CASCADE,
                    CONSTRAINT [FK_UserFavorites_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
                );
            END;
        ");

        // Otomatik Lokalizasyon Kaynaklarını Eşitleme (Eksik Çeviri Kayıtlarını Veritabanına Yükle)
        var existingKeys = dbContext.LocalizationResources
            .Select(r => r.Culture + "_" + r.ResourceKey)
            .ToHashSet();

        var missingResources = DbContextSeed.GetDefaultLocalizationResources()
            .Where(r => !existingKeys.Contains(r.Culture + "_" + r.ResourceKey))
            .Select(r => new LocalizationResource(0, r.Culture, r.ResourceKey, r.Value))
            .ToList();

        if (missingResources.Count > 0)
        {
            dbContext.LocalizationResources.AddRange(missingResources);
            dbContext.SaveChanges();
        }
    }
    catch
    {
        // Migrasyon ve eşitleme hatası durumunda uygulama çökmeden güvenle devam eder
    }
}

// Swagger Güvenlik ve Arayüzü
app.UseMiddleware<SwaggerBasicAuthMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantSuggestionApp API v1");
});

// Dil Seçeneğini İsteklere Uygula
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.UseRateLimiter();
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();