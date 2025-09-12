using Microsoft.Extensions.Options;
using WeatherDashboard.Configuration;
using WeatherDashboard.Services;

namespace WeatherDashboard.Extensions;

/// <summary>
/// Extension methods for registering weather services with dependency injection.
/// </summary>
public static class WeatherServiceExtensions
{
    /// <summary>
    /// Adds weather services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration containing weather API settings.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddWeatherServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration
        services.Configure<WeatherApiOptions>(configuration.GetSection(WeatherApiOptions.SectionName));
        
        // Validate configuration on startup
        services.AddSingleton<IValidateOptions<WeatherApiOptions>, WeatherApiOptionsValidator>();

        // Register HTTP client for weather service
        services.AddHttpClient<IWeatherService, WeatherService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<WeatherApiOptions>>().Value;
            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);
        });

        // Register weather service as singleton since it's stateless and thread-safe
        services.AddSingleton<IWeatherService, WeatherService>();

        // Add memory caching for weather data
        services.AddMemoryCache();

        // Add health checks
        services.AddHealthChecks()
            .AddCheck<WeatherServiceHealthCheck>("weather_service", tags: new[] { "weather", "external" });

        return services;
    }

    /// <summary>
    /// Adds weather services to the service collection with a custom configuration action.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configureOptions">Action to configure weather API options.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddWeatherServices(this IServiceCollection services, Action<WeatherApiOptions> configureOptions)
    {
        // Register configuration
        services.Configure(configureOptions);
        
        // Validate configuration on startup
        services.AddSingleton<IValidateOptions<WeatherApiOptions>, WeatherApiOptionsValidator>();

        // Register HTTP client for weather service
        services.AddHttpClient<IWeatherService, WeatherService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<WeatherApiOptions>>().Value;
            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);
        });

        // Register weather service as singleton since it's stateless and thread-safe
        services.AddSingleton<IWeatherService, WeatherService>();

        // Add memory caching for weather data
        services.AddMemoryCache();

        // Add health checks
        services.AddHealthChecks()
            .AddCheck<WeatherServiceHealthCheck>("weather_service", tags: new[] { "weather", "external" });

        return services;
    }
}

/// <summary>
/// Validator for WeatherApiOptions configuration.
/// </summary>
public class WeatherApiOptionsValidator : IValidateOptions<WeatherApiOptions>
{
    /// <summary>
    /// Validates the WeatherApiOptions configuration.
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options instance to validate.</param>
    /// <returns>The validation result.</returns>
    public ValidateOptionsResult Validate(string? name, WeatherApiOptions options)
    {
        var errors = options.GetValidationErrors().ToList();
        
        if (errors.Any())
        {
            return ValidateOptionsResult.Fail(errors);
        }

        return ValidateOptionsResult.Success;
    }
}