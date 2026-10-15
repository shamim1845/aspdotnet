using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace aspdotnet.Models.Entities;

/// <summary>
/// Abstract base entity for all MongoDB collections.
/// 
/// 💡 TS / Mongoose Comparison:
/// In Mongoose, this is equivalent to `{ _id: Schema.Types.ObjectId, timestamps: true }`.
/// In C#:
/// - `abstract class`: Cannot be instantiated directly with `new BaseEntity()`; must be inherited.
/// - `[BsonId]`: Tells MongoDB this property is the `_id` document identifier.
/// - `[BsonRepresentation(BsonType.ObjectId)]`: Converts MongoDB's 12-byte binary ObjectId into a 24-character C# string automatically.
/// </summary>
public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
