using System.Linq.Expressions;
using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Products;

namespace BusinessLogicLayer.Services;

public class ProductService
{
    private readonly IProductsRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductsRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetProductsAsync();
        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProductsBySearchQueryAsync(string searchQuery)
    {
        Expression<Func<Product, bool>> predicate = product =>
            product.ProductName.Contains(searchQuery) || product.Category.Contains(searchQuery);

        var products = await _productRepository.GetProductsByConditionAsync(predicate);

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepository.GetProductAsync(productId);

        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> AddProductAsync(ProductAddRequestDto productAddRequest)
    {
        var product = _mapper.Map<Product>(productAddRequest);

        product = await _productRepository.AddProductAsync(product);

        if (product == null)
        {
            throw new Exception("Failed to create product.");
        }

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto?> UpdateProductAsync(ProductUpdateRequestDto productUpdateRequest)
    {
        var product = _mapper.Map<Product>(productUpdateRequest);

        product = await _productRepository.UpdateProductAsync(product);

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        return await _productRepository.DeleteProductAsync(productId);
    }
}