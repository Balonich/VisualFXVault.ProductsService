using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByConditionAsync(Expression<Func<Product, bool>> predicate);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid productId);
    }
}