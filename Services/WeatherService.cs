using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherDashboard.Configuration;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

/// <summary>
/// Service for retrieving weather data from the OpenWeatherMap API.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WeatherService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the WeatherService class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making API requests.</param>
    /// <param name="options">The weather API configuration options.</param>
    /// <param name="cache">The memory cache for caching weather data.</param>
    /// <param name="logger">The logger for logging service activities.</param>
    public WeatherService(
        HttpClient httpClient,
        IOptions<WeatherApiOptions> options,
        IMemoryCache cache,
        ILogger<WeatherService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Configure JSON serialization options
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Configure HTTP client
        ConfigureHttpClient();
    }

    /// <inheritdoc />
    public async Task<CurrentWeather> GetCurrentWeatherAsync(string cityName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            throw new ArgumentException("City name cannot be null or empty.", nameof(cityName));

        _logger.LogInformation("Getting current weather for city: {CityName}", cityName);

        var cacheKey = $"weather_city_{cityName.ToLowerInvariant()}";
        
        if (_options.EnableCaching && _cache.TryGetValue(cacheKey, out CurrentWeather? cachedWeather))
        {
            _logger.LogDebug("Returning cached weather data for city: {CityName}", cityName);
            return cachedWeather!;
        }

        try
        {
            var url = $"{_options.BaseUrl}/weather?q={Uri.EscapeDataString(cityName)}&appid={_options.ApiKey}&units={_options.Units}&lang={_options.Language}";
            var response = await GetApiResponseAsync<WeatherApiResponse>(url, cancellationToken);
            var weather = response.ToCurrentWeather();

            if (_options.EnableCaching)
            {
                _cache.Set(cacheKey, weather, _options.CacheDuration);
                _logger.LogDebug("Cached weather data for city: {CityName}", cityName);
            }

            _logger.LogInformation("Successfully retrieved weather data for city: {CityName}", cityName);
            return weather;
        }
        catch (WeatherServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting weather for city: {CityName}", cityName);
            throw new WeatherServiceException($"Failed to get weather data for city '{cityName}'.", ex);
        }
    }

    /// <inheritdoc />
    public async Task<CurrentWeather> GetCurrentWeatherAsync(string cityName, string countryCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            throw new ArgumentException("City name cannot be null or empty.", nameof(cityName));

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Country code cannot be null or empty.", nameof(countryCode));

        _logger.LogInformation("Getting current weather for city: {CityName}, country: {CountryCode}", cityName, countryCode);

        var query = $"{cityName},{countryCode}";
        var cacheKey = $"weather_city_{query.ToLowerInvariant()}";
        
        if (_options.EnableCaching && _cache.TryGetValue(cacheKey, out CurrentWeather? cachedWeather))
        {
            _logger.LogDebug("Returning cached weather data for city: {CityName}, country: {CountryCode}", cityName, countryCode);
            return cachedWeather!;
        }

        try
        {
            var url = $"{_options.BaseUrl}/weather?q={Uri.EscapeDataString(query)}&appid={_options.ApiKey}&units={_options.Units}&lang={_options.Language}";
            var response = await GetApiResponseAsync<WeatherApiResponse>(url, cancellationToken);
            var weather = response.ToCurrentWeather();

            if (_options.EnableCaching)
            {
                _cache.Set(cacheKey, weather, _options.CacheDuration);
                _logger.LogDebug("Cached weather data for city: {CityName}, country: {CountryCode}", cityName, countryCode);
            }

            _logger.LogInformation("Successfully retrieved weather data for city: {CityName}, country: {CountryCode}", cityName, countryCode);
            return weather;
        }
        catch (WeatherServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting weather for city: {CityName}, country: {CountryCode}", cityName, countryCode);
            throw new WeatherServiceException($"Failed to get weather data for city '{cityName}', country '{countryCode}'.", ex);
        }
    }

    /// <inheritdoc />
    public async Task<CurrentWeather> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");

        _logger.LogInformation("Getting current weather for coordinates: {Latitude}, {Longitude}", latitude, longitude);

        var cacheKey = $"weather_coords_{latitude:F2}_{longitude:F2}";
        
        if (_options.EnableCaching && _cache.TryGetValue(cacheKey, out CurrentWeather? cachedWeather))
        {
            _logger.LogDebug("Returning cached weather data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            return cachedWeather!;
        }

        try
        {
            var url = $"{_options.BaseUrl}/weather?lat={latitude}&lon={longitude}&appid={_options.ApiKey}&units={_options.Units}&lang={_options.Language}";
            var response = await GetApiResponseAsync<WeatherApiResponse>(url, cancellationToken);
            var weather = response.ToCurrentWeather();

            if (_options.EnableCaching)
            {
                _cache.Set(cacheKey, weather, _options.CacheDuration);
                _logger.LogDebug("Cached weather data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            }

            _logger.LogInformation("Successfully retrieved weather data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            return weather;
        }
        catch (WeatherServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error getting weather for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            throw new WeatherServiceException($"Failed to get weather data for coordinates ({latitude}, {longitude}).", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CitySearchResult>> SearchCitiesAsync(string query, int limit = 5, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Search query cannot be null or empty.", nameof(query));

        if (limit < 1 || limit > 10)
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 10.");

        _logger.LogInformation("Searching for cities with query: {Query}, limit: {Limit}", query, limit);

        var cacheKey = $"search_{query.ToLowerInvariant()}_{limit}";
        
        if (_options.EnableCaching && _cache.TryGetValue(cacheKey, out IEnumerable<CitySearchResult>? cachedResults))
        {
            _logger.LogDebug("Returning cached search results for query: {Query}", query);
            return cachedResults!;
        }

        try
        {
            var url = $"{_options.GeocodingBaseUrl}/direct?q={Uri.EscapeDataString(query)}&limit={limit}&appid={_options.ApiKey}";
            var response = await GetApiResponseAsync<GeocodingResponse[]>(url, cancellationToken);
            
            var results = response.Select(r => CitySearchResult.Create(
                r.Name ?? "Unknown",
                r.Country ?? "Unknown",
                r.Lat,
                r.Lon,
                r.State
            )).ToList();

            if (_options.EnableCaching)
            {
                _cache.Set(cacheKey, results, TimeSpan.FromMinutes(60)); // Cache search results for 1 hour
                _logger.LogDebug("Cached search results for query: {Query}", query);
            }

            _logger.LogInformation("Successfully found {Count} cities for query: {Query}", results.Count, query);
            return results;
        }
        catch (WeatherServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error searching for cities with query: {Query}", query);
            throw new WeatherServiceException($"Failed to search for cities with query '{query}'.", ex);
        }
    }

    /// <inheritdoc />
    public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Performing health check");
            
            // Test with a simple weather request for a well-known city
            var url = $"{_options.BaseUrl}/weather?q=London,GB&appid={_options.ApiKey}&units={_options.Units}";
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            var isHealthy = response.IsSuccessStatusCode;
            _logger.LogInformation("Health check result: {IsHealthy}", isHealthy);
            
            return isHealthy;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return false;
        }
    }

    private void ConfigureHttpClient()
    {
        _httpClient.Timeout = _options.Timeout;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _options.UserAgent);
    }

    private async Task<T> GetApiResponseAsync<T>(string url, CancellationToken cancellationToken)
    {
        var attempt = 0;
        Exception? lastException = null;

        while (attempt <= _options.MaxRetries)
        {
            try
            {
                _logger.LogDebug("Making API request to: {Url} (attempt {Attempt})", url, attempt + 1);
                
                var response = await _httpClient.GetAsync(url, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync(cancellationToken);
                    var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                    
                    if (result == null)
                        throw new WeatherServiceException("Failed to deserialize API response.");
                    
                    return result;
                }

                await HandleErrorResponse(response);
            }
            catch (WeatherServiceException)
            {
                throw;
            }
            catch (HttpRequestException ex)
            {
                lastException = ex;
                _logger.LogWarning(ex, "HTTP request failed (attempt {Attempt}): {Message}", attempt + 1, ex.Message);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                lastException = ex;
                _logger.LogWarning(ex, "Request timeout (attempt {Attempt})", attempt + 1);
            }
            catch (Exception ex)
            {
                lastException = ex;
                _logger.LogWarning(ex, "Unexpected error during API request (attempt {Attempt}): {Message}", attempt + 1, ex.Message);
            }

            attempt++;
            
            if (attempt <= _options.MaxRetries)
            {
                var delay = TimeSpan.FromMilliseconds(_options.RetryDelayMs * attempt);
                _logger.LogDebug("Retrying in {Delay}ms", delay.TotalMilliseconds);
                await Task.Delay(delay, cancellationToken);
            }
        }

        _logger.LogError("All retry attempts failed for URL: {Url}", url);
        throw WeatherServiceException.NetworkError(lastException ?? new Exception("All retry attempts failed"));
    }

    private static async Task HandleErrorResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                throw WeatherServiceException.CityNotFound("City not found");
            case HttpStatusCode.Unauthorized:
                throw WeatherServiceException.InvalidApiKey();
            case HttpStatusCode.TooManyRequests:
                throw WeatherServiceException.RateLimitExceeded();
            case HttpStatusCode.ServiceUnavailable:
                throw WeatherServiceException.ServiceUnavailable();
            default:
                throw new WeatherServiceException($"API request failed with status {response.StatusCode}: {content}", 
                    httpStatusCode: (int)response.StatusCode);
        }
    }
}

/// <summary>
/// Represents a geocoding API response from OpenWeatherMap.
/// </summary>
public class GeocodingResponse
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}