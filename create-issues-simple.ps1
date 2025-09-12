# Simple GitHub Issues Creation Script
Write-Host "Creating weather dashboard issues..." -ForegroundColor Green

# Create issues without labels first
gh issue create --title "🔧 Setup OpenWeatherMap API Integration" --body "Set up integration with OpenWeatherMap API to fetch weather data."

gh issue create --title "🗄️ Setup Azure Table Storage for Favorites" --body "Configure Azure Table Storage to store user's favorite cities."

gh issue create --title "📊 Create Weather Data Models" --body "Create strongly-typed models for weather data with proper validation."

gh issue create --title "🔍 Implement City Search Functionality" --body "Add real-time city search with weather data retrieval."

gh issue create --title "🌤️ Implement Current Weather Display" --body "Create a comprehensive current weather display component."

gh issue create --title "📅 Implement 5-Day Weather Forecast" --body "Add 5-day weather forecast display with daily summaries."

gh issue create --title "⭐ Implement Favorites Management" --body "Allow users to save and manage favorite cities for quick access."

gh issue create --title "🚨 Implement Comprehensive Error Handling" --body "Add robust error handling and logging throughout the application."

gh issue create --title "📱 Enhance Responsive Design" --body "Improve mobile and tablet experience with enhanced responsive design."

gh issue create --title "⚡ Implement Caching and Performance Optimizations" --body "Add caching strategies and performance optimizations."

Write-Host "Issues created successfully!" -ForegroundColor Green
gh issue list