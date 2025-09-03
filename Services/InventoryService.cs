using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Models;
using InventoryApi.DTOs;

namespace InventoryApi.Services;

public interface IInventoryService
{
    Task<PaginatedResult<ProductDto>> GetProductsAsync(int page = 1, int pageSize = 10, string? search = null, int? categoryId = null);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductByCodeAsync(string code);
    Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductRequest request);
    Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<ApiResponse<bool>> DeleteProductAsync(int id);
}

public class InventoryService : IInventoryService
{
    private readonly InventoryContext _context;

    public InventoryService(InventoryContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<ProductDto>> GetProductsAsync(int page = 1, int pageSize = 10, string? search = null, int? categoryId = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search) || 
                                   (p.Sku != null && p.Sku.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var products = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Sku,
                p.CategoryId,
                p.SupplierId,
                p.UnitId,
                p.Quantity,
                p.UnitPrice,
                p.Description,
                p.CheckinAt,
                p.CheckoutAt
            ))
            .ToListAsync();

        return new PaginatedResult<ProductDto>(products, totalItems, page, pageSize, totalPages);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.Supplier)
            .Where(p => p.Id == id)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Sku,
                p.CategoryId,
                p.SupplierId,
                p.UnitId,
                p.Quantity,
                p.UnitPrice,
                p.Description,
                p.CheckinAt,
                p.CheckoutAt
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto?> GetProductByCodeAsync(string code)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Include(p => p.Supplier)
            .Where(p => p.Sku == code)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Sku,
                p.CategoryId,
                p.SupplierId,
                p.UnitId,
                p.Quantity,
                p.UnitPrice,
                p.Description,
                p.CheckinAt,
                p.CheckoutAt
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductRequest request)
    {
        try
        {
            // Check if SKU already exists
            if (!string.IsNullOrEmpty(request.Sku) && 
                await _context.Products.AnyAsync(p => p.Sku == request.Sku))
            {
                return new ApiResponse<ProductDto>(false, null, "Product SKU already exists");
            }

            // Validate foreign keys
            if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId))
                return new ApiResponse<ProductDto>(false, null, "Invalid category");

            if (!await _context.Units.AnyAsync(u => u.Id == request.UnitId))
                return new ApiResponse<ProductDto>(false, null, "Invalid unit");

            if (!await _context.Suppliers.AnyAsync(s => s.Id == request.SupplierId))
                return new ApiResponse<ProductDto>(false, null, "Invalid supplier");

            var product = new Product
            {
                Name = request.Name,
                Sku = request.Sku,
                Description = request.Description,
                CategoryId = request.CategoryId,
                UnitId = request.UnitId,
                SupplierId = request.SupplierId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                CheckinAt = DateTime.UtcNow.ToString()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productDto = await GetProductByIdAsync(product.Id);
            return new ApiResponse<ProductDto>(true, productDto, "Product created successfully");
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>(false, null, "Product creation failed", new[] { ex.Message });
        }
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new ApiResponse<ProductDto>(false, null, "Product not found");

            // Check if SKU already exists for different product
            if (!string.IsNullOrEmpty(request.Sku) && 
                await _context.Products.AnyAsync(p => p.Sku == request.Sku && p.Id != id))
            {
                return new ApiResponse<ProductDto>(false, null, "Product SKU already exists");
            }

            // Validate foreign keys
            if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId))
                return new ApiResponse<ProductDto>(false, null, "Invalid category");

            if (!await _context.Units.AnyAsync(u => u.Id == request.UnitId))
                return new ApiResponse<ProductDto>(false, null, "Invalid unit");

            if (!await _context.Suppliers.AnyAsync(s => s.Id == request.SupplierId))
                return new ApiResponse<ProductDto>(false, null, "Invalid supplier");

            product.Name = request.Name;
            product.Sku = request.Sku;
            product.Description = request.Description;
            product.CategoryId = request.CategoryId;
            product.UnitId = request.UnitId;
            product.SupplierId = request.SupplierId;
            product.Quantity = request.Quantity;
            product.UnitPrice = request.UnitPrice;

            await _context.SaveChangesAsync();

            var productDto = await GetProductByIdAsync(product.Id);
            return new ApiResponse<ProductDto>(true, productDto, "Product updated successfully");
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>(false, null, "Product update failed", new[] { ex.Message });
        }
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return new ApiResponse<bool>(false, false, "Product not found");

            // Remove the product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, true, "Product deleted successfully");
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>(false, false, "Product deletion failed", new[] { ex.Message });
        }
    }
}
