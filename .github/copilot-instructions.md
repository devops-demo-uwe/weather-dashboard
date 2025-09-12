
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

## Development Guidelines

### User Interaction Rules
- **Never close a GitHub issue without checking with the user first** - Always ask for confirmation before closing any issue, even if implementation appears complete
- **Do not create more code than requested** - Before adding additional functionality not immediately covered by the specification, ask the user for permission
- **Stick to the scope** - Focus on the specific requirements and avoid feature creep without explicit user approval
- **Confirm before major changes** - Always verify with the user before making significant architectural or design decisions

### GitHub Issue Management

### Feature Branch Workflow
Always use feature branches for new development work. Never commit directly to the main branch.

#### Creating Feature Branches
Before starting work on any new feature or issue:

```powershell
# Ensure you're on main branch and up to date
git checkout main
git pull origin main

# Create and switch to a new feature branch
# Use descriptive names with issue number when applicable
git checkout -b feature/issue-7-current-weather-display
git checkout -b feature/city-search-functionality
git checkout -b fix/temperature-conversion-bug
git checkout -b docs/api-documentation-update
```

#### Branch Naming Conventions
- **Features:** `feature/issue-#-brief-description` or `feature/brief-description`
- **Bug fixes:** `fix/issue-#-brief-description` or `fix/brief-description`
- **Documentation:** `docs/brief-description`
- **Refactoring:** `refactor/brief-description`
- **Tests:** `test/brief-description`

#### Working on Feature Branches
```powershell
# Make your changes and commit regularly
git add .
git commit -m "feat: implement current weather display models"

# Push your feature branch to remote
git push -u origin feature/issue-7-current-weather-display

# Continue making commits as needed
git add .
git commit -m "feat: add weather API service integration"
git push
```

#### Pull Request Workflow
When your feature is complete:

1. **Push final changes:**
```powershell
git push
```

2. **Create Pull Request:**
```powershell
# Create PR using GitHub CLI (recommended)
gh pr create --title "feat: Implement Current Weather Display (Issue #7)" --body "## 🚀 **Feature Implementation**

### 📋 **Summary**
Implements current weather display functionality as specified in issue #7.

### 🏗️ **Changes Made:**
- Added CurrentWeather and WeatherCondition models
- Implemented WeatherService with OpenWeatherMap API integration
- Added responsive UI components for weather display
- Included comprehensive error handling and validation

### ✅ **Testing:**
- [ ] Unit tests added for weather models
- [ ] Integration tests for weather service
- [ ] Manual testing on desktop and mobile
- [ ] Error scenarios validated

### 🔗 **Related Issues:**
Closes #7

### 📸 **Screenshots/Demo:**
(Add screenshots or demo links if applicable)

**Ready for review** ✅"

# Alternative: Create PR through web interface
gh pr view --web
```

3. **Remind the user about the PR:**
Always remind the user that a pull request should be created after feature completion:

> 🔔 **Important Reminder:** 
> 
> Your feature implementation is complete! Please create a pull request to merge your changes:
> 1. Push your final changes: `git push`
> 2. Create a PR: `gh pr create` or through GitHub web interface
> 3. Request review from team members
> 4. Merge after approval and delete the feature branch

#### Post-Merge Cleanup
After the pull request is merged:

```powershell
# Switch back to main and pull latest changes
git checkout main
git pull origin main

# Delete the local feature branch
git branch -d feature/issue-7-current-weather-display

# Delete the remote feature branch (if not auto-deleted)
git push origin --delete feature/issue-7-current-weather-display
```

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

# IMPORTANT: Always ask user before closing issues
# gh issue close 7 --comment "✅ Feature implementation completed. All acceptance criteria met."
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

