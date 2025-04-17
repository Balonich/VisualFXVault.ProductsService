using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository _repository;
        public ProductService(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> AddProductAsync(ProductAddRequest request)
        {
            var product = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductName = request.ProductName,
                Category = request.Category,
                UnitPrice = request.UnitPrice,
                QuantityInStock = request.QuantityInStock
            };
            await _repository.AddProductAsync(product);
            return MapToResponse(product);
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            await _repository.DeleteProductAsync(productId);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _repository.GetProductsAsync();
            return products.Select(MapToResponse);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid productId)
        {
            var product = await _repository.GetProductByConditionAsync(p => p.ProductID == productId);
            return product is null ? null : MapToResponse(product);
        }

        public async Task<IEnumerable<ProductResponse>> SearchProductsAsync(string searchString)
        {
            var products = await _repository.GetProductsAsync();
            var filtered = products.Where(p =>
                p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            return filtered.Select(MapToResponse);
        }

        public async Task UpdateProductAsync(ProductUpdateRequest request)
        {
            var existing = await _repository.GetProductByConditionAsync(p => p.ProductID == request.ProductID);
            if (existing is null)
            {
                throw new KeyNotFoundException($"Product with ID {request.ProductID} not found.");
            }
            existing.ProductName = request.ProductName;
            existing.Category = request.Category;
            existing.UnitPrice = request.UnitPrice;
            existing.QuantityInStock = request.QuantityInStock;
            await _repository.UpdateProductAsync(existing);
        }

        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Category = product.Category,
                UnitPrice = product.UnitPrice,
                QuantityInStock = product.QuantityInStock
            };
        }
    }
}