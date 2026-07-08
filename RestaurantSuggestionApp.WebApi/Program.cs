using Microsoft.EntityFrameworkCore;
using RestaurantSuggestionApp.Business.Services;
using RestaurantSuggestionApp.DataAccess;
using static RestaurantSuggestionApp.Business.Services.RestaurantServices;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddScoped<IRestaurantService, RestaurantService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            
            sqlOptions.EnableRetryOnFailure();
        }));





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 

app.Run();