using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockTransactionsController : ControllerBase
{
    private readonly InventoryContext _db;
    public StockTransactionsController(InventoryContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockTransaction>>> Get([FromQuery] int? productId = null, [FromQuery] string? from = null, [FromQuery] string? to = null)
    {
        var q = _db.StockTransactions.AsQueryable();
        if (productId.HasValue) q = q.Where(t => t.ProductId == productId.Value);
        if (!string.IsNullOrEmpty(from)) q = q.Where(t => string.Compare(t.Date, from) >= 0);
        if (!string.IsNullOrEmpty(to)) q = q.Where(t => string.Compare(t.Date, to) <= 0);
        var list = await q.OrderByDescending(t => t.Date).ToListAsync();
        return Ok(list);
    }
}
