# Weather Dashboard GitHub Issues Creation Script
# Run this script after authenticating with GitHub CLI

Write-Host "Creating GitHub issues for Weather Dashboard project..." -ForegroundColor Green

# First, let's clean up the test issue
Write-Host "Cleaning up test issue..." -ForegroundColor Gray
gh issue close 1 2>$null

# High Priority Issues
Write-Host "Creating high priority issues..." -ForegroundColor Yellow

$body1 = @"
## Description
Set up integration with OpenWeatherMap API to fetch weather data.

## Acceptance Criteria
- [ ] Create WeatherService class with proper dependency injection
- [ ] Implement async methods for current weather and forecast data
- [ ] Add OpenWeatherMap API configuration to appsettings
- [ ] Set up secret management for API key using .NET Secret Manager
- [ ] Add proper error handling for API failures
- [ ] Include XML documentation for all public methods
- [ ] Add unit tests for WeatherService

## Technical Requirements
- Use HttpClient with dependency injection
- Implement IWeatherService interface
- Follow async/await patterns
- Add proper logging

## Priority
High - Required for all weather features
"@

gh issue create --title "🔧 Setup OpenWeatherMap API Integration" --body $body1 --label "enhancement,infrastructure"

gh issue create --title "🗄️ Setup Azure Table Storage for Favorites" --body "## Description
Configure Azure Table Storage to store user's favorite cities with local emulator support.

## Acceptance Criteria
- [ ] Create FavoriteCity entity model
- [ ] Implement FavoritesService with Azure Table Storage
- [ ] Set up local Azure Storage Emulator configuration
- [ ] Add connection string configuration in appsettings
- [ ] Implement CRUD operations for favorites
- [ ] Add proper error handling and logging
- [ ] Include XML documentation

## Technical Requirements
- Use Azure.Data.Tables NuGet package
- Implement IFavoritesService interface
- Enable nullable reference types
- Follow async/await patterns

## Priority
High - Required for favorites functionality" --label "enhancement,high-priority,infrastructure"

gh issue create --title "📊 Create Weather Data Models" --body "## Description
Create strongly-typed models for weather data with proper validation.

## Acceptance Criteria
- [ ] Create CurrentWeather model with all required properties
- [ ] Create WeatherForecast model for 5-day forecasts
- [ ] Create WeatherCondition enum or class
- [ ] Add data annotations for validation
- [ ] Include temperature, humidity, wind speed, weather icons
- [ ] Add XML documentation for all models
- [ ] Ensure nullable reference types compliance

## Models to Create
- CurrentWeather
- WeatherForecast
- WeatherCondition
- FavoriteCity
- WeatherApiResponse (for API mapping)

## Priority
High - Foundation for all weather features" --label "enhancement,high-priority,models"

gh issue create --title "🔍 Implement City Search Functionality" --body "## Description
Add real-time city search with weather data retrieval.

## Acceptance Criteria
- [ ] Update Index.cshtml with functional search form
- [ ] Implement search action in IndexModel
- [ ] Add client-side validation for city input
- [ ] Display current weather after successful search
- [ ] Show appropriate error messages for failed searches
- [ ] Add loading indicators during API calls
- [ ] Implement search history (optional)

## Technical Requirements
- Use Razor Pages pattern
- Implement proper input validation
- Add AJAX for better UX (optional)
- Follow Bootstrap 5 styling

## Priority
High - Core user functionality" --label "enhancement,high-priority,feature"

gh issue create --title "🌤️ Implement Current Weather Display" --body "## Description
Create a comprehensive current weather display component.

## Acceptance Criteria
- [ ] Display temperature (Celsius and Fahrenheit)
- [ ] Show weather condition with appropriate icons
- [ ] Display humidity, wind speed, and pressure
- [ ] Add weather condition descriptions
- [ ] Implement responsive design for mobile/desktop
- [ ] Add proper error handling for missing data
- [ ] Include last updated timestamp

## Design Requirements
- Use Bootstrap 5 cards and components
- Responsive layout for all screen sizes
- Weather icons (consider using weather icons font or images)
- Clean, modern design

