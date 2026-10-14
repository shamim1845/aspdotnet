using aspdotnet.Data;
using aspdotnet.Models.DTOs;
using aspdotnet.Models.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace aspdotnet.Services;

public class UserService : IUserService
{
    private readonly MongoDbContext _context;

    public UserService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users.Find(_ => true).ToListAsync();
        return users.Select(MapToResponseDto);
    }

    public async Task<UserResponseDto?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return null;

        var user = await _context.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
        return user == null ? null : MapToResponseDto(user);
    }

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

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _)) return false;

        var result = await _context.Users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    private static UserResponseDto MapToResponseDto(User user) =>
        new(
            user.Id!,
            user.Name,
            user.Email,
            user.Role,
            user.CreatedAt
        );
}
