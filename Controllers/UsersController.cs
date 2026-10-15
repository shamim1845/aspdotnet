using System.ComponentModel.DataAnnotations;
using aspdotnet.Models.Common;
using aspdotnet.Models.DTOs;
using aspdotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet.Controllers;

/// <summary>
/// Controller exposing REST API endpoints for User management.
/// </summary>
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// GET /api/users
    /// Fetches all registered users.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDto>>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return SuccessResponse(users, "Users retrieved successfully.");
    }

    /// <summary>
    /// GET /api/users/{id}
    /// Fetches a specific user by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetById(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFoundResponse($"User with ID '{id}' was not found.");
        }
        return SuccessResponse(user, "User retrieved successfully.");
    }

    /// <summary>
    /// POST /api/users
    /// Creates a new user with validation on body input.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> Create([FromBody] CreateUserDto dto)
    {
        var user = await _userService.CreateAsync(dto);
        return CreatedResponse(nameof(GetById), new { id = user.Id }, user, "User created successfully.");
    }

    /// <summary>
    /// DELETE /api/users/{id}
    /// Deletes a user by ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object?>>> Delete(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFoundResponse($"User with ID '{id}' was not found.");
        }
        return SuccessResponse("User deleted successfully.");
    }
}
