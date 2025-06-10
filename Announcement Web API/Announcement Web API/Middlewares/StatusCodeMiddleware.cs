namespace Announcement_Web_API.Middlewares;

public class StatusCodeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StatusCodeMiddleware> _logger;

    public StatusCodeMiddleware(RequestDelegate next,  ILogger<StatusCodeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
        {
            _logger.LogWarning("404 Not Found for path: {Path}", context.Request.Path);
            
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"status\":\"Error\",\"message\":\"Resource not found\"}");
        }
    }
}