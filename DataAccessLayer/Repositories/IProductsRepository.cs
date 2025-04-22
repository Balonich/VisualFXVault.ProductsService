using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByConditionAsync(Expression<Func<Product, bool>> predicate);
        Task<IEnumerable<Product>> GetProductsByConditionAsync(Expression<Func<Product, bool>> predicate);
        Task<Product> AddProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}
