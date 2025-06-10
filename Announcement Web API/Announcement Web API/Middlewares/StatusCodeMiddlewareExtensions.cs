namespace Announcement_Web_API.Middlewares;

public static class StatusCodeMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomStatusCodeMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<StatusCodeMiddleware>();
    }
}