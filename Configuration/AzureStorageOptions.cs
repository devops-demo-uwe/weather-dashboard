using System.ComponentModel.DataAnnotations;

namespace WeatherDashboard.Configuration;

/// <summary>
/// Configuration options for Azure Table Storage.
/// </summary>
public class AzureStorageOptions
{
    /// <summary>
    /// The configuration section name for Azure Storage settings.
    /// </summary>
    public const string SectionName = "AzureStorage";

    /// <summary>
    /// Gets or sets the Azure Storage connection string.
    /// </summary>
    [Required(ErrorMessage = "Azure Storage connection string is required")]
    public required string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the name of the table for storing favorite cities.
    /// </summary>
    [Required(ErrorMessage = "Favorites table name is required")]
    public string FavoritesTableName { get; set; } = "Favorites";

    /// <summary>
    /// Validates the configuration options.
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise.</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(ConnectionString) &&
               !string.IsNullOrWhiteSpace(FavoritesTableName);
    }

    /// <summary>
    /// Gets the validation errors for the current configuration.
    /// </summary>
    /// <returns>A collection of validation errors.</returns>
    public IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(ConnectionString))
            errors.Add("Connection string is required");

        if (string.IsNullOrWhiteSpace(FavoritesTableName))
            errors.Add("Favorites table name is required");

        return errors;
    }

    /// <summary>
    /// Checks if we're using the local development storage emulator.
    /// </summary>
    /// <returns>True if using development storage, false otherwise.</returns>
    public bool IsUsingDevelopmentStorage()
    {
        return ConnectionString.Contains("UseDevelopmentStorage=true", StringComparison.OrdinalIgnoreCase);
    }
}