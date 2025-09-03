using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public int UnitId { get; set; }
    public Unit? Unit { get; set; }

    public int Quantity { get; set; }
    public double UnitPrice { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public string? CheckinAt { get; set; }
    public string? CheckoutAt { get; set; }

    public ICollection<StockTransaction> Transactions { get; set; } = new List<StockTransaction>();
}
