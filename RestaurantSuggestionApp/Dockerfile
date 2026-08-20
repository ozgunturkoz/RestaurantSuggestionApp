# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["RestaurantSuggestionApp.WebApi/RestaurantSuggestionApp.WebApi.csproj", "RestaurantSuggestionApp.WebApi/"]
COPY ["RestaurantSuggestionApp.Business/RestaurantSuggestionApp.Business.csproj", "RestaurantSuggestionApp.Business/"]
COPY ["RestaurantSuggestionApp.Core/RestaurantSuggestionApp.Core.csproj", "RestaurantSuggestionApp.Core/"]
COPY ["RestaurantSuggestionApp.DataAccess/RestaurantSuggestionApp.DataAccess.csproj", "RestaurantSuggestionApp.DataAccess/"]

RUN dotnet restore "RestaurantSuggestionApp.WebApi/RestaurantSuggestionApp.WebApi.csproj"

# Copy all source files and publish
COPY . .
WORKDIR "/src/RestaurantSuggestionApp.WebApi"
RUN dotnet publish "RestaurantSuggestionApp.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Environment variables for Linux Docker / Render.com
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 8080

ENTRYPOINT ["dotnet", "RestaurantSuggestionApp.WebApi.dll"]
