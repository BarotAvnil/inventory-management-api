using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly InventoryContext _db;
    public SuppliersController(InventoryContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supplier>>> GetAll()
        => await _db.Suppliers.OrderBy(c => c.Name).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Supplier>> Get(int id)
    {
        var e = await _db.Suppliers.FindAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Supplier>> Create(Supplier c)
    {
        _db.Suppliers.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Supplier>> Update(int id, Supplier dto)
    {
        var e = await _db.Suppliers.FindAsync(id);
        if (e is null) return NotFound();
        e.Name = dto.Name; e.Email = dto.Email; e.Phone = dto.Phone;
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Suppliers.FindAsync(id);
        if (e is null) return NotFound();
        _db.Suppliers.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
