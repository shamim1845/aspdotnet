using System.ComponentModel.DataAnnotations;

namespace aspdotnet.Models.DTOs;

/// <summary>
/// DTO for creating a product.
/// 
/// 💡 TS / NestJS Comparison:
/// In NestJS, this would be a class with `class-validator` decorators:
/// export class CreateProductDto {
///   @IsNotEmpty() @MinLength(2) name: string;
///   @IsNotEmpty() description: string;
///   @Min(0.01) price: number;
/// }
/// 
/// In C#:
/// - `record`: A lightweight, immutable type with value-based equality (ideal for DTOs).
/// - Positional syntax `CreateProductDto(...)`: Generates constructor + read-only properties automatically.
/// - Data Annotations (`[Required]`, `[Range]`): Built-in validation rules checked automatically by ASP.NET Core.
/// </summary>
public record CreateProductDto(
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    string Name,

    [Required(ErrorMessage = "Description is required")]
    string Description,

    [Range(0.01, 1_000_000, ErrorMessage = "Price must be greater than 0")]
    decimal Price,

    [Required(ErrorMessage = "Category is required")]
    string Category,

    [Range(0, 100_000, ErrorMessage = "Stock cannot be negative")]
    int Stock,

    string? ImageUrl
);

/// <summary>
/// DTO for updating a product (partial update / PATCH).
/// 
/// 💡 TS / NestJS Comparison:
/// In TypeScript: `Partial<CreateProductDto>` or NestJS `PartialType(CreateProductDto)`.
/// In C#: Properties are nullable (`string?`, `decimal?`) so omitting them sends `null`.
/// </summary>
public record UpdateProductDto(
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    string? Name,

    string? Description,

    [Range(0.01, 1_000_000, ErrorMessage = "Price must be greater than 0")]
    decimal? Price,

    string? Category,

    [Range(0, 100_000, ErrorMessage = "Stock cannot be negative")]
    int? Stock,

    string? ImageUrl,

    bool? IsActive
);

/// <summary>
/// DTO for returning product data to the client.
/// Ensures internal database details are not directly leaked.
/// </summary>
public record ProductResponseDto(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    int Stock,
    string? ImageUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
