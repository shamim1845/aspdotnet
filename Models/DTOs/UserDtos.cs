using System.ComponentModel.DataAnnotations;

namespace aspdotnet.Models.DTOs;

/// <summary>
/// DTO for user registration.
/// 
/// 💡 TS / NestJS Comparison:
/// Similar to a TypeScript DTO class with `@IsEmail()`, `@IsNotEmpty()` from `class-validator`.
/// </summary>
public record CreateUserDto(
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    string Name,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email,

    string? Role
);

/// <summary>
/// DTO for returning user information to the client (omits sensitive internal fields).
/// </summary>
public record UserResponseDto(
    string Id,
    string Name,
    string Email,
    string Role,
    DateTime CreatedAt
);
