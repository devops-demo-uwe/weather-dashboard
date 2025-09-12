using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Models;

/// <summary>
/// Represents the current weather conditions for a specific location.
/// </summary>
public class CurrentWeather
{
    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    [Required]
    public required string CityName { get; set; }

    /// <summary>
    /// Gets or sets the country code (e.g., "US", "GB").
    /// </summary>
    [Required]
    public required string Country { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate.
    /// </summary>
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90 degrees")]
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate.
    /// </summary>
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180 degrees")]
    public double Longitude { get; set; }

    /// <summary>
    /// Gets or sets the current temperature in Celsius.
    /// </summary>
    [Range(-100, 100, ErrorMessage = "Temperature must be between -100 and 100 degrees Celsius")]
    public double TemperatureCelsius { get; set; }

    /// <summary>
    /// Gets the current temperature in Fahrenheit.
    /// </summary>
    public double TemperatureFahrenheit => (TemperatureCelsius * 9.0 / 5.0) + 32.0;

    /// <summary>
    /// Gets or sets the "feels like" temperature in Celsius.
    /// </summary>
    [Range(-100, 100, ErrorMessage = "Feels like temperature must be between -100 and 100 degrees Celsius")]
    public double FeelsLikeCelsius { get; set; }

    /// <summary>
    /// Gets the "feels like" temperature in Fahrenheit.
    /// </summary>
    public double FeelsLikeFahrenheit => (FeelsLikeCelsius * 9.0 / 5.0) + 32.0;

    /// <summary>
    /// Gets or sets the weather condition information.
    /// </summary>
    [Required]
    public required WeatherCondition Condition { get; set; }

    /// <summary>
    /// Gets or sets the humidity percentage.
    /// </summary>
    [Range(0, 100, ErrorMessage = "Humidity must be between 0 and 100 percent")]
    public int Humidity { get; set; }

    /// <summary>
    /// Gets or sets the atmospheric pressure in hPa (hectopascals).
    /// </summary>
    [Range(800, 1200, ErrorMessage = "Pressure must be between 800 and 1200 hPa")]
    public double Pressure { get; set; }

    /// <summary>
    /// Gets or sets the wind speed in meters per second.
    /// </summary>
    [Range(0, 200, ErrorMessage = "Wind speed must be between 0 and 200 m/s")]
    public double WindSpeed { get; set; }

    /// <summary>
    /// Gets or sets the wind direction in degrees (0-360).
    /// </summary>
    [Range(0, 360, ErrorMessage = "Wind direction must be between 0 and 360 degrees")]
    public int WindDirection { get; set; }

    /// <summary>
    /// Gets or sets the visibility in meters.
    /// </summary>
    [Range(0, 50000, ErrorMessage = "Visibility must be between 0 and 50,000 meters")]
    public double Visibility { get; set; }

    /// <summary>
    /// Gets or sets the UV index.
    /// </summary>
    [Range(0, 15, ErrorMessage = "UV Index must be between 0 and 15")]
    public double UvIndex { get; set; }

    /// <summary>
    /// Gets or sets the time when the weather data was last updated.
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the sunrise time in UTC.
    /// </summary>
    public DateTime Sunrise { get; set; }

    /// <summary>
    /// Gets or sets the sunset time in UTC.
    /// </summary>
    public DateTime Sunset { get; set; }

    /// <summary>
    /// Gets the wind direction as a compass direction (N, NE, E, etc.).
    /// </summary>
    public string WindDirectionCompass
    {
        get
        {
            var directions = new[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            var index = (int)Math.Round(WindDirection / 22.5) % 16;
            return directions[index];
        }
    }

    /// <summary>
    /// Gets the wind speed in kilometers per hour.
    /// </summary>
    public double WindSpeedKmh => WindSpeed * 3.6;

    /// <summary>
    /// Gets the wind speed in miles per hour.
    /// </summary>
    public double WindSpeedMph => WindSpeed * 2.237;
}