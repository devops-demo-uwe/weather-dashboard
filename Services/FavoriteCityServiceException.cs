namespace WeatherDashboard.Services;

/// <summary>
/// Exception thrown when an error occurs in the FavoriteCityService.
/// </summary>
public class FavoriteCityServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the FavoriteCityServiceException class.
    /// </summary>
    public FavoriteCityServiceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the FavoriteCityServiceException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public FavoriteCityServiceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the FavoriteCityServiceException class with a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public FavoriteCityServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}