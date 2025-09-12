using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Models;

/// <summary>
/// Represents a favorite city saved by the user.
/// </summary>
public class FavoriteCity
{
    /// <summary>
    /// Gets or sets the unique identifier for the favorite city (used as RowKey in Azure Table Storage).
    /// </summary>
    [Required]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the partition key for Azure Table Storage (typically "favorites").
    /// </summary>
    [Required]
    public required string PartitionKey { get; set; }

    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    public required string CityName { get; set; }

    /// <summary>
    /// Gets or sets the country code (e.g., "US", "GB").
    /// </summary>
    [Required]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 characters")]
    public required string Country { get; set; }

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
    /// Creates a new favorite city with generated ID.
    /// </summary>
    /// <param name="cityName">The name of the city.</param>
    /// <param name="country">The country code.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <returns>A new FavoriteCity instance.</returns>
    public static FavoriteCity Create(string cityName, string country, double latitude, double longitude)
    {
        return new FavoriteCity
        {
            Id = Guid.NewGuid().ToString(),
            PartitionKey = "favorites",
            CityName = cityName,
            Country = country,
            Latitude = latitude,
            Longitude = longitude,
            DateAdded = DateTime.UtcNow,
            LastAccessed = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates the last accessed time to the current UTC time.
    /// </summary>
    public void UpdateLastAccessed()
    {
        LastAccessed = DateTime.UtcNow;
    }
}