## Priority
High - Primary feature" --label "enhancement,high-priority,ui,feature"

# Medium Priority Issues
Write-Host "Creating medium priority issues..." -ForegroundColor Yellow

gh issue create --title "📅 Implement 5-Day Weather Forecast" --body "## Description
Add 5-day weather forecast display with daily summaries.

## Acceptance Criteria
- [ ] Fetch 5-day forecast from OpenWeatherMap API
- [ ] Display daily weather summaries (high/low temps, conditions)
- [ ] Add weather icons for each day
- [ ] Show date and day of week
- [ ] Implement responsive grid layout
- [ ] Add proper error handling
- [ ] Include loading states

## Design Requirements
- Bootstrap 5 card grid layout
- Mobile-responsive design
- Clear visual hierarchy
- Consistent with current weather styling

## Priority
Medium - Important but secondary to current weather" --label "enhancement,medium-priority,feature"

gh issue create --title "⭐ Implement Favorites Management" --body "## Description
Allow users to save and manage favorite cities for quick access.

## Acceptance Criteria
- [ ] Add 'Add to Favorites' button on weather display
- [ ] Create favorites list display
- [ ] Implement remove from favorites functionality
- [ ] Add quick weather view for favorite cities
- [ ] Limit maximum number of favorites (e.g., 10)
- [ ] Add proper error handling for storage operations
- [ ] Include confirmation dialogs for remove actions

## Technical Requirements
- Use Azure Table Storage for persistence
- Implement proper CRUD operations
- Add client-side JavaScript for interactions
- Follow responsive design principles

## Priority
Medium - Enhances user experience" --label "enhancement,medium-priority,feature"

gh issue create --title "🚨 Implement Comprehensive Error Handling" --body "## Description
Add robust error handling and logging throughout the application.

## Acceptance Criteria
- [ ] Create custom exception classes for weather API errors
- [ ] Implement global exception handling middleware
- [ ] Add structured logging with Serilog or built-in logging
- [ ] Create user-friendly error pages
- [ ] Add retry logic for API failures
- [ ] Implement circuit breaker pattern for external calls
- [ ] Add health checks for dependencies

## Technical Requirements
- Use try-catch blocks appropriately
- Implement ILogger throughout services
- Add correlation IDs for request tracking
- Follow security best practices (don't log sensitive data)

## Priority
Medium - Important for production readiness" --label "enhancement,medium-priority,reliability"

# Low Priority Issues
Write-Host "Creating low priority issues..." -ForegroundColor Yellow

gh issue create --title "📱 Enhance Responsive Design" --body "## Description
Improve mobile and tablet experience with enhanced responsive design.

## Acceptance Criteria
- [ ] Optimize layout for mobile devices (320px+)
- [ ] Enhance tablet experience (768px+)
- [ ] Add touch-friendly interactions
- [ ] Implement dark mode support (optional)
- [ ] Add print-friendly styles
- [ ] Test across multiple browsers and devices
- [ ] Optimize performance for mobile networks

## Design Requirements
- Bootstrap 5 responsive utilities
- Mobile-first approach
- Accessible design (WCAG compliance)
- Fast loading on mobile networks

## Priority
Low - Polish and enhancement" --label "enhancement,low-priority,ui"

gh issue create --title "⚡ Implement Caching and Performance Optimizations" --body "## Description
Add caching strategies and performance optimizations.

## Acceptance Criteria
- [ ] Implement memory caching for weather data (5-10 minutes)
- [ ] Add response caching for static content
- [ ] Optimize images and static assets
- [ ] Implement lazy loading for forecast data
- [ ] Add compression for HTTP responses
- [ ] Minimize JavaScript and CSS bundles
- [ ] Add performance monitoring

## Technical Requirements
- Use IMemoryCache for weather data
- Implement cache invalidation strategies
- Add performance metrics
- Follow ASP.NET Core performance best practices

## Priority
Low - Optimization for scale" --label "enhancement,low-priority,performance"

Write-Host "All GitHub issues created successfully!" -ForegroundColor Green
Write-Host "You can view them at: https://github.com/{owner}/{repo}/issues" -ForegroundColor Cyan