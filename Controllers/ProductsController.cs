using System.ComponentModel.DataAnnotations;
using aspdotnet.Models.DTOs;
using aspdotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll(
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var products = await _productService.GetAllAsync(category, minPrice, maxPrice);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID '{id}' was not found." });
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create([FromBody] CreateProductDto dto)
    {
        var product = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<ProductResponseDto>> Update(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id,
        [FromBody] UpdateProductDto dto)
    {
        var updated = await _productService.UpdateAsync(id, dto);
        if (updated == null)
        {
            return NotFound(new { message = $"Product with ID '{id}' was not found." });
        }
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute]
        [RegularExpression("^[a-fA-F0-9]{24}$", ErrorMessage = "The ID must be a valid 24-character hexadecimal string.")]
        string id)
    {
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Product with ID '{id}' was not found." });
        }
        return NoContent();
    }
}
