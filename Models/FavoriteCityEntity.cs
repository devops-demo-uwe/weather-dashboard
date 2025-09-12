using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Models;

/// <summary>
/// Represents a favorite city entity for Azure Table Storage.
/// </summary>
public class FavoriteCityEntity : ITableEntity
{
    /// <summary>
    /// Gets or sets the partition key (always "favorites" for favorite cities).
    /// </summary>
    public string PartitionKey { get; set; } = "favorites";

    /// <summary>
    /// Gets or sets the row key (unique identifier for the favorite city).
    /// </summary>
    public string RowKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp for the entity.
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the ETag for optimistic concurrency.
    /// </summary>
    public ETag ETag { get; set; }

    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    public string CityName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country code (e.g., "US", "GB").
    /// </summary>
    [Required]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 characters")]
    public string Country { get; set; } = string.Empty;

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
    /// Gets or sets the date and time when the city was added to favorites.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the favorite was last accessed.
    /// </summary>
    public DateTime LastAccessed { get; set; }

    /// <summary>
    /// Gets the display name combining city and country.
    /// </summary>
    public string DisplayName => $"{CityName}, {Country}";

    /// <summary>
    /// Gets the unique identifier (same as RowKey).
    /// </summary>
    public string Id => RowKey;

    /// <summary>
    /// Initializes a new instance of the FavoriteCityEntity class.
    /// </summary>
    public FavoriteCityEntity()
    {
    }

    /// <summary>
    /// Initializes a new instance of the FavoriteCityEntity class with specified values.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    public FavoriteCityEntity(string cityName, string country, double latitude, double longitude)
    {
        RowKey = Guid.NewGuid().ToString();
        CityName = cityName;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        DateAdded = DateTime.UtcNow;
        LastAccessed = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new favorite city entity with generated ID.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <returns>A new FavoriteCityEntity instance.</returns>
    public static FavoriteCityEntity Create(string cityName, string country, double latitude, double longitude)
    {
        return new FavoriteCityEntity(cityName, country, latitude, longitude);
    }

    /// <summary>
    /// Updates the last accessed time to the current UTC time.
    /// </summary>
    public void UpdateLastAccessed()
    {
        LastAccessed = DateTime.UtcNow;
    }

    /// <summary>
    /// Converts this entity to a FavoriteCity model.
    /// </summary>
    /// <returns>A FavoriteCity instance.</returns>
    public FavoriteCity ToFavoriteCity()
    {
        return new FavoriteCity
        {
            Id = RowKey,
            PartitionKey = PartitionKey,
            CityName = CityName,
            Country = Country,
            Latitude = Latitude,
            Longitude = Longitude,
            DateAdded = DateAdded,
            LastAccessed = LastAccessed
        };
    }

    /// <summary>
    /// Creates a FavoriteCityEntity from a FavoriteCity model.
    /// </summary>
    /// <param name="favoriteCity">The FavoriteCity to convert.</param>
    /// <returns>A new FavoriteCityEntity instance.</returns>
    public static FavoriteCityEntity FromFavoriteCity(FavoriteCity favoriteCity)
    {
        return new FavoriteCityEntity
        {
            RowKey = favoriteCity.Id,
            PartitionKey = favoriteCity.PartitionKey,
            CityName = favoriteCity.CityName,
            Country = favoriteCity.Country,
            Latitude = favoriteCity.Latitude,
            Longitude = favoriteCity.Longitude,
            DateAdded = favoriteCity.DateAdded,
            LastAccessed = favoriteCity.LastAccessed
        };
    }
}