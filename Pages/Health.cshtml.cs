using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WeatherDashboard.Pages;

/// <summary>
/// Page model for displaying system health status.
/// </summary>
public class HealthModel : PageModel
{
    private readonly HealthCheckService _healthCheckService;
    private readonly ILogger<HealthModel> _logger;

    /// <summary>
    /// Initializes a new instance of the HealthModel class.
    /// </summary>
    /// <param name="healthCheckService">The health check service.</param>
    /// <param name="logger">The logger for logging activities.</param>
    public HealthModel(HealthCheckService healthCheckService, ILogger<HealthModel> logger)
    {
        _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the health report from the health check service.
    /// </summary>
    public HealthReport? HealthReport { get; private set; }

    /// <summary>
    /// Handles GET requests to retrieve and display health status.
    /// </summary>
    public async Task OnGetAsync()
    {
        try
        {
            _logger.LogDebug("Retrieving system health status");
            HealthReport = await _healthCheckService.CheckHealthAsync();
            _logger.LogInformation("Health status retrieved successfully. Overall status: {Status}", HealthReport.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve health status");
            HealthReport = null;
        }
    }
}