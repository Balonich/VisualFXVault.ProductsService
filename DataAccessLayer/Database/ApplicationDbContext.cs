using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product { ProductID = Guid.NewGuid(), ProductName = "Sample Product 1", Category = "Category A", UnitPrice = 19.99, QuantityInStock = 100 },
            new Product { ProductID = Guid.NewGuid(), ProductName = "Sample Product 2", Category = "Category B", UnitPrice = 29.50, QuantityInStock = 50 }
        );
    }
}