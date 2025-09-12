# Weather Dashboard

A minimal ASP.NET Core Razor Pages application for weather information.

## Current Status

This is a basic project structure with placeholder homepage. The application is ready for development but weather features are not yet implemented.

## Features (Planned)

- 🌤️ Current weather display
- 📅 5-day weather forecast
- 🔍 City search functionality
- ⭐ Favorite cities management
- 📱 Responsive design with Bootstrap 5

## Project Structure

```
├── Configuration/     # Configuration classes (empty, ready for weather settings)
├── Models/           # Data models (empty, ready for weather models)
├── Services/         # Business logic services (empty, ready for weather API services)
├── Pages/            # Razor Pages
├── wwwroot/          # Static files
└── appsettings.json  # Configuration with placeholder weather API settings
```

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```bash
   dotnet run
   ```
4. Open your browser to `https://localhost:5001`

## Next Steps

- Add OpenWeatherMap API integration
- Implement Azure Table Storage for favorites
- Add weather data models and services
- Implement search and forecast functionality

## Technology Stack

- .NET 9
- ASP.NET Core Razor Pages
- Bootstrap 5 (included)
- Future: OpenWeatherMap API
- Future: Azure Table Storage