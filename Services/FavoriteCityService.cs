using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using WeatherDashboard.Configuration;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

/// <summary>
/// Service for managing favorite cities in Azure Table Storage.
/// </summary>
public class FavoriteCityService : IFavoriteCityService
{
    private readonly TableClient _tableClient;
    private readonly ILogger<FavoriteCityService> _logger;
    private const string PartitionKey = "favorites";

    /// <summary>
    /// Initializes a new instance of the FavoriteCityService class.
    /// </summary>
    /// <param name="tableServiceClient">The Azure Table service client.</param>
    /// <param name="options">The Azure Storage configuration options.</param>
    /// <param name="logger">The logger instance.</param>
    public FavoriteCityService(
        TableServiceClient tableServiceClient,
        IOptions<AzureStorageOptions> options,
        ILogger<FavoriteCityService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        var storageOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
        
        _tableClient = tableServiceClient.GetTableClient(storageOptions.FavoritesTableName);
        
        // Create table if it doesn't exist
        _ = Task.Run(async () =>
        {
            try
            {
                await _tableClient.CreateIfNotExistsAsync();
                _logger.LogInformation("Favorites table '{TableName}' is ready", storageOptions.FavoritesTableName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create or verify favorites table '{TableName}'", storageOptions.FavoritesTableName);
            }
        });
    }

    /// <summary>
    /// Gets all favorite cities for a user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of favorite cities.</returns>
    public async Task<IEnumerable<FavoriteCity>> GetFavoritesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving all favorite cities from table storage");

            var favorites = new List<FavoriteCity>();
            await foreach (var entity in _tableClient.QueryAsync<FavoriteCityEntity>(
                filter: TableClient.CreateQueryFilter($"PartitionKey eq {PartitionKey}"),
                cancellationToken: cancellationToken))
            {
                favorites.Add(entity.ToFavoriteCity());
            }

            _logger.LogInformation("Retrieved {Count} favorite cities", favorites.Count);
            return favorites.OrderBy(f => f.CityName).ThenBy(f => f.Country);
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to retrieve favorite cities from table storage. Status: {Status}, Error: {Error}", 
                ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException("Failed to retrieve favorite cities", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving favorite cities");
            throw new FavoriteCityServiceException("An unexpected error occurred while retrieving favorite cities", ex);
        }
    }

