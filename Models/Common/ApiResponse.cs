namespace aspdotnet.Models.Common;

/// <summary>
/// Generic API Response Envelope.
/// 
/// 💡 TS / JS Comparison:
/// In TypeScript, you would define this as:
/// interface ApiResponse<T> {
///   success: boolean;
///   message: string;
///   data?: T | null;
///   errors?: string[] | null;
///   statusCode: number;
///   timestamp: string;
/// }
/// 
/// In C#:
/// - `public class ApiResponse<T>`: Declares a class with a generic type parameter `T`.
/// - `{ get; set; }`: C# Auto-implemented properties (provides getter and setter without writing boilerplate backing fields).
/// - `T?`: Nullable generic type (indicates `data` can be null, similar to `T | null` in TypeScript).
/// - `List<string>?`: A resizable list of strings, equivalent to `string[] | null` in TypeScript.
/// - `= string.Empty;`: Default value initializer to avoid null warnings when nullable reference types are enabled.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Static Helper / Factory class for creating ApiResponse instances.
/// 
/// 💡 TS / JS Comparison:
/// In JavaScript/TypeScript, you might have utility functions like `createSuccessResponse(data, msg)`
/// or a class with static methods: `class ApiResponse { static success(data) { ... } }`.
/// In C#, a `static class` cannot be instantiated with `new` and can only hold static methods.
/// </summary>
public static class ApiResponse
{
    /// <summary>
    /// Generates a successful response containing data.
    /// `T? data`: Can accept any type or null.
    /// Default argument `int statusCode = 200`: Just like JS default params `(statusCode = 200)`.
    /// </summary>
    public static ApiResponse<T> Success<T>(T? data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = null,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Overload for successful responses that don't need a return payload (data = null).
    /// `object?` is the C# equivalent of TypeScript's `any | null` or `unknown | null`.
    /// </summary>
    public static ApiResponse<object?> Success(string message = "Success", int statusCode = 200)
    {
        return Success<object?>(null, message, statusCode);
    }

    /// <summary>
    /// Generates a failure response (e.g. 400 Bad Request, 404 Not Found).
    /// `T? data = default`: `default` in C# returns null for reference types, or 0/false for value types.
    /// </summary>
    public static ApiResponse<T> Failure<T>(string message, int statusCode = 400, List<string>? errors = null, T? data = default)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = data,
            Errors = errors,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Non-generic failure helper for quick error messages.
    /// </summary>
    public static ApiResponse<object?> Failure(string message, int statusCode = 400, List<string>? errors = null)
    {
        return Failure<object?>(message, statusCode, errors, null);
    }

    /// <summary>
    /// Generic server error helper (defaults to HTTP 500).
    /// </summary>
    public static ApiResponse<T> Error<T>(string message, int statusCode = 500, List<string>? errors = null)
    {
        return Failure<T>(message, statusCode, errors);
    }

    /// <summary>
    /// Non-generic server error helper (defaults to HTTP 500).
    /// </summary>
    public static ApiResponse<object?> Error(string message, int statusCode = 500, List<string>? errors = null)
    {
        return Failure<object?>(message, statusCode, errors, null);
    }
}
