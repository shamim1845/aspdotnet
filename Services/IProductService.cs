using aspdotnet.Models.DTOs;

namespace aspdotnet.Services;

/// <summary>
/// Service contract for Product business operations.
/// 
/// 💡 TS / NestJS Comparison:
/// - In TypeScript, interfaces are erased during build and cannot be used as DI tokens directly (NestJS often uses strings/symbols or abstract classes).
/// - In C#, interfaces are first-class runtime types used as Dependency Injection tokens (`AddScoped<IProductService, ProductService>()`).
/// - `Task<T>`: Identical to JavaScript's `Promise<T>`. Every async method returns a `Task`.
/// - `IEnumerable<T>`: An abstract read-only collection/sequence, similar to `T[]` or `Iterable<T>` in TypeScript.
/// - `ProductResponseDto?`: The `?` indicates that the Task resolves to either the DTO or `null` (like `Promise<Product | null>`).
/// </summary>
public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null);
    Task<ProductResponseDto?> GetByIdAsync(string id);
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
    Task<ProductResponseDto?> UpdateAsync(string id, UpdateProductDto dto);
    Task<bool> DeleteAsync(string id);
}
