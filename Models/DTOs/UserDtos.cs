using System.ComponentModel.DataAnnotations;

namespace aspdotnet.Models.DTOs;

public record CreateUserDto(
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2)]
    string Name,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email,

    string? Role
);

public record UserResponseDto(
    string Id,
    string Name,
    string Email,
    string Role,
    DateTime CreatedAt
);
