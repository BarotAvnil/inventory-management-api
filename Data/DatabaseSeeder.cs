using Microsoft.EntityFrameworkCore;
using InventoryApi.Models;
using InventoryApi.Services;
using InventoryApi.DTOs;

namespace InventoryApi.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(InventoryContext context, IAuthService authService)
    {
        // Skip seeding if data already exists
        if (await context.Users.AnyAsync())
            return;

        // Create default admin user
        var adminRequest = new RegisterRequest(
            "admin@inventorymanagement.com",
            "Admin123!",
            "+1234567890",
            "Admin"
        );
        await authService.RegisterAsync(adminRequest);

        // Create default categories
        var categories = new[]
        {
            new Category { Name = "Electronics" },
            new Category { Name = "Clothing" },
            new Category { Name = "Books" },
            new Category { Name = "Home & Garden" },
            new Category { Name = "Sports" }
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        // Create subcategories
        var subcategories = new[]
        {
            new Category { Name = "Smartphones", ParentId = categories[0].Id },
            new Category { Name = "Laptops", ParentId = categories[0].Id },
            new Category { Name = "Men's Clothing", ParentId = categories[1].Id },
            new Category { Name = "Women's Clothing", ParentId = categories[1].Id }
        };

        context.Categories.AddRange(subcategories);
        await context.SaveChangesAsync();

        // Create default units
        var units = new[]
        {
            new Unit { Name = "Pieces", CheckinAt = DateTime.UtcNow.ToString() },
            new Unit { Name = "Kilograms", CheckinAt = DateTime.UtcNow.ToString() },
            new Unit { Name = "Meters", CheckinAt = DateTime.UtcNow.ToString() },
            new Unit { Name = "Liters", CheckinAt = DateTime.UtcNow.ToString() },
            new Unit { Name = "Boxes", CheckinAt = DateTime.UtcNow.ToString() }
        };

        context.Units.AddRange(units);
        await context.SaveChangesAsync();

        // Create default suppliers
        var suppliers = new[]
        {
            new Supplier 
            { 
                Name = "TechCorp Ltd", 
                Email = "contact@techcorp.com", 
                Phone = "+1234567891", 
                CheckinAt = DateTime.UtcNow.ToString() 
            },
            new Supplier 
            { 
                Name = "Fashion Forward Inc", 
                Email = "sales@fashionforward.com", 
                Phone = "+1234567892", 
                CheckinAt = DateTime.UtcNow.ToString() 
            },
            new Supplier 
            { 
                Name = "Book World", 
                Email = "orders@bookworld.com", 
                Phone = "+1234567893", 
                CheckinAt = DateTime.UtcNow.ToString() 
            }
        };

        context.Suppliers.AddRange(suppliers);
        await context.SaveChangesAsync();

        // Create sample products
        var products = new[]
        {
            new Product
            {
                Name = "iPhone 15 Pro",
                Sku = "APL-IP15P-128",
                Description = "Latest iPhone with advanced features",
                CategoryId = subcategories[0].Id,
                UnitId = units[0].Id,
                SupplierId = suppliers[0].Id,
                UnitPrice = 999.99,
                Quantity = 50,
                CheckinAt = DateTime.UtcNow.ToString()
            },
            new Product
            {
                Name = "MacBook Air M2",
                Sku = "APL-MBA-M2-256",
                Description = "Lightweight laptop with M2 chip",
                CategoryId = subcategories[1].Id,
                UnitId = units[0].Id,
                SupplierId = suppliers[0].Id,
                UnitPrice = 1199.99,
                Quantity = 25,
                CheckinAt = DateTime.UtcNow.ToString()
            },
            new Product
            {
                Name = "Premium Cotton T-Shirt",
                Sku = "FASH-TSH-COT-M",
                Description = "High-quality cotton t-shirt for men",
                CategoryId = subcategories[2].Id,
                UnitId = units[0].Id,
                SupplierId = suppliers[1].Id,
                UnitPrice = 29.99,
                Quantity = 100,
                CheckinAt = DateTime.UtcNow.ToString()
            },
            new Product
            {
                Name = "Women's Jeans",
                Sku = "FASH-JNS-WOM-32",
                Description = "Stylish women's jeans, size 32",
                CategoryId = subcategories[3].Id,
                UnitId = units[0].Id,
                SupplierId = suppliers[1].Id,
                UnitPrice = 79.99,
                Quantity = 60,
                CheckinAt = DateTime.UtcNow.ToString()
            },
            new Product
            {
                Name = "Programming Fundamentals",
                Sku = "BOOK-PROG-FUND-2023",
                Description = "Comprehensive guide to programming",
                CategoryId = categories[2].Id,
                UnitId = units[0].Id,
                SupplierId = suppliers[2].Id,
                UnitPrice = 49.99,
                Quantity = 30,
                CheckinAt = DateTime.UtcNow.ToString()
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // Create some initial stock transactions
        var admin = await context.Users.FirstAsync(u => u.Email == "admin@inventorymanagement.com");
        var transactions = new[]
        {
            new StockTransaction
            {
                ProductId = products[0].Id,
                UserId = admin.Id,
                Type = "IN",
                Quantity = 50,
                Remarks = "Initial stock",
                Date = DateTime.UtcNow.AddDays(-7).ToString()
            },
            new StockTransaction
            {
                ProductId = products[1].Id,
                UserId = admin.Id,
                Type = "IN",
                Quantity = 25,
                Remarks = "Initial stock",
                Date = DateTime.UtcNow.AddDays(-7).ToString()
            },
            new StockTransaction
            {
                ProductId = products[2].Id,
                UserId = admin.Id,
                Type = "IN",
                Quantity = 100,
                Remarks = "Initial stock",
                Date = DateTime.UtcNow.AddDays(-6).ToString()
            },
            new StockTransaction
            {
                ProductId = products[0].Id,
                UserId = admin.Id,
                Type = "OUT",
                Quantity = 5,
                Remarks = "Sample sale",
                Date = DateTime.UtcNow.AddDays(-3).ToString()
            }
        };

        context.StockTransactions.AddRange(transactions);
        await context.SaveChangesAsync();
    }
}