    /// <summary>
    /// Gets a favorite city by its ID.
    /// </summary>
    /// <param name="id">The ID of the favorite city.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The favorite city if found, null otherwise.</returns>
    public async Task<FavoriteCity?> GetFavoriteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("ID cannot be null or empty", nameof(id));
        }

        try
        {
            _logger.LogDebug("Retrieving favorite city with ID: {Id}", id);

            var response = await _tableClient.GetEntityIfExistsAsync<FavoriteCityEntity>(
                PartitionKey, id, cancellationToken: cancellationToken);

            if (!response.HasValue)
            {
                _logger.LogDebug("Favorite city with ID {Id} not found", id);
                return null;
            }

            var favoriteCity = response.Value.ToFavoriteCity();
            _logger.LogDebug("Retrieved favorite city: {CityName}, {Country}", favoriteCity.CityName, favoriteCity.Country);
            return favoriteCity;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to retrieve favorite city with ID {Id}. Status: {Status}, Error: {Error}", 
                id, ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException($"Failed to retrieve favorite city with ID {id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while retrieving favorite city with ID {Id}", id);
            throw new FavoriteCityServiceException($"An unexpected error occurred while retrieving favorite city with ID {id}", ex);
        }
    }

    /// <summary>
    /// Adds a new favorite city.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created favorite city.</returns>
    public async Task<FavoriteCity> AddFavoriteAsync(string cityName, string country, double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            throw new ArgumentException("City name cannot be null or empty", nameof(cityName));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be null or empty", nameof(country));
        }

        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees");
        }

        try
        {
            _logger.LogDebug("Adding favorite city: {CityName}, {Country}", cityName, country);

            // Check if the city is already a favorite
            if (await IsFavoriteAsync(cityName, country, cancellationToken))
            {
                throw new FavoriteCityServiceException($"City {cityName}, {country} is already in favorites");
            }

            var entity = FavoriteCityEntity.Create(cityName, country, latitude, longitude);
            await _tableClient.AddEntityAsync(entity, cancellationToken);

            var favoriteCity = entity.ToFavoriteCity();
            _logger.LogInformation("Added favorite city: {CityName}, {Country} with ID {Id}", 
                favoriteCity.CityName, favoriteCity.Country, favoriteCity.Id);

            return favoriteCity;
        }
        catch (RequestFailedException ex) when (ex.Status == 409)
        {
            _logger.LogWarning("Favorite city already exists: {CityName}, {Country}", cityName, country);
            throw new FavoriteCityServiceException($"City {cityName}, {country} is already in favorites", ex);
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to add favorite city {CityName}, {Country}. Status: {Status}, Error: {Error}", 
                cityName, country, ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException($"Failed to add favorite city {cityName}, {country}", ex);
        }
        catch (FavoriteCityServiceException)
        {
            throw; // Re-throw our custom exceptions
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding favorite city {CityName}, {Country}", cityName, country);
            throw new FavoriteCityServiceException($"An unexpected error occurred while adding favorite city {cityName}, {country}", ex);
        }
    }

    /// <summary>
    /// Removes a favorite city by its ID.
    /// </summary>
    /// <param name="id">The ID of the favorite city to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the favorite was removed, false if it was not found.</returns>
    public async Task<bool> RemoveFavoriteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("ID cannot be null or empty", nameof(id));
        }

        try
        {
            _logger.LogDebug("Removing favorite city with ID: {Id}", id);

            var response = await _tableClient.DeleteEntityAsync(PartitionKey, id, ETag.All, cancellationToken);
            _logger.LogInformation("Removed favorite city with ID: {Id}", id);
            return true;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            _logger.LogDebug("Favorite city with ID {Id} not found for removal", id);
            return false;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to remove favorite city with ID {Id}. Status: {Status}, Error: {Error}", 
                id, ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException($"Failed to remove favorite city with ID {id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while removing favorite city with ID {Id}", id);
            throw new FavoriteCityServiceException($"An unexpected error occurred while removing favorite city with ID {id}", ex);
        }
    }

    /// <summary>
    /// Updates the last accessed time for a favorite city.
    /// </summary>
    /// <param name="id">The ID of the favorite city.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the favorite was updated, false if it was not found.</returns>
    public async Task<bool> UpdateLastAccessedAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("ID cannot be null or empty", nameof(id));
        }

        try
        {
            _logger.LogDebug("Updating last accessed time for favorite city with ID: {Id}", id);

            var response = await _tableClient.GetEntityIfExistsAsync<FavoriteCityEntity>(
                PartitionKey, id, cancellationToken: cancellationToken);

            if (!response.HasValue)
            {
                _logger.LogDebug("Favorite city with ID {Id} not found for update", id);
                return false;
            }

            var entity = response.Value;
            entity.UpdateLastAccessed();

            await _tableClient.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace, cancellationToken);
            _logger.LogDebug("Updated last accessed time for favorite city with ID: {Id}", id);
            return true;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            _logger.LogDebug("Favorite city with ID {Id} not found for update", id);
            return false;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to update favorite city with ID {Id}. Status: {Status}, Error: {Error}", 
                id, ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException($"Failed to update favorite city with ID {id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating favorite city with ID {Id}", id);
            throw new FavoriteCityServiceException($"An unexpected error occurred while updating favorite city with ID {id}", ex);
        }
    }

    /// <summary>
    /// Checks if a city is already in the favorites list.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the city is already a favorite, false otherwise.</returns>
    public async Task<bool> IsFavoriteAsync(string cityName, string country, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            throw new ArgumentException("City name cannot be null or empty", nameof(cityName));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("Country cannot be null or empty", nameof(country));
        }

        try
        {
            _logger.LogDebug("Checking if city is favorite: {CityName}, {Country}", cityName, country);

            var filter = TableClient.CreateQueryFilter($"PartitionKey eq {PartitionKey} and CityName eq {cityName} and Country eq {country}");
            await foreach (var entity in _tableClient.QueryAsync<FavoriteCityEntity>(filter: filter, cancellationToken: cancellationToken))
            {
                _logger.LogDebug("City {CityName}, {Country} is already a favorite", cityName, country);
                return true;
            }

            _logger.LogDebug("City {CityName}, {Country} is not a favorite", cityName, country);
            return false;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to check if city is favorite: {CityName}, {Country}. Status: {Status}, Error: {Error}", 
                cityName, country, ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException($"Failed to check if city {cityName}, {country} is a favorite", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while checking if city is favorite: {CityName}, {Country}", cityName, country);
            throw new FavoriteCityServiceException($"An unexpected error occurred while checking if city {cityName}, {country} is a favorite", ex);
        }
    }

    /// <summary>
    /// Gets the count of favorite cities.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of favorite cities.</returns>
    public async Task<int> GetFavoritesCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Getting favorites count");

            int count = 0;
            await foreach (var entity in _tableClient.QueryAsync<FavoriteCityEntity>(
                filter: TableClient.CreateQueryFilter($"PartitionKey eq {PartitionKey}"),
                select: new[] { "RowKey" }, // Only select the RowKey to minimize data transfer
                cancellationToken: cancellationToken))
            {
                count++;
            }

            _logger.LogDebug("Favorites count: {Count}", count);
            return count;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Failed to get favorites count. Status: {Status}, Error: {Error}", 
                ex.Status, ex.ErrorCode);
            throw new FavoriteCityServiceException("Failed to get favorites count", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while getting favorites count");
            throw new FavoriteCityServiceException("An unexpected error occurred while getting favorites count", ex);
        }
    }
}