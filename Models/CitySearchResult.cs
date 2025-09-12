using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Models;

/// <summary>
/// Represents a city search result from the weather API.
/// </summary>
public class CitySearchResult
{
    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the country code (e.g., "US", "GB").
    /// </summary>
    [Required]
    public required string Country { get; set; }

    /// <summary>
    /// Gets or sets the state or region (optional).
    /// </summary>
    public string? State { get; set; }

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
    /// Gets the display name combining city, state (if available), and country.
    /// </summary>
    public string DisplayName
    {
        get
        {
            if (!string.IsNullOrEmpty(State))
            {
                return $"{Name}, {State}, {Country}";
            }
            return $"{Name}, {Country}";
        }
    }

    /// <summary>
    /// Gets the search query representation for this city.
    /// </summary>
    public string SearchQuery => $"{Name},{Country}";

    /// <summary>
    /// Creates a new city search result.
    /// </summary>
    /// <param name="name">The city name.</param>
    /// <param name="country">The country code.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="state">The state or region (optional).</param>
    /// <returns>A new CitySearchResult instance.</returns>
    public static CitySearchResult Create(string name, string country, double latitude, double longitude, string? state = null)
    {
        return new CitySearchResult
        {
            Name = name,
            Country = country,
            State = state,
            Latitude = latitude,
            Longitude = longitude
        };
    }
}