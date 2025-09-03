using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly InventoryContext _db;
    public CategoriesController(InventoryContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        => await _db.Categories.OrderBy(c => c.Name).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> Get(int id)
    {
        var e = await _db.Categories.FindAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create(Category c)
    {
        _db.Categories.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Category>> Update(int id, Category dto)
    {
        var e = await _db.Categories.FindAsync(id);
        if (e is null) return NotFound();
        e.Name = dto.Name; e.ParentId = dto.ParentId;
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Categories.FindAsync(id);
        if (e is null) return NotFound();
        _db.Categories.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
