namespace WeatherDashboard.Services;

/// <summary>
/// Exception thrown when the weather service encounters an error.
/// </summary>
public class WeatherServiceException : Exception
{
    /// <summary>
    /// Gets the error code from the weather service.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Gets the HTTP status code if the error was from an HTTP request.
    /// </summary>
    public int? HttpStatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the WeatherServiceException class.
    /// </summary>
    public WeatherServiceException() : base("An error occurred while accessing the weather service.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the WeatherServiceException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public WeatherServiceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the WeatherServiceException class with a specified error message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public WeatherServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the WeatherServiceException class with detailed error information.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="errorCode">The error code from the weather service.</param>
    /// <param name="httpStatusCode">The HTTP status code if applicable.</param>
    public WeatherServiceException(string message, string? errorCode = null, int? httpStatusCode = null) : base(message)
    {
        ErrorCode = errorCode;
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Initializes a new instance of the WeatherServiceException class with detailed error information and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="errorCode">The error code from the weather service.</param>
    /// <param name="httpStatusCode">The HTTP status code if applicable.</param>
    public WeatherServiceException(string message, Exception innerException, string? errorCode = null, int? httpStatusCode = null) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Creates a WeatherServiceException for when a city is not found.
    /// </summary>
    /// <param name="cityName">The name of the city that was not found.</param>
    /// <returns>A WeatherServiceException with appropriate error details.</returns>
    public static WeatherServiceException CityNotFound(string cityName)
    {
        return new WeatherServiceException($"City '{cityName}' was not found.", "CITY_NOT_FOUND", 404);
    }

    /// <summary>
    /// Creates a WeatherServiceException for API authentication failures.
    /// </summary>
    /// <returns>A WeatherServiceException with appropriate error details.</returns>
    public static WeatherServiceException InvalidApiKey()
    {
        return new WeatherServiceException("Invalid API key. Please check your OpenWeatherMap API configuration.", "INVALID_API_KEY", 401);
    }

    /// <summary>
    /// Creates a WeatherServiceException for API rate limiting.
    /// </summary>
    /// <returns>A WeatherServiceException with appropriate error details.</returns>
    public static WeatherServiceException RateLimitExceeded()
    {
        return new WeatherServiceException("API rate limit exceeded. Please try again later.", "RATE_LIMIT_EXCEEDED", 429);
    }

    /// <summary>
    /// Creates a WeatherServiceException for network connectivity issues.
    /// </summary>
    /// <param name="innerException">The underlying network exception.</param>
    /// <returns>A WeatherServiceException with appropriate error details.</returns>
    public static WeatherServiceException NetworkError(Exception innerException)
    {
        return new WeatherServiceException("Unable to connect to the weather service. Please check your internet connection.", innerException, "NETWORK_ERROR");
    }

    /// <summary>
    /// Creates a WeatherServiceException for service unavailability.
    /// </summary>
    /// <returns>A WeatherServiceException with appropriate error details.</returns>
    public static WeatherServiceException ServiceUnavailable()
    {
        return new WeatherServiceException("The weather service is currently unavailable. Please try again later.", "SERVICE_UNAVAILABLE", 503);
    }
}