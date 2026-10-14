using aspdotnet.Data;
using aspdotnet.Models.DTOs;
using aspdotnet.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace aspdotnet.Services;

public class ProductService : IProductService
{
    private readonly MongoDbContext _context;

    public ProductService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(
        string? category = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(category))
        {
            filter &= builder.Regex(p => p.Category, new MongoDB.Bson.BsonRegularExpression(category, "i"));
        }

        if (minPrice.HasValue)
        {
            filter &= builder.Gte(p => p.Price, minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            filter &= builder.Lte(p => p.Price, maxPrice.Value);
        }

        var products = await _context.Products.Find(filter).ToListAsync();
        return products.Select(MapToResponseDto);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product == null ? null : MapToResponseDto(product);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Category = dto.Category,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Products.InsertOneAsync(product);
        return MapToResponseDto(product);
    }

    public async Task<ProductResponseDto?> UpdateAsync(string id, UpdateProductDto dto)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        var existing = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (existing == null) return null;

        var updateBuilder = Builders<Product>.Update
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var updates = new List<UpdateDefinition<Product>>
        {
            updateBuilder
        };

        if (dto.Name != null) updates.Add(Builders<Product>.Update.Set(p => p.Name, dto.Name));
        if (dto.Description != null) updates.Add(Builders<Product>.Update.Set(p => p.Description, dto.Description));
        if (dto.Price.HasValue) updates.Add(Builders<Product>.Update.Set(p => p.Price, dto.Price.Value));
        if (dto.Category != null) updates.Add(Builders<Product>.Update.Set(p => p.Category, dto.Category));
        if (dto.Stock.HasValue) updates.Add(Builders<Product>.Update.Set(p => p.Stock, dto.Stock.Value));
        if (dto.ImageUrl != null) updates.Add(Builders<Product>.Update.Set(p => p.ImageUrl, dto.ImageUrl));
        if (dto.IsActive.HasValue) updates.Add(Builders<Product>.Update.Set(p => p.IsActive, dto.IsActive.Value));

        var combinedUpdate = Builders<Product>.Update.Combine(updates);
        var updatedProduct = await _context.Products.FindOneAndUpdateAsync(
            p => p.Id == id,
            combinedUpdate,
            new FindOneAndUpdateOptions<Product> { ReturnDocument = ReturnDocument.After }
        );

        return updatedProduct == null ? null : MapToResponseDto(updatedProduct);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return false;

        var result = await _context.Products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    private static ProductResponseDto MapToResponseDto(Product product) =>
        new(
            product.Id!,
            product.Name,
            product.Description,
            product.Price,
            product.Category,
            product.Stock,
            // product.ImageUrl,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt
        );
}
