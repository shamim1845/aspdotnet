using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace aspdotnet.Models.Entities;

/// <summary>
/// Product entity stored in the MongoDB "products" collection.
/// 
/// 💡 TS / JS Comparison:
/// - In JavaScript, numbers are 64-bit floating point (can have precision bugs like 0.1 + 0.2 = 0.30000000000000004).
/// - In C#, `decimal` is a high-precision 128-bit type designed specifically for financial and currency calculations.
/// - `[BsonRepresentation(BsonType.Decimal128)]`: Stores the decimal accurately as a 128-bit BSON decimal in MongoDB.
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
