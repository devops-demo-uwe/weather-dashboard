using System.Text.Json.Serialization;

namespace WeatherDashboard.Models;

/// <summary>
/// Represents the API response from OpenWeatherMap for current weather data.
/// This class is used for JSON deserialization and should match the OpenWeatherMap API structure.
/// </summary>
public class WeatherApiResponse
{
    /// <summary>
    /// Gets or sets the geographical coordinates.
    /// </summary>
    [JsonPropertyName("coord")]
    public Coord? Coord { get; set; }

    /// <summary>
    /// Gets or sets the weather conditions array.
    /// </summary>
    [JsonPropertyName("weather")]
    public Weather[]? Weather { get; set; }

    /// <summary>
    /// Gets or sets the data source information.
    /// </summary>
    [JsonPropertyName("base")]
    public string? Base { get; set; }

    /// <summary>
    /// Gets or sets the main weather data.
    /// </summary>
    [JsonPropertyName("main")]
    public Main? Main { get; set; }

    /// <summary>
    /// Gets or sets the visibility in meters.
    /// </summary>
    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    /// <summary>
    /// Gets or sets the wind information.
    /// </summary>
    [JsonPropertyName("wind")]
    public Wind? Wind { get; set; }

    /// <summary>
    /// Gets or sets the cloudiness information.
    /// </summary>
    [JsonPropertyName("clouds")]
    public Clouds? Clouds { get; set; }

    /// <summary>
    /// Gets or sets the data calculation timestamp.
    /// </summary>
    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    /// <summary>
    /// Gets or sets the system information including sunrise and sunset.
    /// </summary>
    [JsonPropertyName("sys")]
    public Sys? Sys { get; set; }

    /// <summary>
    /// Gets or sets the timezone offset in seconds.
    /// </summary>
    [JsonPropertyName("timezone")]
    public int Timezone { get; set; }

    /// <summary>
    /// Gets or sets the city ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the city name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the response code.
    /// </summary>
    [JsonPropertyName("cod")]
    public int Cod { get; set; }
}

/// <summary>
/// Represents geographical coordinates in the API response.
/// </summary>
public class Coord
{
    /// <summary>
    /// Gets or sets the longitude.
    /// </summary>
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    /// <summary>
    /// Gets or sets the latitude.
    /// </summary>
    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}

/// <summary>
/// Represents weather condition information in the API response.
/// </summary>
public class Weather
{
    /// <summary>
    /// Gets or sets the weather condition ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the main weather condition.
    /// </summary>
    [JsonPropertyName("main")]
    public string? Main { get; set; }

    /// <summary>
    /// Gets or sets the weather description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the weather icon code.
    /// </summary>
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}

/// <summary>
/// Represents main weather data in the API response.
/// </summary>
public class Main
{
    /// <summary>
    /// Gets or sets the temperature. Units depend on the API request:
    /// - Default: Kelvin
    /// - Metric: Celsius  
    /// - Imperial: Fahrenheit
    /// </summary>
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    /// <summary>
    /// Gets or sets the "feels like" temperature. Units depend on the API request:
    /// - Default: Kelvin
    /// - Metric: Celsius
    /// - Imperial: Fahrenheit
    /// </summary>
    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    /// <summary>
    /// Gets or sets the minimum temperature. Units depend on the API request:
    /// - Default: Kelvin
    /// - Metric: Celsius
    /// - Imperial: Fahrenheit
    /// </summary>
    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    /// <summary>
    /// Gets or sets the maximum temperature. Units depend on the API request:
    /// - Default: Kelvin
    /// - Metric: Celsius
    /// - Imperial: Fahrenheit
    /// </summary>
    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    /// <summary>
    /// Gets or sets the atmospheric pressure in hPa.
    /// </summary>
    [JsonPropertyName("pressure")]
    public int Pressure { get; set; }

    /// <summary>
    /// Gets or sets the humidity percentage.
    /// </summary>
    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }

    /// <summary>
    /// Gets or sets the sea level pressure in hPa.
    /// </summary>
    [JsonPropertyName("sea_level")]
    public int? SeaLevel { get; set; }

    /// <summary>
    /// Gets or sets the ground level pressure in hPa.
    /// </summary>
    [JsonPropertyName("grnd_level")]
    public int? GrndLevel { get; set; }
}

/// <summary>
/// Represents wind information in the API response.
/// </summary>
public class Wind
{
    /// <summary>
    /// Gets or sets the wind speed in meters per second.
    /// </summary>
    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    /// <summary>
    /// Gets or sets the wind direction in degrees.
    /// </summary>
    [JsonPropertyName("deg")]
    public int? Deg { get; set; }

    /// <summary>
    /// Gets or sets the wind gust speed in meters per second.
    /// </summary>
    [JsonPropertyName("gust")]
    public double? Gust { get; set; }
}

/// <summary>
/// Represents cloudiness information in the API response.
/// </summary>
public class Clouds
{
    /// <summary>
    /// Gets or sets the cloudiness percentage.
    /// </summary>
    [JsonPropertyName("all")]
    public int All { get; set; }
}

/// <summary>
/// Represents system information in the API response.
/// </summary>
public class Sys
{
    /// <summary>
    /// Gets or sets the system type.
    /// </summary>
    [JsonPropertyName("type")]
    public int? Type { get; set; }

    /// <summary>
    /// Gets or sets the system ID.
    /// </summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the country code.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets the sunrise timestamp.
    /// </summary>
    [JsonPropertyName("sunrise")]
    public long Sunrise { get; set; }

    /// <summary>
    /// Gets or sets the sunset timestamp.
    /// </summary>
    [JsonPropertyName("sunset")]
    public long Sunset { get; set; }
}