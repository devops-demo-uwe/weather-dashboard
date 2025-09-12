using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

/// <summary>
/// Interface for weather data retrieval services.
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Gets the current weather for the specified city.
    /// </summary>
    /// <param name="cityName">The name of the city to get weather for.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The current weather data for the specified city.</returns>
    /// <exception cref="ArgumentException">Thrown when cityName is null or empty.</exception>
    /// <exception cref="WeatherServiceException">Thrown when the weather service encounters an error.</exception>
    Task<CurrentWeather> GetCurrentWeatherAsync(string cityName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current weather for the specified city and country.
    /// </summary>
    /// <param name="cityName">The name of the city to get weather for.</param>
    /// <param name="countryCode">The country code (e.g., "US", "GB").</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The current weather data for the specified city and country.</returns>
    /// <exception cref="ArgumentException">Thrown when cityName or countryCode is null or empty.</exception>
    /// <exception cref="WeatherServiceException">Thrown when the weather service encounters an error.</exception>
    Task<CurrentWeather> GetCurrentWeatherAsync(string cityName, string countryCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current weather for the specified geographic coordinates.
    /// </summary>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The current weather data for the specified coordinates.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when coordinates are out of valid range.</exception>
    /// <exception cref="WeatherServiceException">Thrown when the weather service encounters an error.</exception>
    Task<CurrentWeather> GetCurrentWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for cities matching the specified query.
    /// </summary>
    /// <param name="query">The search query for city names.</param>
    /// <param name="limit">The maximum number of results to return (default: 5, max: 10).</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of cities matching the search query.</returns>
    /// <exception cref="ArgumentException">Thrown when query is null or empty.</exception>
    /// <exception cref="WeatherServiceException">Thrown when the weather service encounters an error.</exception>
    Task<IEnumerable<CitySearchResult>> SearchCitiesAsync(string query, int limit = 5, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates that the weather service is properly configured and can connect to the API.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>True if the service is healthy, false otherwise.</returns>
    Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default);
}