using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using InventoryApi.Services;
using InventoryApi.DTOs;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    [HttpGet("profile")]
    public ActionResult<object> GetProfile()
    {
        return Ok(new
        {
            UserId = "1",
            Name = "Admin",
            Email = "admin@inventorymanagement.com",
            Role = "Administrator"
        });
    }

    [HttpPost("validate-token")]
    public ActionResult ValidateToken()
    {
        return Ok(new { Message = "Token is valid", IsValid = true });
    }
}
