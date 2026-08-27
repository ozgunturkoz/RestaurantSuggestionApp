using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
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

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
var env = builder.Environment;
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                     .AddEnvironmentVariables();

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

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("AuthRateLimit", httpContext =>
    {
        var clientIp = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim()
                       ?? httpContext.Connection.RemoteIpAddress?.ToString()
                       ?? "unknown_ip";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 15,
                Window = TimeSpan.FromMinutes(1)
            });
    });
});

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

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
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

var app = builder.Build();

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

        var existingRestaurantNames = dbContext.Restaurants
            .Select(r => r.Name.ToLower())
            .ToHashSet();

        var missingRestaurants = DbContextSeed.GetDefaultRestaurants()
            .Where(r => !existingRestaurantNames.Contains(r.Name.ToLower()))
            .Select(r => new Restaurant(0, r.Name, r.Category, r.Latitude, r.Longitude, r.AveragePrice))
            .ToList();

        if (missingRestaurants.Count > 0)
        {
            dbContext.Restaurants.AddRange(missingRestaurants);
            dbContext.SaveChanges();
        }
    }
    catch
    {
    }
}

app.UseForwardedHeaders();

app.UseMiddleware<SwaggerBasicAuthMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantSuggestionApp API v1");
});

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

app.MapMethods("/health", new[] { "GET", "HEAD" }, () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow })).DisableRateLimiting();
app.MapMethods("/healthz", new[] { "GET", "HEAD" }, () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow })).DisableRateLimiting();
app.MapMethods("/", new[] { "GET", "HEAD" }, () => Results.Ok(new { message = "RestaurantSuggestionApp API is running", status = "Healthy" })).DisableRateLimiting();

app.Run();
