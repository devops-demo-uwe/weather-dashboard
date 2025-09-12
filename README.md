# 🌤️ Weather Dashboard

A modern ASP.NET Core Razor Pages weather application that provides real-time weather information with a beautiful, responsive interface.

## ✨ Current Status

**FULLY FUNCTIONAL** - The weather dashboard is complete with current weather display, responsive UI, and comprehensive error handling. Ready for production deployment!

## 🚀 Features (Implemented)

- ✅ **Current Weather Display** - Real-time weather data with temperature, humidity, wind speed, pressure
- ✅ **Temperature Conversions** - Displays both Celsius and Fahrenheit with accurate conversions  
- ✅ **Weather Icons** - Dynamic Bootstrap icons that change based on weather conditions
- ✅ **City Search** - Search for weather in any city worldwide
- ✅ **Responsive Design** - Mobile-friendly interface with Bootstrap 5 and custom styling
- ✅ **Error Handling** - Comprehensive user feedback for API failures and invalid cities
- ✅ **Health Monitoring** - Built-in health checks for API connectivity
- ✅ **Caching** - Memory caching for improved performance
- ✅ **Security** - Secure API key management with .NET Secret Manager

## 🏗️ Features (Planned)

- 📅 5-day weather forecast
- ⭐ Favorite cities management with Azure Table Storage
- � Enhanced city search with autocomplete
- 📱 Progressive Web App (PWA) capabilities

## 📁 Project Structure

```
├── Configuration/          # Configuration classes
│   ├── WeatherApiOptions.cs      # Strongly-typed weather API configuration
│   └── AzureStorageOptions.cs    # Strongly-typed Azure Storage configuration
├── Extensions/             # Extension methods
│   ├── WeatherServiceExtensions.cs   # Weather service dependency injection
│   └── AzureStorageServiceExtensions.cs   # Azure Storage dependency injection
├── Models/                 # Data models
│   ├── CurrentWeather.cs          # Current weather data model
│   ├── WeatherCondition.cs        # Weather condition with icons
│   ├── WeatherApiResponse.cs      # OpenWeatherMap API response models
│   ├── WeatherModelExtensions.cs  # Model conversion utilities
│   ├── CitySearchResult.cs        # City search result model
│   ├── FavoriteCity.cs           # Favorite city model
│   └── FavoriteCityEntity.cs     # Azure Table Storage entity for favorites
├── Services/               # Business logic services
│   ├── IWeatherService.cs         # Weather service interface
│   ├── WeatherService.cs          # OpenWeatherMap API integration
│   ├── WeatherServiceException.cs # Custom exception handling
│   ├── WeatherServiceHealthCheck.cs   # Health monitoring
│   ├── IFavoriteCityService.cs    # Favorite cities service interface
│   ├── FavoriteCityService.cs     # Azure Table Storage favorites implementation
│   └── FavoriteCityServiceException.cs # Favorites service exception handling
├── Pages/                  # Razor Pages
│   ├── Index.cshtml/.cs          # Main weather dashboard page
│   ├── Health.cshtml/.cs         # Health monitoring page
│   ├── Error.cshtml/.cs          # Error handling page
│   ├── Privacy.cshtml/.cs        # Privacy policy page
│   └── Shared/                   # Shared layouts and partials
├── wwwroot/                # Static files
│   ├── css/site.css             # Custom styling with gradients
│   ├── js/site.js               # JavaScript utilities
│   └── lib/                     # Third-party libraries (Bootstrap, jQuery)
├── Properties/             # Launch settings
├── Program.cs              # Application startup and configuration
├── appsettings.json        # Application configuration
└── WeatherDashboard.csproj # Project file with dependencies
```

## 🛠️ Technology Stack

- **.NET 9** - Latest .NET framework
- **ASP.NET Core Razor Pages** - Server-side web framework
- **OpenWeatherMap API** - Weather data provider
- **Azure Table Storage** - NoSQL cloud storage for favorites
- **Bootstrap 5** - Responsive CSS framework
- **Bootstrap Icons** - Modern icon library
- **System.Text.Json** - High-performance JSON serialization
- **Microsoft.Extensions.Http** - HTTP client factory
- **Microsoft.Extensions.Caching.Memory** - In-memory caching
- **Microsoft.Extensions.Diagnostics.HealthChecks** - Health monitoring
- **Azure.Data.Tables** - Azure Table Storage client library

## ⚡ Getting Started

### Prerequisites
- .NET 9 SDK
- OpenWeatherMap API key (free at [openweathermap.org](https://openweathermap.org/api))
- Azure Storage Emulator (for favorites functionality)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/devops-demo-uwe/weather-dashboard.git
   cd weather-dashboard
   ```

2. **Set up your OpenWeatherMap API key**
   ```bash
   dotnet user-secrets set "WeatherApi:ApiKey" "your-api-key-here"
   ```

3. **Set up Azure Storage Emulator (for favorites)**
   
   **Windows (Azurite - recommended):**
   ```bash
   # Install Azurite globally
   npm install -g azurite
   
   # Start the storage emulator
   azurite --silent --location c:\azurite --debug c:\azurite\debug.log
   ```
   
   **Alternative - Azure Storage Emulator (legacy):**
   - Download and install the [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
   - Start the emulator: `AzureStorageEmulator.exe start`
   
   **Verification:**
   The application will automatically create the `Favorites` table on first run.

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open your browser**
   Navigate to `http://localhost:5224`

### Configuration

The application uses strongly-typed configuration in `appsettings.json`:

```json
{
  "WeatherApi": {
    "BaseUrl": "https://api.openweathermap.org/data/2.5",
    "Units": "metric",
    "Language": "en",
    "TimeoutSeconds": 30,
    "EnableCaching": true,
    "CacheDurationMinutes": 10
  },
  "AzureStorage": {
    "ConnectionString": "UseDevelopmentStorage=true",
    "FavoritesTableName": "Favorites"
  }
}
```

For production, replace the `ConnectionString` with your Azure Storage account connection string.

## 🧪 Testing

### Manual Testing
1. Search for different cities: "London", "New York", "Tokyo"
2. Verify temperature accuracy and realistic ranges
3. Test responsive design on different screen sizes
4. Check error handling with invalid city names

### Health Monitoring
Visit `/Health` to check API connectivity and system status.

## 🔒 Security Features

- **API Key Protection** - Uses .NET Secret Manager for development
- **Input Validation** - Server-side validation for all user inputs  
- **Error Handling** - Secure error messages without exposing internal details
- **HTTPS Ready** - Configured for secure connections

## 🎨 UI Features

- **Modern Design** - Custom CSS with gradients and smooth animations
- **Weather Icons** - Dynamic icons that match weather conditions
- **Responsive Layout** - Works perfectly on desktop and mobile
- **Loading States** - Professional spinners during API calls
- **User Feedback** - Clear success and error messages

## 📈 Performance

- **HTTP Client Factory** - Efficient connection pooling
- **Memory Caching** - Reduces API calls and improves response times
- **Async/Await** - Non-blocking operations throughout
- **Lightweight** - Minimal dependencies and optimized bundle size

## 🚀 Deployment Ready

The application is production-ready with:
- Health checks for monitoring
- Comprehensive logging
- Error handling and recovery
- Secure configuration management
- Docker support (Dockerfile included)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- [OpenWeatherMap](https://openweathermap.org/) for providing the weather API
- [Bootstrap](https://getbootstrap.com/) for the responsive framework
- [Bootstrap Icons](https://icons.getbootstrap.com/) for the beautiful icons