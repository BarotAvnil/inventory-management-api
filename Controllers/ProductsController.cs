using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly InventoryContext _db;
    public ProductsController(InventoryContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] string? search = null)
    {
        var q = _db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(p => p.Name.Contains(search));
        return await q.OrderBy(p => p.Name).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
        var e = await _db.Products.FindAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product p)
    {
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(int id, Product dto)
    {
        var e = await _db.Products.FindAsync(id);
        if (e is null) return NotFound();
        e.Name = dto.Name; e.CategoryId = dto.CategoryId; e.UnitId = dto.UnitId; e.SupplierId = dto.SupplierId;
        e.UnitPrice = dto.UnitPrice; e.Quantity = dto.Quantity; e.Sku = dto.Sku; e.Description = dto.Description;
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Products.FindAsync(id);
        if (e is null) return NotFound();
        _db.Products.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:int}/stock-in")]
    public async Task<IActionResult> StockIn(int id, [FromQuery] int userId, [FromBody] StockAdjustDto dto)
    {
        var p = await _db.Products.FindAsync(id);
        if (p is null) return NotFound(new { error = "Product not found" });

        p.Quantity += (int)dto.Quantity;
        _db.StockTransactions.Add(new StockTransaction {
            ProductId = id,
            UserId = userId,
            Quantity = (int)dto.Quantity,
            Type = "IN",
            Remarks = dto.Remarks,
            Date = DateTime.UtcNow.ToString()
        });
        await _db.SaveChangesAsync();
        return Ok(new { message = "Stock increased", p.Quantity });
    }

    [HttpPost("{id:int}/stock-out")]
    public async Task<IActionResult> StockOut(int id, [FromQuery] int userId, [FromBody] StockAdjustDto dto)
    {
        var p = await _db.Products.FindAsync(id);
        if (p is null) return NotFound(new { error = "Product not found" });
        if (p.Quantity < (int)dto.Quantity) return BadRequest(new { error = "Insufficient stock" });

        p.Quantity -= (int)dto.Quantity;
        _db.StockTransactions.Add(new StockTransaction {
            ProductId = id,
            UserId = userId,
            Quantity = (int)dto.Quantity,
            Type = "OUT",
            Remarks = dto.Remarks,
            Date = DateTime.UtcNow.ToString()
        });
        await _db.SaveChangesAsync();
        return Ok(new { message = "Stock decreased", p.Quantity });
    }
}

public record StockAdjustDto(decimal Quantity, string? Remarks);
