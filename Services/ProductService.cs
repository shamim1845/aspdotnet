using aspdotnet.Data;
using aspdotnet.Models.DTOs;
using aspdotnet.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace aspdotnet.Services;

/// <summary>
/// Service implementation managing product data access and business logic.
/// 
/// 💡 TS / NestJS Comparison:
/// In NestJS, this is an `@Injectable()` service injected into your controller constructor.
/// </summary>
public class ProductService : IProductService
{
    // `readonly`: Assigned once (in constructor) and cannot be modified elsewhere, just like TypeScript's `readonly`.
    private readonly MongoDbContext _context;

    public ProductService(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all products with optional filtering.
    /// Demonstrates MongoDB Query Building and LINQ mapping.
    /// </summary>
    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(
        string? category = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null)
    {
        // `Builders<Product>.Filter`: Type-safe MongoDB filter builder.
        // Equivalent to building a MongoDB filter object in JS: `const filter = {};`
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        // In JS: if (category) filter.category = new RegExp(category, 'i');
        if (!string.IsNullOrWhiteSpace(category))
        {
            filter &= builder.Regex(p => p.Category, new MongoDB.Bson.BsonRegularExpression(category, "i"));
        }

        // In JS: if (minPrice != null) filter.price = { ...filter.price, $gte: minPrice };
        // In C#: `.HasValue` is a property of nullable value types (`decimal?`, `int?`).
        if (minPrice.HasValue)
        {
            filter &= builder.Gte(p => p.Price, minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            filter &= builder.Lte(p => p.Price, maxPrice.Value);
        }

        // `await ... ToListAsync()`: Awaits the MongoDB cursor and fetches documents as a C# List.
        var products = await _context.Products.Find(filter).ToListAsync();

        // LINQ `.Select(MapToResponseDto)`: Identical to JavaScript's `products.map(mapToResponseDto)`.
        return products.Select(MapToResponseDto);
    }

    /// <summary>
    /// Retrieves a single product by its 24-character hexadecimal MongoDB ObjectId.
    /// </summary>
    public async Task<ProductResponseDto?> GetByIdAsync(string id)
    {
        // `ObjectId.TryParse(id, out _)`:
        // Validates hexadecimal format (like `mongoose.Types.ObjectId.isValid(id)` in Node.js).
        // `out _`: The discard `_` means we don't care about saving the parsed output variable.
        if (!ObjectId.TryParse(id, out _)) return null;

        // `.FirstOrDefaultAsync()`: Returns the first match or null if not found (like JS `.find()` or Mongoose `.findOne()`).
        var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        return product == null ? null : MapToResponseDto(product);
    }

    /// <summary>
    /// Inserts a new product into the database.
    /// </summary>
    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        // C# Object Initializer syntax: `new Product { Prop1 = val, Prop2 = val }`.
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

        // Inserts document; MongoDB driver automatically generates and populates `product.Id`.
        await _context.Products.InsertOneAsync(product);
        return MapToResponseDto(product);
    }

    /// <summary>
    /// Applies partial updates to a product document using MongoDB update definitions.
    /// </summary>
    public async Task<ProductResponseDto?> UpdateAsync(string id, UpdateProductDto dto)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        var existing = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (existing == null) return null;

        // In Mongoose: `$set: { updatedAt: new Date(), ... }`
        var updateBuilder = Builders<Product>.Update
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var updates = new List<UpdateDefinition<Product>>
        {
            updateBuilder
        };

        // Conditionally apply only provided fields
        if (dto.Name != null) updates.Add(Builders<Product>.Update.Set(p => p.Name, dto.Name));
        if (dto.Description != null) updates.Add(Builders<Product>.Update.Set(p => p.Description, dto.Description));
        if (dto.Price.HasValue) updates.Add(Builders<Product>.Update.Set(p => p.Price, dto.Price.Value));
        if (dto.Category != null) updates.Add(Builders<Product>.Update.Set(p => p.Category, dto.Category));
        if (dto.Stock.HasValue) updates.Add(Builders<Product>.Update.Set(p => p.Stock, dto.Stock.Value));
        if (dto.ImageUrl != null) updates.Add(Builders<Product>.Update.Set(p => p.ImageUrl, dto.ImageUrl));
        if (dto.IsActive.HasValue) updates.Add(Builders<Product>.Update.Set(p => p.IsActive, dto.IsActive.Value));

        var combinedUpdate = Builders<Product>.Update.Combine(updates);

        // Atomically updates and returns the modified document (like `{ new: true }` in Mongoose findOneAndUpdate)
        var updatedProduct = await _context.Products.FindOneAndUpdateAsync(
            p => p.Id == id,
            combinedUpdate,
            new FindOneAndUpdateOptions<Product> { ReturnDocument = ReturnDocument.After }
        );

        return updatedProduct == null ? null : MapToResponseDto(updatedProduct);
    }

    /// <summary>
    /// Deletes a product by Id.
    /// </summary>
    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return false;

        var result = await _context.Products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Private mapping helper transforming Entity -> Response DTO.
    /// `product.Id!`: The null-forgiving operator `!` tells the compiler we know `Id` is not null here.
    /// </summary>
    private static ProductResponseDto MapToResponseDto(Product product) =>
        new(
            product.Id!,
            product.Name,
            product.Description,
            product.Price,
            product.Category,
            product.Stock,
            product.ImageUrl,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt
        );
}
