- Follow secure coding practices throughout

## Coding Standards
- Use async/await patterns
- Enable nullable reference types
- Use proper error handling with try-catch blocks
- Implement logging
- Follow C# naming conventions
- Add XML documentation for public APIs
- Use dependency injection throughout
# Copilot Instructions: ASP.NET Core Weather Dashboard
 
## Project Overview
This project is an ASP.NET Core web application that serves as a weather dashboard. It provides users with real-time weather information and forecasts for cities around the world. The dashboard is designed to be responsive and user-friendly, supporting both desktop and mobile devices.
 
## Core Features
 - **Current Weather Display:** Show up-to-date weather conditions for the selected city, including temperature, humidity, wind speed, and weather icons.
 - **5-Day Forecast:** Present a five-day weather forecast with daily summaries and visual indicators.
 - **City Search:** Allow users to search for cities to view their weather data.
 - **Favorites Management:** Enable users to add cities to a favorites list for quick access.
 - **Responsive Design:** Ensure the dashboard layout adapts seamlessly to different screen sizes and devices.

## Technical Requirements
- **.NET Core 9** with Razor Pages (no MVC)
- **Azure Table Storage** with local emulator for development
- **OpenWeatherMap API** integration for weather data
- **Bootstrap 5** for responsive UI


## Security Guidelines
- Use .NET Secret Manager for development
- Prepare for Azure Key Vault in production
- Implement input validation
- Secure API key handling
- Follow secure coding practices throughout

