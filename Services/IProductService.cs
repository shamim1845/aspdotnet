using aspdotnet.Models.DTOs;

namespace aspdotnet.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null);
    Task<ProductResponseDto?> GetByIdAsync(string id);
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
    Task<ProductResponseDto?> UpdateAsync(string id, UpdateProductDto dto);
    Task<bool> DeleteAsync(string id);
}
