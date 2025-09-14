using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WeatherDashboard.Models;
using WeatherDashboard.Services;

namespace WeatherDashboard.Pages;

/// <summary>
/// Request model for adding a favorite city
/// </summary>
public class AddFavoriteRequest
{
    public required string CityName { get; set; }
    public required string Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWeatherService _weatherService;
    private readonly IFavoriteCityService _favoriteCityService;

    public IndexModel(ILogger<IndexModel> logger, IWeatherService weatherService, IFavoriteCityService favoriteCityService)
    {
        _logger = logger;
        _weatherService = weatherService;
        _favoriteCityService = favoriteCityService;
    }

    /// <summary>
    /// The city name entered by the user for weather search
    /// </summary>
    [BindProperty]
    [Required(ErrorMessage = "Please enter a city name")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 100 characters")]
    [Display(Name = "City Name")]
    public string CityName { get; set; } = string.Empty;

    /// <summary>
    /// Current weather data for the searched city
    /// </summary>
    public CurrentWeather? CurrentWeather { get; set; }

    /// <summary>
    /// List of favorite cities for the user
    /// </summary>
    public IEnumerable<FavoriteCity> FavoriteCities { get; set; } = Enumerable.Empty<FavoriteCity>();

    /// <summary>
    /// Indicates if the current city is already in favorites
    /// </summary>
    public bool IsCurrentCityFavorite { get; set; }

    /// <summary>
    /// Error message to display to the user
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Success message to display to the user
    /// </summary>
    public string? SuccessMessage { get; set; }

    /// <summary>
    /// Indicates if the page is currently loading weather data
    /// </summary>
    public bool IsLoading { get; set; }

    /// <summary>
    /// Handles GET requests to the page
    /// </summary>
    public async Task OnGetAsync()
    {
        _logger.LogInformation("Index page loaded");
        
        // Load favorite cities
        await LoadFavoriteCitiesAsync();
    }

    /// <summary>
    /// Handles POST requests when the user searches for weather
    /// </summary>
    /// <returns>Page result with weather data or error message</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("OnPostAsync called - Weather search initiated for city: {CityName}", CityName ?? "null");

        // Clear previous messages
        ErrorMessage = null;
        SuccessMessage = null;
        CurrentWeather = null;

        // Validate that this is actually a search request
        if (string.IsNullOrEmpty(CityName))
        {
            _logger.LogWarning("OnPostAsync called with empty CityName");
            ModelState.AddModelError(nameof(CityName), "Please enter a city name");
        }

        // Validate model state
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for city search");
            return Page();
        }

        try
        {
            IsLoading = true;
            
            // Get weather data from the service
            CurrentWeather = await _weatherService.GetCurrentWeatherAsync(CityName.Trim());
            
            if (CurrentWeather != null)
            {
                SuccessMessage = $"Weather data retrieved successfully for {CurrentWeather.CityName}";
                _logger.LogInformation("Weather data retrieved successfully for {CityName}", CurrentWeather.CityName);
                
                // Check if current city is in favorites
                await CheckIfCurrentCityIsFavoriteAsync();
            }
            else
            {
                ErrorMessage = "No weather data found for the specified city";
                _logger.LogWarning("No weather data returned for city: {CityName}", CityName);
            }
        }
        catch (WeatherServiceException ex)
        {
            ErrorMessage = GetUserFriendlyErrorMessage(ex);
            _logger.LogError(ex, "Weather service error occurred while searching for {CityName}", CityName);
        }
        catch (Exception ex)
        {
            ErrorMessage = "An unexpected error occurred. Please try again later.";
            _logger.LogError(ex, "Unexpected error occurred while searching for weather data for {CityName}", CityName);
        }
        finally
        {
            IsLoading = false;
        }

        // Reload favorite cities
        await LoadFavoriteCitiesAsync();

