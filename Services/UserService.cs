using aspdotnet.Data;
using aspdotnet.Models.DTOs;
using aspdotnet.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace aspdotnet.Services;

/// <summary>
/// Service implementation for User business operations.
/// </summary>
public class UserService : IUserService
{
    private readonly MongoDbContext _context;

    public UserService(MongoDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all users.
    /// `_ => true`: Lambda expression matching every document, equivalent to `{}` in MongoDB/Mongoose.
    /// </summary>
    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users.Find(_ => true).ToListAsync();
        return users.Select(MapToResponseDto);
    }

    /// <summary>
    /// Finds a user by their MongoDB ObjectId string.
    /// </summary>
    public async Task<UserResponseDto?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
        return user == null ? null : MapToResponseDto(user);
    }

    /// <summary>
    /// Creates and persists a new user entity.
    /// </summary>
    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "Customer" : dto.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Users.InsertOneAsync(user);
        return MapToResponseDto(user);
    }

    /// <summary>
    /// Removes a user from the collection by Id.
    /// </summary>
    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return false;

        var result = await _context.Users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Transforms the internal User entity to the public UserResponseDto.
    /// </summary>
    private static UserResponseDto MapToResponseDto(User user) =>
        new(
            user.Id!,
            user.Name,
            user.Email,
            user.Role,
            user.CreatedAt
        );
}
