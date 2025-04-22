using System.Linq.Expressions;
using DataAccessLayer.Database;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByConditionAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _dbContext.Products
            .Where(predicate)
            .ToListAsync();
    }

    public Task<Product?> GetProductAsync(Guid productId)
    {
        return _dbContext.Products
            .FirstOrDefaultAsync(p => p.ProductID == productId);
    }

    public Task<Product?> GetProductByConditionAsync(Expression<Func<Product, bool>> predicate)
    {
        return _dbContext.Products
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);

        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}