namespace aspdotnet.Models.Entities;

/// <summary>
/// User entity stored in the MongoDB "users" collection.
/// Inherits `Id`, `CreatedAt`, and `UpdatedAt` from BaseEntity.
/// </summary>
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer"; // "Customer" or "Admin"
}
