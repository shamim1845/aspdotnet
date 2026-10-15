using System.Net;
using System.Text.Json;
using aspdotnet.Models.Common;

namespace aspdotnet.Middlewares;

/// <summary>
/// Global Exception Handling Middleware.
/// 
/// 💡 TS / Express / NestJS Comparison:
/// - In Express, this is the error-handling middleware: `app.use((err, req, res, next) => { ... })`.
/// - In NestJS, this is an `@Catch() ExceptionFilter`.
/// - In ASP.NET Core:
///   - `RequestDelegate _next`: Corresponds directly to `next` function in Express middleware.
///   - `HttpContext context`: Holds both the `HttpRequest` (req) and `HttpResponse` (res).
///   - `ILogger<GlobalExceptionMiddleware>`: Built-in structured logging system.
///   - `IHostEnvironment _env`: Checks environment, equivalent to checking `process.env.NODE_ENV === 'development'`.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Executes the middleware in the ASP.NET Core HTTP request pipeline.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next middleware in the pipeline (equivalent to `await next()` in Koa or `next()` in Express)
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while processing HTTP request to {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Serializes and writes a standardized 500 ApiResponse to the HTTP response stream.
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // In Development, expose the error message and stack trace for easy debugging.
        // In Production, keep it null to avoid leaking sensitive internal details.
        var errors = _env.IsDevelopment()
            ? new List<string> { exception.Message, exception.StackTrace ?? string.Empty }
            : null;

        var response = ApiResponse.Error(
            message: "An unexpected error occurred. Please try again later.",
            statusCode: (int)HttpStatusCode.InternalServerError,
            errors: errors
        );

        // Serialize to camelCase JSON (standard in JavaScript frontends)
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
