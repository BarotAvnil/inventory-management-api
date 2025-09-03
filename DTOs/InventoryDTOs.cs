using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs;

// Category DTOs
public record CategoryDto(
    int Id,
    string Name,
    int? ParentId
);

public record CreateCategoryRequest(
    [Required, MaxLength(120)] string Name,
    int? ParentId = null
);

public record UpdateCategoryRequest(
    [Required, MaxLength(120)] string Name,
    int? ParentId
);

// Product DTOs
public record ProductDto(
    int Id,
    string Name,
    string Sku,
    int CategoryId,
    int SupplierId,
    int UnitId,
    int Quantity,
    double UnitPrice,
    string? Description,
    string? CheckinAt,
    string? CheckoutAt
);

public record CreateProductRequest(
    [Required, MaxLength(160)] string Name,
    [Required, MaxLength(50)] string Sku,
    [Required] int CategoryId,
    [Required] int SupplierId,
    [Required] int UnitId,
    [Required] int Quantity,
    [Required, Range(0, double.MaxValue)] double UnitPrice,
    [MaxLength(500)] string? Description = null
);

public record UpdateProductRequest(
    [Required, MaxLength(160)] string Name,
    [Required, MaxLength(50)] string Sku,
    [Required] int CategoryId,
    [Required] int SupplierId,
    [Required] int UnitId,
    [Required] int Quantity,
    [Required, Range(0, double.MaxValue)] double UnitPrice,
    [MaxLength(500)] string? Description = null
);

// Supplier DTOs
public record SupplierDto(
    int Id,
    string Name,
    string? Email,
    string? Phone,
    string? CheckinAt,
    string? CheckoutAt
);

public record CreateSupplierRequest(
    [Required, MaxLength(140)] string Name,
    [EmailAddress, MaxLength(100)] string? Email = null,
    [MaxLength(20)] string? Phone = null
);

public record UpdateSupplierRequest(
    [Required, MaxLength(140)] string Name,
    [EmailAddress, MaxLength(100)] string? Email,
    [MaxLength(20)] string? Phone
);

// Unit DTOs
public record UnitDto(
    int Id,
    string Name,
    string? CheckinAt,
    string? CheckoutAt
);

public record CreateUnitRequest(
    [Required, MaxLength(50)] string Name
);

public record UpdateUnitRequest(
    [Required, MaxLength(50)] string Name
);

// Stock Transaction DTOs
public record StockTransactionDto(
    int Id,
    int ProductId,
    string Type,
    int Quantity,
    string Date,
    string? Remarks,
    int UserId,
    string? CheckinAt,
    string? CheckoutAt
);

public record StockAdjustmentRequest(
    [Required, Range(1, int.MaxValue)] int Quantity,
    [MaxLength(500)] string? Remarks
);

// Common DTOs
public record PaginatedResult<T>(
    IEnumerable<T> Items,
    int TotalItems,
    int Page,
    int PageSize,
    int TotalPages
);

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message,
    IEnumerable<string>? Errors = null
);

public record DashboardStatsDto(
    int TotalProducts,
    int ActiveProducts,
    int LowStockProducts,
    int TotalCategories,
    int TotalSuppliers,
    int TotalUnits,
    decimal TotalInventoryValue,
    int RecentTransactions
);
