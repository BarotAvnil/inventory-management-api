using Microsoft.EntityFrameworkCore;
using InventoryApi.Data;
using InventoryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<InventoryContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Data/ims.db";
    opt.UseSqlite(connectionString);
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Use CORS
app.UseCors("AllowAll");

// Only use HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InventoryContext>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await DatabaseSeeder.SeedAsync(context, authService);
}

app.Run();
