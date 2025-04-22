using System.Linq.Expressions;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories.Products;

public interface IProductsRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetProductsByConditionAsync(Expression<Func<Product, bool>> predicate);
    Task<Product?> GetProductAsync(Guid productId);
    Task<Product?> GetProductByConditionAsync(Expression<Func<Product, bool>> predicate);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productId);
}