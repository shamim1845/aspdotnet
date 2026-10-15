using aspdotnet.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet.Controllers;

/// <summary>
/// Abstract Base Controller providing standardized API response methods for all endpoints.
/// 
/// 💡 TS / NestJS Comparison:
/// In NestJS, you might write an `@UseInterceptors(TransformInterceptor)` or custom base service.
/// In ASP.NET Core:
/// - `[ApiController]`: Automatically enforces model validation, binding conventions, and problem details.
/// - `[Route("api/[controller]")`: Token `[controller]` is automatically replaced by the class prefix (e.g., `ProductsController` -> `/api/products`).
/// - `ControllerBase`: Standard ASP.NET Core base class providing HTTP helper methods (`Ok()`, `NotFound()`, `BadRequest()`).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Returns HTTP 200 OK with ApiResponse payload.
    /// </summary>
    protected OkObjectResult SuccessResponse<T>(T? data, string message = "Success")
    {
        return Ok(ApiResponse.Success(data, message, StatusCodes.Status200OK));
    }

    /// <summary>
    /// Returns HTTP 200 OK without a data payload.
    /// </summary>
    protected OkObjectResult SuccessResponse(string message = "Success")
    {
        return Ok(ApiResponse.Success<object?>(null, message, StatusCodes.Status200OK));
    }

    /// <summary>
    /// Returns a success response with a custom HTTP status code (e.g. 200, 202).
    /// </summary>
    protected ObjectResult SuccessResponse<T>(T? data, string message, int statusCode)
    {
        return StatusCode(statusCode, ApiResponse.Success(data, message, statusCode));
    }

    /// <summary>
    /// Returns HTTP 201 Created with a `Location` header pointing to the new resource.
    /// In Express: `res.status(201).location('/api/products/' + id).json(...)`.
    /// </summary>
    protected CreatedAtActionResult CreatedResponse<T>(string actionName, object? routeValues, T data, string message = "Resource created successfully")
    {
        return CreatedAtAction(actionName, routeValues, ApiResponse.Success(data, message, StatusCodes.Status201Created));
    }

    /// <summary>
    /// Returns HTTP 404 Not Found formatted as ApiResponse.
    /// </summary>
    protected NotFoundObjectResult NotFoundResponse(string message = "Resource not found")
    {
        return NotFound(ApiResponse.Failure(message, StatusCodes.Status404NotFound));
    }

    /// <summary>
    /// Returns HTTP 400 Bad Request formatted as ApiResponse with optional error list.
    /// </summary>
    protected BadRequestObjectResult BadRequestResponse(string message = "Bad request", List<string>? errors = null)
    {
        return BadRequest(ApiResponse.Failure(message, StatusCodes.Status400BadRequest, errors));
    }

    /// <summary>
    /// Returns a generic error response with the specified HTTP status code.
    /// </summary>
    protected ObjectResult ErrorResponse(string message = "An internal server error occurred", int statusCode = StatusCodes.Status500InternalServerError, List<string>? errors = null)
    {
        return StatusCode(statusCode, ApiResponse.Error(message, statusCode, errors));
    }
}
