using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record RegisterRequest(
    [Required, EmailAddress, MaxLength(160)] string Email,
    [Required, MinLength(6)] string Password,
    [Required, MaxLength(20)] string Mobile,
    [MaxLength(120)] string? Username = null
);

public record LoginResponse(
    int Id,
    string Email,
    string Mobile,
    string? Username
);

public record UserDto(
    int Id,
    string Email,
    string Password,
    string Mobile,
    string? Username,
    string? CreatedAt,
    string? UpdatedAt
);

public record CreateUserRequest(
    [Required, MaxLength(120)] string FullName,
    [Required, EmailAddress, MaxLength(160)] string Email,
    [MaxLength(20)] string? Mobile,
    [Required, MinLength(6)] string Password,
    [MaxLength(50)] string Role = "User",
    bool IsActive = true
);

public record UpdateUserRequest(
    [Required, MaxLength(120)] string FullName,
    [MaxLength(20)] string? Mobile,
    [MaxLength(50)] string Role,
    bool IsActive
);

public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required, MinLength(6)] string NewPassword
);
