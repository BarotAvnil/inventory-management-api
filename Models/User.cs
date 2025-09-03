using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Mobile { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? Username { get; set; }

    public string? CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }

    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
