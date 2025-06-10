using Microsoft.AspNetCore.Diagnostics;

namespace Announcement_Web_API.Exceptions;

public class GlobalExceptionHandler: IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, response) = exception switch
        {
            NotFoundException notFoundException => HandleNotFoundException(notFoundException),
            _ => HandleUnknownException(exception)
        };
        
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private (int statusCode, object response) HandleNotFoundException(NotFoundException exception)
    {
        _logger.LogWarning("Not found exception: {Message}", exception.Message);
        
        return (StatusCodes.Status404NotFound, new
            {
              Status = "Error",
              Message = exception.Message,
              Details = (object?)null
            });
    }

    private (int statusCode, object response) HandleUnknownException(Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception occurred");
        
        return (StatusCodes.Status500InternalServerError, new
        {
            Status = "Error",
            Message = "An unexpected error occurred",
            Details = (object?)null
        });
    }
}