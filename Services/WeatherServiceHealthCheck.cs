using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WeatherDashboard.Services;

/// <summary>
/// Health check for the weather service to ensure it's properly configured and accessible.
/// </summary>
public class WeatherServiceHealthCheck : IHealthCheck
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherServiceHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the WeatherServiceHealthCheck class.
    /// </summary>
    /// <param name="weatherService">The weather service to check.</param>
    /// <param name="logger">The logger for logging health check activities.</param>
    public WeatherServiceHealthCheck(IWeatherService weatherService, ILogger<WeatherServiceHealthCheck> logger)
    {
        _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Performs the health check for the weather service.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The health check result.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Starting weather service health check");
            
            var isHealthy = await _weatherService.IsHealthyAsync(cancellationToken);
            
            if (isHealthy)
            {
                _logger.LogDebug("Weather service health check passed");
                return HealthCheckResult.Healthy("Weather service is accessible and responding normally.");
            }
            else
            {
                _logger.LogWarning("Weather service health check failed - service not responding properly");
                return HealthCheckResult.Unhealthy("Weather service is not responding properly.");
            }
        }
        catch (WeatherServiceException ex)
        {
            _logger.LogError(ex, "Weather service health check failed with WeatherServiceException");
            return HealthCheckResult.Unhealthy($"Weather service error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Weather service health check failed with unexpected exception");
            return HealthCheckResult.Unhealthy($"Unexpected error during weather service health check: {ex.Message}", ex);
        }
    }
}