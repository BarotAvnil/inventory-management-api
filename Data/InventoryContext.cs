using Microsoft.EntityFrameworkCore;
using InventoryApi.Models;

namespace InventoryApi.Data;

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId);

        modelBuilder.Entity<Product>().Property(p => p.UnitPrice).HasPrecision(18,2);
        modelBuilder.Entity<StockTransaction>().Property(t => t.Quantity).HasPrecision(18,2);

        base.OnModelCreating(modelBuilder);
    }
}
