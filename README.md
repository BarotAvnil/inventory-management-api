
# InventoryApi (.NET 9 + EF Core + SQLite) — No Audit Logs

A ready-to-run Inventory Management System API with the following tables:
Users, Categories (with ParentId), Units, Suppliers, Products, StockTransactions.

## Run
```bash
dotnet restore
dotnet build
dotnet run
```
Swagger: http://localhost:5083/swagger

## Endpoints (high level)
- /api/users  (GET, POST, PUT, DELETE)
- /api/categories  (GET, POST, PUT, DELETE)
- /api/units  (GET, POST, PUT, DELETE)
- /api/suppliers  (GET, POST, PUT, DELETE)
- /api/products  (GET, POST, PUT, DELETE)
- /api/products/{id}/stock-in?userId=1  (POST)
- /api/products/{id}/stock-out?userId=1  (POST)
- /api/stocktransactions  (GET with filters: productId, from, to)

DB file: Data/ims.db (pre-seeded)