        return Page();
    }

    /// <summary>
    /// Handles POST request to add a city to favorites
    /// </summary>
    /// <returns>JSON result indicating success or failure</returns>
    public async Task<IActionResult> OnPostAddFavoriteAsync()
    {
        try
        {
            // Read the request body to get weather data
            using var reader = new StreamReader(Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            
            if (string.IsNullOrEmpty(requestBody))
            {
                return new JsonResult(new { success = false, message = "No weather data provided" });
            }

            var addFavoriteRequest = System.Text.Json.JsonSerializer.Deserialize<AddFavoriteRequest>(requestBody, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (addFavoriteRequest == null)
            {
                return new JsonResult(new { success = false, message = "Invalid weather data format" });
            }

            // Validate the request data
            if (string.IsNullOrWhiteSpace(addFavoriteRequest.CityName) || string.IsNullOrWhiteSpace(addFavoriteRequest.Country))
            {
                return new JsonResult(new { success = false, message = "City name and country are required" });
            }

            var favoriteCity = await _favoriteCityService.AddFavoriteAsync(
                addFavoriteRequest.CityName,
                addFavoriteRequest.Country,
                addFavoriteRequest.Latitude,
                addFavoriteRequest.Longitude);

            _logger.LogInformation("Added favorite city: {CityName}, {Country}, Id: {Id}", 
                favoriteCity.CityName, favoriteCity.Country, favoriteCity.Id);

            return new JsonResult(new { 
                success = true, 
                message = $"Added {favoriteCity.DisplayName} to favorites",
                favoriteId = favoriteCity.Id
            });
        }
        catch (FavoriteCityServiceException ex)
        {
            _logger.LogWarning(ex, "Failed to add favorite city from request");
            return new JsonResult(new { success = false, message = ex.Message });
        }
        catch (System.Text.Json.JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON format in add favorite request");
            return new JsonResult(new { success = false, message = "Invalid request format" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding favorite city from request");
            return new JsonResult(new { success = false, message = "Failed to add city to favorites" });
        }
    }

    /// <summary>
    /// Handles POST request to remove a city from favorites
    /// </summary>
    /// <param name="favoriteId">The ID of the favorite city to remove</param>
    /// <returns>JSON result indicating success or failure</returns>
    public async Task<IActionResult> OnPostRemoveFavoriteAsync(string favoriteId)
    {
        if (string.IsNullOrEmpty(favoriteId))
        {
            return new JsonResult(new { success = false, message = "Invalid favorite ID" });
        }

        try
        {
            var removed = await _favoriteCityService.RemoveFavoriteAsync(favoriteId);
            
            if (removed)
            {
                _logger.LogInformation("Removed favorite city with ID: {FavoriteId}", favoriteId);
                return new JsonResult(new { success = true, message = "Removed from favorites" });
            }
            else
            {
                _logger.LogWarning("Favorite city not found for removal: {FavoriteId}", favoriteId);
                return new JsonResult(new { success = false, message = "Favorite city not found" });
            }
        }
        catch (FavoriteCityServiceException ex)
        {
            _logger.LogWarning(ex, "Failed to remove favorite city: {FavoriteId}", favoriteId);
            return new JsonResult(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error removing favorite city: {FavoriteId}", favoriteId);
            return new JsonResult(new { success = false, message = "Failed to remove city from favorites" });
        }
    }

    /// <summary>
    /// Handles POST request to load weather for a favorite city
    /// </summary>
    /// <param name="favoriteId">The ID of the favorite city</param>
    /// <returns>Redirect to the same page with weather data loaded</returns>
    public async Task<IActionResult> OnPostLoadFavoriteWeatherAsync(string favoriteId)
    {
        _logger.LogDebug("OnPostLoadFavoriteWeatherAsync called with favoriteId: '{FavoriteId}'", favoriteId ?? "null");
        
        // Check if this is actually meant to be a regular search request
        if (string.IsNullOrEmpty(favoriteId))
        {
            _logger.LogWarning("OnPostLoadFavoriteWeatherAsync called with empty or null favoriteId - redirecting to regular search");
            
            // If CityName is provided, this was probably meant to be a regular search
            if (!string.IsNullOrEmpty(CityName))
            {
                _logger.LogInformation("Redirecting to regular search for city: {CityName}", CityName);
                return await OnPostAsync();
            }
            
            ErrorMessage = "Invalid favorite city ID provided";
            await LoadFavoriteCitiesAsync();
            return Page();
        }

        try
        {
            var favorite = await _favoriteCityService.GetFavoriteByIdAsync(favoriteId);
            if (favorite == null)
            {
                ErrorMessage = "Favorite city not found";
                await LoadFavoriteCitiesAsync();
                return Page();
            }

            // Update last accessed time
            await _favoriteCityService.UpdateLastAccessedAsync(favoriteId);

            // Load weather for this city
            CityName = favorite.CityName;
            CurrentWeather = await _weatherService.GetCurrentWeatherAsync($"{favorite.CityName},{favorite.Country}");
            
            if (CurrentWeather != null)
            {
                SuccessMessage = $"Weather loaded for {favorite.DisplayName}";
                await CheckIfCurrentCityIsFavoriteAsync();
            }
            else
            {
                ErrorMessage = $"Unable to load weather data for {favorite.DisplayName}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading weather for favorite city: {FavoriteId}", favoriteId);
            ErrorMessage = "Failed to load weather data for favorite city";
        }

        await LoadFavoriteCitiesAsync();
        return Page();
    }

    /// <summary>
    /// Handles GET request to fetch favorites via AJAX
    /// </summary>
    /// <returns>JSON result with list of favorite cities</returns>
    public async Task<IActionResult> OnGetFavoritesAsync()
    {
        try
        {
            var favorites = await _favoriteCityService.GetFavoritesAsync();
            return new JsonResult(new { success = true, favorites = favorites });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading favorites via AJAX");
            return new JsonResult(new { success = false, message = "Failed to load favorites" });
        }
    }

    /// <summary>
    /// Loads the list of favorite cities from the service
    /// </summary>
    private async Task LoadFavoriteCitiesAsync()
    {
        try
        {
            FavoriteCities = await _favoriteCityService.GetFavoritesAsync();
            _logger.LogDebug("Loaded {Count} favorite cities", FavoriteCities.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading favorite cities");
            FavoriteCities = Enumerable.Empty<FavoriteCity>();
        }
    }

    /// <summary>
    /// Checks if the current weather city is already in favorites
    /// </summary>
    private async Task CheckIfCurrentCityIsFavoriteAsync()
    {
        if (CurrentWeather == null)
        {
            IsCurrentCityFavorite = false;
            return;
        }

        try
        {
            IsCurrentCityFavorite = await _favoriteCityService.IsFavoriteAsync(
                CurrentWeather.CityName, CurrentWeather.Country);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error checking if city is favorite: {CityName}", CurrentWeather.CityName);
            IsCurrentCityFavorite = false;
        }
    }

    /// <summary>
    /// Converts technical weather service exceptions into user-friendly error messages
    /// </summary>
    /// <param name="exception">The weather service exception</param>
    /// <returns>User-friendly error message</returns>
    private string GetUserFriendlyErrorMessage(WeatherServiceException exception)
    {
        return exception.Message switch
        {
            var msg when msg.Contains("404") || msg.Contains("not found") => 
                "City not found. Please check the spelling and try again.",
            var msg when msg.Contains("401") || msg.Contains("unauthorized") => 
                "Weather service is temporarily unavailable. Please try again later.",
            var msg when msg.Contains("429") || msg.Contains("rate limit") => 
                "Too many requests. Please wait a moment and try again.",
            var msg when msg.Contains("timeout") || msg.Contains("network") => 
                "Network connection issue. Please check your internet connection and try again.",
            _ => "Unable to retrieve weather data. Please try again later."
        };
    }
}
