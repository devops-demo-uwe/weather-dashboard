using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

/// <summary>
/// Service interface for managing favorite cities in Azure Table Storage.
/// </summary>
public interface IFavoriteCityService
{
    /// <summary>
    /// Gets all favorite cities for a user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of favorite cities.</returns>
    Task<IEnumerable<FavoriteCity>> GetFavoritesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a favorite city by its ID.
    /// </summary>
    /// <param name="id">The ID of the favorite city.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The favorite city if found, null otherwise.</returns>
    Task<FavoriteCity?> GetFavoriteByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new favorite city.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created favorite city.</returns>
    Task<FavoriteCity> AddFavoriteAsync(string cityName, string country, double latitude, double longitude, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a favorite city by its ID.
    /// </summary>
    /// <param name="id">The ID of the favorite city to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the favorite was removed, false if it was not found.</returns>
    Task<bool> RemoveFavoriteAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the last accessed time for a favorite city.
    /// </summary>
    /// <param name="id">The ID of the favorite city.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the favorite was updated, false if it was not found.</returns>
    Task<bool> UpdateLastAccessedAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a city is already in the favorites list.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the city is already a favorite, false otherwise.</returns>
    Task<bool> IsFavoriteAsync(string cityName, string country, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of favorite cities.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of favorite cities.</returns>
    Task<int> GetFavoritesCountAsync(CancellationToken cancellationToken = default);
}