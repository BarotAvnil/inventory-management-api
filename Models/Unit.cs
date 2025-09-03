using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models;

public class Unit
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public string? CheckinAt { get; set; }
    public string? CheckoutAt { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
