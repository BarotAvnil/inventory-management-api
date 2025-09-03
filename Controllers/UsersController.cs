using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;

namespace InventoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly InventoryContext _db;
    public UsersController(InventoryContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll() => await _db.Users.OrderBy(u => u.Username).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var e = await _db.Users.FindAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User u)
    {
        _db.Users.Add(u);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = u.Id }, u);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> Update(int id, User dto)
    {
        var e = await _db.Users.FindAsync(id);
        if (e is null) return NotFound();
        e.Username = dto.Username; e.Email = dto.Email; e.Mobile = dto.Mobile; e.Password = dto.Password;
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Users.FindAsync(id);
        if (e is null) return NotFound();
        _db.Users.Remove(e);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
