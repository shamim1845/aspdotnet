using System.ComponentModel.DataAnnotations;

namespace aspdotnet.Models.DTOs;

public record CreateProductDto(
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 2)]
    string Name,

    [Required]
    string Description,

    [Range(0.01, 1_000_000, ErrorMessage = "Price must be greater than 0")]
    decimal Price,

    [Required]
    string Category,

    [Range(0, 100_000, ErrorMessage = "Stock cannot be negative")]
    int Stock,

    string? ImageUrl
);

public record UpdateProductDto(
    [StringLength(100, MinimumLength = 2)]
    string? Name,

    string? Description,

    [Range(0.01, 1_000_000)]
    decimal? Price,

    string? Category,

    [Range(0, 100_000)]
    int? Stock,

    string? ImageUrl,

    bool? IsActive
);

public record ProductResponseDto(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    int Stock,
    // string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
