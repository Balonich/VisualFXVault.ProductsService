using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services
{
    public class ProductService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productsRepository.GetProductsAsync();
            return products.Select(MapToProductResponse);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid productId)
        {
            var product = await _productsRepository.GetProductByConditionAsync(p => p.ProductID == productId);
            return product == null ? null : MapToProductResponse(product);
        }

        public async Task<IEnumerable<ProductResponse>> SearchProductsAsync(string searchString)
        {
            Expression<Func<Product, bool>> predicate = p =>
                (p.ProductName != null && p.ProductName.Contains(searchString)) ||
                (p.Category != null && p.Category.Contains(searchString));

            var products = await _productsRepository.GetProductsByConditionAsync(predicate);
            return products.Select(MapToProductResponse);
        }

        public async Task<ProductResponse> AddProductAsync(ProductAddRequest productAddRequest)
        {
            var product = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductName = productAddRequest.ProductName,
                Category = productAddRequest.Category,
                UnitPrice = productAddRequest.UnitPrice,
                QuantityInStock = productAddRequest.QuantityInStock
            };

            var addedProduct = await _productsRepository.AddProductAsync(product);
            return MapToProductResponse(addedProduct);
        }

        public async Task<ProductResponse?> UpdateProductAsync(ProductUpdateRequest productUpdateRequest)
        {
            var product = await _productsRepository.GetProductByConditionAsync(p => p.ProductID == productUpdateRequest.ProductID);
            if (product == null)
            {
                return null; // Or handle as not found
            }

            // Update properties
            product.ProductName = productUpdateRequest.ProductName;
            product.Category = productUpdateRequest.Category;
            product.UnitPrice = productUpdateRequest.UnitPrice;
            product.QuantityInStock = productUpdateRequest.QuantityInStock;

            var updatedProduct = await _productsRepository.UpdateProductAsync(product);
            return updatedProduct == null ? null : MapToProductResponse(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            return await _productsRepository.DeleteProductAsync(productId);
        }

        // Helper method to map Product entity to ProductResponse DTO
        private static ProductResponse MapToProductResponse(Product product)
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
