using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models;

public class Supplier
{
    public int Id { get; set; }

    [Required, MaxLength(140)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public string? CheckinAt { get; set; }
    public string? CheckoutAt { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
