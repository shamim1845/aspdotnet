using aspdotnet.Models.DTOs;

namespace aspdotnet.Services;

/// <summary>
/// Service contract for User operations.
/// </summary>
public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(string id);
    Task<UserResponseDto> CreateAsync(CreateUserDto dto);
    Task<bool> DeleteAsync(string id);
}
