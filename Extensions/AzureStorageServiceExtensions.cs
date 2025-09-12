using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using WeatherDashboard.Configuration;
using WeatherDashboard.Services;

namespace WeatherDashboard.Extensions;

/// <summary>
/// Extension methods for registering Azure Storage services with dependency injection.
/// </summary>
public static class AzureStorageServiceExtensions
{
    /// <summary>
    /// Adds Azure Storage services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration containing Azure Storage settings.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddAzureStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration
        services.Configure<AzureStorageOptions>(configuration.GetSection(AzureStorageOptions.SectionName));
        
        // Validate configuration on startup
        services.AddSingleton<IValidateOptions<AzureStorageOptions>, AzureStorageOptionsValidator>();

        // Register Azure Table Storage client
        services.AddSingleton<TableServiceClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AzureStorageOptions>>().Value;
            return new TableServiceClient(options.ConnectionString);
        });

        // Register favorite city service
        services.AddScoped<IFavoriteCityService, FavoriteCityService>();

        return services;
    }

    /// <summary>
    /// Adds Azure Storage services to the service collection with a custom configuration action.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configureOptions">Action to configure Azure Storage options.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddAzureStorageServices(this IServiceCollection services, Action<AzureStorageOptions> configureOptions)
    {
        // Register configuration
        services.Configure(configureOptions);
        
        // Validate configuration on startup
        services.AddSingleton<IValidateOptions<AzureStorageOptions>, AzureStorageOptionsValidator>();

        // Register Azure Table Storage client
        services.AddSingleton<TableServiceClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AzureStorageOptions>>().Value;
            return new TableServiceClient(options.ConnectionString);
        });

        // Register favorite city service
        services.AddScoped<IFavoriteCityService, FavoriteCityService>();

        return services;
    }
}

/// <summary>
/// Validator for AzureStorageOptions configuration.
/// </summary>
public class AzureStorageOptionsValidator : IValidateOptions<AzureStorageOptions>
{
    /// <summary>
    /// Validates the AzureStorageOptions configuration.
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options instance to validate.</param>
    /// <returns>The validation result.</returns>
    public ValidateOptionsResult Validate(string? name, AzureStorageOptions options)
    {
        var errors = options.GetValidationErrors().ToList();
        
        if (errors.Any())
        {
            return ValidateOptionsResult.Fail(errors);
        }

        return ValidateOptionsResult.Success;
    }
}