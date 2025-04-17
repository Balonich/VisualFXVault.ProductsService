using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(Guid productId);
        Task<IEnumerable<ProductResponse>> SearchProductsAsync(string searchString);
        Task<ProductResponse> AddProductAsync(ProductAddRequest request);
        Task UpdateProductAsync(ProductUpdateRequest request);
        Task DeleteProductAsync(Guid productId);
    }
}