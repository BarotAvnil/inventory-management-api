using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InventoryApi.Services;
using InventoryApi.DTOs;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public DashboardController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("health")]
    public ActionResult GetHealth()
    {
        return Ok(new { Status = "OK", Timestamp = DateTime.UtcNow });
    }
}
