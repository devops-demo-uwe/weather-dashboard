namespace WeatherDashboard.Models;

/// <summary>
/// Extension methods for converting between different weather data models.
/// </summary>
public static class WeatherModelExtensions
{
    /// <summary>
    /// Converts a WeatherApiResponse to a CurrentWeather model.
    /// </summary>
    /// <param name="apiResponse">The API response from OpenWeatherMap.</param>
    /// <returns>A CurrentWeather model with converted data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when apiResponse is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when required data is missing from the API response.</exception>
    public static CurrentWeather ToCurrentWeather(this WeatherApiResponse apiResponse)
    {
        ArgumentNullException.ThrowIfNull(apiResponse);

        if (apiResponse.Main == null)
            throw new InvalidOperationException("Weather API response is missing main weather data.");

        if (apiResponse.Weather == null || apiResponse.Weather.Length == 0)
            throw new InvalidOperationException("Weather API response is missing weather condition data.");

        if (string.IsNullOrEmpty(apiResponse.Name))
            throw new InvalidOperationException("Weather API response is missing city name.");

        var weather = apiResponse.Weather[0];
        var main = apiResponse.Main;
        var sys = apiResponse.Sys;
        var wind = apiResponse.Wind;

        return new CurrentWeather
        {
            CityName = apiResponse.Name,
            Country = sys?.Country ?? "Unknown",
            TemperatureCelsius = KelvinToCelsius(main.Temp),
            FeelsLikeCelsius = KelvinToCelsius(main.FeelsLike),
            Condition = new WeatherCondition
            {
                Id = weather.Id,
                Main = weather.Main ?? "Unknown",
                Description = weather.Description ?? "Unknown",
                IconCode = weather.Icon ?? "01d"
            },
            Humidity = main.Humidity,
            Pressure = main.Pressure,
            WindSpeed = wind?.Speed ?? 0,
            WindDirection = wind?.Deg ?? 0,
            Visibility = apiResponse.Visibility,
            UvIndex = 0, // UV Index is not provided in current weather API, would need separate call
            LastUpdated = DateTimeOffset.FromUnixTimeSeconds(apiResponse.Dt).DateTime,
            Sunrise = sys != null ? DateTimeOffset.FromUnixTimeSeconds(sys.Sunrise).DateTime : DateTime.MinValue,
            Sunset = sys != null ? DateTimeOffset.FromUnixTimeSeconds(sys.Sunset).DateTime : DateTime.MinValue
        };
    }

    /// <summary>
    /// Converts temperature from Kelvin to Celsius.
    /// </summary>
    /// <param name="kelvin">Temperature in Kelvin.</param>
    /// <returns>Temperature in Celsius.</returns>
    public static double KelvinToCelsius(double kelvin)
    {
        return Math.Round(kelvin - 273.15, 1);
    }

    /// <summary>
    /// Converts temperature from Celsius to Kelvin.
    /// </summary>
    /// <param name="celsius">Temperature in Celsius.</param>
    /// <returns>Temperature in Kelvin.</returns>
    public static double CelsiusToKelvin(double celsius)
    {
        return celsius + 273.15;
    }

    /// <summary>
    /// Converts temperature from Celsius to Fahrenheit.
    /// </summary>
    /// <param name="celsius">Temperature in Celsius.</param>
    /// <returns>Temperature in Fahrenheit.</returns>
    public static double CelsiusToFahrenheit(double celsius)
    {
        return Math.Round((celsius * 9.0 / 5.0) + 32.0, 1);
    }

    /// <summary>
    /// Converts temperature from Fahrenheit to Celsius.
    /// </summary>
    /// <param name="fahrenheit">Temperature in Fahrenheit.</param>
    /// <returns>Temperature in Celsius.</returns>
    public static double FahrenheitToCelsius(double fahrenheit)
    {
        return Math.Round((fahrenheit - 32.0) * 5.0 / 9.0, 1);
    }

    /// <summary>
    /// Converts wind speed from meters per second to kilometers per hour.
    /// </summary>
    /// <param name="mps">Wind speed in meters per second.</param>
    /// <returns>Wind speed in kilometers per hour.</returns>
    public static double MpsToKmh(double mps)
    {
        return Math.Round(mps * 3.6, 1);
    }

    /// <summary>
    /// Converts wind speed from meters per second to miles per hour.
    /// </summary>
    /// <param name="mps">Wind speed in meters per second.</param>
    /// <returns>Wind speed in miles per hour.</returns>
    public static double MpsToMph(double mps)
    {
        return Math.Round(mps * 2.237, 1);
    }

    /// <summary>
    /// Converts visibility from meters to kilometers.
    /// </summary>
    /// <param name="meters">Visibility in meters.</param>
    /// <returns>Visibility in kilometers.</returns>
    public static double MetersToKilometers(double meters)
    {
        return Math.Round(meters / 1000.0, 1);
    }

    /// <summary>
    /// Converts visibility from meters to miles.
    /// </summary>
    /// <param name="meters">Visibility in meters.</param>
    /// <returns>Visibility in miles.</returns>
    public static double MetersToMiles(double meters)
    {
        return Math.Round(meters / 1609.344, 1);
    }

    /// <summary>
    /// Gets a user-friendly description of UV Index level.
    /// </summary>
    /// <param name="uvIndex">The UV Index value.</param>
    /// <returns>A descriptive string for the UV Index level.</returns>
    public static string GetUvIndexDescription(double uvIndex)
    {
        return uvIndex switch
        {
            <= 2 => "Low",
            <= 5 => "Moderate",
            <= 7 => "High",
            <= 10 => "Very High",
            _ => "Extreme"
        };
    }

    /// <summary>
    /// Gets a user-friendly description of humidity level.
    /// </summary>
    /// <param name="humidity">The humidity percentage.</param>
    /// <returns>A descriptive string for the humidity level.</returns>
    public static string GetHumidityDescription(int humidity)
    {
        return humidity switch
        {
            < 30 => "Low",
            <= 60 => "Comfortable",
            <= 80 => "High",
            _ => "Very High"
        };
    }

    /// <summary>
    /// Gets a user-friendly description of wind speed.
    /// </summary>
    /// <param name="windSpeedMps">Wind speed in meters per second.</param>
    /// <returns>A descriptive string for the wind speed.</returns>
    public static string GetWindSpeedDescription(double windSpeedMps)
    {
        return windSpeedMps switch
        {
            < 1 => "Calm",
            < 4 => "Light Breeze",
            < 7 => "Gentle Breeze",
            < 11 => "Moderate Breeze",
            < 17 => "Fresh Breeze",
            < 22 => "Strong Breeze",
            < 28 => "Near Gale",
            < 34 => "Gale",
            < 41 => "Strong Gale",
            _ => "Storm"
        };
    }
}