using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Models;

/// <summary>
/// Represents a weather condition with details about the current weather state.
/// </summary>
public class WeatherCondition
{
    /// <summary>
    /// Gets or sets the unique identifier for the weather condition.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the main weather condition (e.g., "Rain", "Snow", "Clear").
    /// </summary>
    [Required]
    public required string Main { get; set; }

    /// <summary>
    /// Gets or sets the detailed description of the weather condition.
    /// </summary>
    [Required]
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the weather icon code from OpenWeatherMap.
    /// </summary>
    [Required]
    public required string IconCode { get; set; }

    /// <summary>
    /// Gets the URL for the weather icon from OpenWeatherMap.
    /// </summary>
    public string IconUrl => $"https://openweathermap.org/img/wn/{IconCode}@2x.png";

    /// <summary>
    /// Gets the capitalized description for display purposes.
    /// </summary>
    public string DisplayDescription => char.ToUpper(Description[0]) + Description[1..];

    /// <summary>
    /// Gets a value indicating whether the weather condition represents clear skies.
    /// </summary>
    public bool IsClear => Main.Equals("Clear", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the weather condition represents cloudy skies.
    /// </summary>
    public bool IsCloudy => Main.Equals("Clouds", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the weather condition represents rain.
    /// </summary>
    public bool IsRain => Main.Equals("Rain", StringComparison.OrdinalIgnoreCase) || 
                          Main.Equals("Drizzle", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the weather condition represents snow.
    /// </summary>
    public bool IsSnow => Main.Equals("Snow", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the weather condition represents a thunderstorm.
    /// </summary>
    public bool IsThunderstorm => Main.Equals("Thunderstorm", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the weather condition represents atmospheric conditions like fog, mist, etc.
    /// </summary>
    public bool IsAtmospheric => Main.Equals("Mist", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Smoke", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Haze", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Dust", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Fog", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Sand", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Ash", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Squall", StringComparison.OrdinalIgnoreCase) ||
                                 Main.Equals("Tornado", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the CSS class name for styling based on the weather condition.
    /// </summary>
    public string CssClass
    {
        get
        {
            return Main.ToLower() switch
            {
                "clear" => "weather-clear",
                "clouds" => "weather-clouds",
                "rain" or "drizzle" => "weather-rain",
                "snow" => "weather-snow",
                "thunderstorm" => "weather-thunderstorm",
                _ => "weather-atmospheric"
            };
        }
    }

    /// <summary>
    /// Gets the Bootstrap icon class for the weather condition.
    /// </summary>
    public string BootstrapIcon
    {
        get
        {
            return Main.ToLower() switch
            {
                "clear" => IconCode.Contains('n') ? "bi-moon-stars" : "bi-sun",
                "clouds" => "bi-clouds",
                "rain" => "bi-cloud-rain",
                "drizzle" => "bi-cloud-drizzle",
                "snow" => "bi-cloud-snow",
                "thunderstorm" => "bi-cloud-lightning",
                "mist" or "fog" => "bi-cloud-fog",
                _ => "bi-cloud"
            };
        }
    }
}