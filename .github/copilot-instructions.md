
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

## Coding Standards
- Use async/await patterns
- Enable nullable reference types
- Use proper error handling with try-catch blocks
- Implement logging
- Follow C# naming conventions
- Add XML documentation for public APIs
- Use dependency injection throughout

## Security Guidelines
- Use .NET Secret Manager for development
- Prepare for Azure Key Vault in production
- Implement input validation
- Secure API key handling
- Follow secure coding practices throughout

## GitHub Issue Management

### Setup GitHub CLI
Ensure GitHub CLI is properly configured and accessible:

```powershell
# Check if GitHub CLI is available
gh --version

# If not available, refresh environment variables after installation
$env:PATH = [Environment]::GetEnvironmentVariable("PATH", "Machine") + ";" + [Environment]::GetEnvironmentVariable("PATH", "User")

# Verify authentication
gh auth status
```

### Working with Issues
When implementing features or fixing bugs, follow this workflow:

#### Viewing Issues
```powershell
# View specific issue
gh issue view 7

# List all issues
gh issue list

# List issues with specific labels
gh issue list --label "enhancement,high-priority"
```

#### Updating Issue Progress
```powershell
# Add progress comment to issue
gh issue comment 7 --body "## ✅ Progress Update: [Feature Name] Completed

### 🏗️ **Implementation Details:**
- Feature 1: Description
- Feature 2: Description

### ✅ **Technical Achievements:**
- Achievement 1
- Achievement 2

### 📋 **Next Steps:**
- Next step 1
- Next step 2

**Commit:** [commit-hash] - [commit message]"

# Add labels to categorize work
gh issue edit 7 --add-label "in-progress"
gh issue edit 7 --add-label "enhancement"

# Close issue when complete
gh issue close 7 --comment "✅ Feature implementation completed. All acceptance criteria met."
```

#### Commit Message Standards
Follow conventional commit format for better traceability:

```
feat: implement weather data models for current weather display
fix: resolve temperature conversion accuracy issue
docs: update API documentation for weather service
style: improve responsive design for mobile devices
refactor: optimize weather service caching logic
test: add unit tests for weather model conversions
```

#### Linking Commits to Issues
Always reference issue numbers in commit messages:
```
feat: implement current weather display

- Add CurrentWeather model with temperature conversions
- Add WeatherCondition model with icon support
- Add comprehensive validation and error handling

Addresses issue #7: Implement Current Weather Display
```

### Environment Variable Management
If GitHub CLI authentication issues occur:

```powershell
# Re-authenticate if needed
gh auth login

# Set specific token if required
gh auth login --with-token

# Check current configuration
gh config list
```

### Issue Documentation Standards
When updating issues, include:

1. **Progress Summary** - What was accomplished
2. **Technical Details** - Implementation specifics
3. **Code Changes** - Files modified, features added
4. **Testing Status** - What was tested and verified
5. **Next Steps** - Clear roadmap for remaining work
6. **Commit References** - Link to specific commits

This ensures full traceability and helps stakeholders track development progress effectively.

