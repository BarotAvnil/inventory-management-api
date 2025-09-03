using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;
using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<UserDto>> RegisterAsync(RegisterRequest request);
}

public class AuthService : IAuthService
{
    private readonly InventoryContext _context;
    private readonly IConfiguration _config;

    public AuthService(InventoryContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || user.Password != request.Password)
            {
                return new ApiResponse<LoginResponse>(false, null, "Invalid email or password");
            }

            var response = new LoginResponse(
                user.Id,
                user.Email,
                user.Mobile,
                user.Username
            );

            return new ApiResponse<LoginResponse>(true, response, "Login successful");
        }
        catch (Exception ex)
        {
            return new ApiResponse<LoginResponse>(false, null, "Login failed", new[] { ex.Message });
        }
    }

    public async Task<ApiResponse<UserDto>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return new ApiResponse<UserDto>(false, null, "User with this email already exists");
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Mobile = request.Mobile,
                Username = request.Username,
                CreatedAt = DateTime.UtcNow.ToString()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.Password,
                user.Mobile,
                user.Username,
                user.CreatedAt,
                user.UpdatedAt
            );

            return new ApiResponse<UserDto>(true, userDto, "Registration successful");
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserDto>(false, null, "Registration failed", new[] { ex.Message });
        }
    }
}
