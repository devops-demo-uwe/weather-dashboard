using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WeatherDashboard.Models;
using WeatherDashboard.Services;

namespace WeatherDashboard.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWeatherService _weatherService;

    public IndexModel(ILogger<IndexModel> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
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
    public void OnGet()
    {
        _logger.LogInformation("Index page loaded");
    }

    /// <summary>
    /// Handles POST requests when the user searches for weather
    /// </summary>
    /// <returns>Page result with weather data or error message</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogInformation("Weather search initiated for city: {CityName}", CityName);

        // Clear previous messages
        ErrorMessage = null;
        SuccessMessage = null;
        CurrentWeather = null;

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

        return Page();
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
