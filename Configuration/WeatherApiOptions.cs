using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Configuration;

/// <summary>
/// Configuration options for the OpenWeatherMap API service.
/// </summary>
public class WeatherApiOptions
{
    /// <summary>
    /// The configuration section name for weather API settings.
    /// </summary>
    public const string SectionName = "WeatherApi";

    /// <summary>
    /// Gets or sets the OpenWeatherMap API key.
    /// </summary>
    [Required(ErrorMessage = "OpenWeatherMap API key is required")]
    public required string ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the base URL for the OpenWeatherMap API.
    /// </summary>
    [Required(ErrorMessage = "API base URL is required")]
    [Url(ErrorMessage = "API base URL must be a valid URL")]
    public string BaseUrl { get; set; } = "https://api.openweathermap.org/data/2.5";

    /// <summary>
    /// Gets or sets the base URL for the OpenWeatherMap Geocoding API.
    /// </summary>
    [Required(ErrorMessage = "Geocoding API base URL is required")]
    [Url(ErrorMessage = "Geocoding API base URL must be a valid URL")]
    public string GeocodingBaseUrl { get; set; } = "https://api.openweathermap.org/geo/1.0";

    /// <summary>
    /// Gets or sets the timeout in seconds for API requests.
    /// </summary>
    [Range(1, 300, ErrorMessage = "Timeout must be between 1 and 300 seconds")]
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Gets or sets the maximum number of retry attempts for failed requests.
    /// </summary>
    [Range(0, 5, ErrorMessage = "Max retries must be between 0 and 5")]
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the delay in milliseconds between retry attempts.
    /// </summary>
    [Range(100, 10000, ErrorMessage = "Retry delay must be between 100 and 10000 milliseconds")]
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// Gets or sets whether to cache weather data.
    /// </summary>
    public bool EnableCaching { get; set; } = true;

    /// <summary>
    /// Gets or sets the cache duration in minutes for weather data.
    /// </summary>
    [Range(1, 60, ErrorMessage = "Cache duration must be between 1 and 60 minutes")]
    public int CacheDurationMinutes { get; set; } = 10;

    /// <summary>
    /// Gets or sets the user agent string for API requests.
    /// </summary>
    public string UserAgent { get; set; } = "WeatherDashboard/1.0";

    /// <summary>
    /// Gets or sets the preferred units for temperature (metric, imperial, kelvin).
    /// </summary>
    public string Units { get; set; } = "metric";

    /// <summary>
    /// Gets or sets the preferred language for weather descriptions.
    /// </summary>
    public string Language { get; set; } = "en";

    /// <summary>
    /// Gets the timeout as a TimeSpan.
    /// </summary>
    public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutSeconds);

    /// <summary>
    /// Gets the retry delay as a TimeSpan.
    /// </summary>
    public TimeSpan RetryDelay => TimeSpan.FromMilliseconds(RetryDelayMs);

    /// <summary>
    /// Gets the cache duration as a TimeSpan.
    /// </summary>
    public TimeSpan CacheDuration => TimeSpan.FromMinutes(CacheDurationMinutes);

    /// <summary>
    /// Validates the configuration options.
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise.</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ApiKey) &&
               !string.IsNullOrWhiteSpace(BaseUrl) &&
               !string.IsNullOrWhiteSpace(GeocodingBaseUrl) &&
               TimeoutSeconds > 0 &&
               MaxRetries >= 0 &&
               RetryDelayMs > 0 &&
               CacheDurationMinutes > 0;
    }

    /// <summary>
    /// Gets the validation errors for the current configuration.
    /// </summary>
    /// <returns>A collection of validation errors.</returns>
    public IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(ApiKey))
            errors.Add("API Key is required");

        if (string.IsNullOrWhiteSpace(BaseUrl))
            errors.Add("Base URL is required");

        if (string.IsNullOrWhiteSpace(GeocodingBaseUrl))
            errors.Add("Geocoding Base URL is required");

        if (TimeoutSeconds <= 0)
            errors.Add("Timeout must be greater than 0 seconds");

        if (MaxRetries < 0)
            errors.Add("Max retries cannot be negative");

        if (RetryDelayMs <= 0)
            errors.Add("Retry delay must be greater than 0 milliseconds");

        if (CacheDurationMinutes <= 0)
            errors.Add("Cache duration must be greater than 0 minutes");

        return errors;
    }
}