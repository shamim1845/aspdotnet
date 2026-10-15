using System.ComponentModel.DataAnnotations;
using aspdotnet.Models.Common;
using aspdotnet.Models.DTOs;
using aspdotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet.Controllers;

/// <summary>
/// Controller exposing REST API endpoints for Product management.
/// 
/// 💡 TS / NestJS Comparison:
/// - `@Controller('api/products')` in NestJS is equivalent to `[ApiController]` and `[Route("api/[controller]")]`.
/// - Constructor Dependency Injection: Injected services are automatically provided by ASP.NET's DI container.
/// </summary>
public class ProductsController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// GET /api/products
    /// 💡 [FromQuery] is equivalent to NestJS `@Query('category')` or Express `req.query.category`.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDto>>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var products = await _productService.GetAllAsync(category, minPrice, maxPrice);
        return SuccessResponse(products, "Products retrieved successfully.");
    }

    /// <summary>
    /// GET /api/products/{id}
    /// 💡 [FromRoute] is equivalent to NestJS `@Param('id')` or Express `req.params.id`.
    /// 💡 [RegularExpression] validates that the route parameter is a valid 24-char MongoDB ObjectId.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetById(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFoundResponse($"Product with ID '{id}' was not found.");
        }
        return SuccessResponse(product, "Product retrieved successfully.");
    }

    /// <summary>
    /// POST /api/products
    /// 💡 [FromBody] is equivalent to NestJS `@Body()` or Express `req.body`.
    /// Model validation occurs automatically before the method body runs.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> Create([FromBody] CreateProductDto dto)
    {
        var product = await _productService.CreateAsync(dto);
        return CreatedResponse(nameof(GetById), new { id = product.Id }, product, "Product created successfully.");
    }

    /// <summary>
    /// PATCH /api/products/{id}
    /// Performs partial update on the product document.
    /// </summary>
    [HttpPatch("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> Update(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id,
        [FromBody] UpdateProductDto dto)
    {
        var updated = await _productService.UpdateAsync(id, dto);
        if (updated == null)
        {
            return NotFoundResponse($"Product with ID '{id}' was not found.");
        }
        return SuccessResponse(updated, "Product updated successfully.");
    }

    /// <summary>
    /// DELETE /api/products/{id}
    /// Removes a product by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object?>>> Delete(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id)
    {
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFoundResponse($"Product with ID '{id}' was not found.");
        }
        return SuccessResponse("Product deleted successfully.");
    }
}
