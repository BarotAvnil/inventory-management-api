using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsController : ControllerBase
{
    private readonly InventoryContext _db;
    public UnitsController(InventoryContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Unit>>> GetAll()
        => await _db.Units.OrderBy(c => c.Name).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Unit>> Get(int id)
    {
        var e = await _db.Units.FindAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Unit>> Create(Unit c)
    {
        _db.Units.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Unit>> Update(int id, Unit dto)
    {
        var e = await _db.Units.FindAsync(id);
        if (e is null) return NotFound();
        e.Name = dto.Name;
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Units.FindAsync(id);
        if (e is null) return NotFound();
        _db.Units.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
