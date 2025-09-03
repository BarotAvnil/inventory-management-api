using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models;

public class StockTransaction
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required, MaxLength(10)]
    public string Type { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public string Date { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public string? CheckinAt { get; set; }
    public string? CheckoutAt { get; set; }
}
