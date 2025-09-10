using System.Net;
using System.Text.Json;

namespace ComplianceAnalytics.API.Middleware;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var start = DateTime.UtcNow;
            await _next(context); // continue pipeline
            var duration = DateTime.UtcNow - start;

            _logger.LogInformation(
                "Request {method} {url} => {statusCode} in {duration}ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode,
                duration.TotalMilliseconds
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                error = "Something went wrong.",
                details = ex.Message
            });

            await context.Response.WriteAsync(result);
        }
    }
}
