using System.Linq.Expressions;
using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Validators;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Products;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessLogicLayer.Services;

public class ProductService
{
    private readonly IValidator<ProductAddRequestDto> _productAddRequestValidator;
    private readonly IValidator<ProductUpdateRequestDto> _productUpdateRequestValidator;
    private readonly IProductsRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(
        IValidator<ProductAddRequestDto> productAddRequestValidator,
        IValidator<ProductUpdateRequestDto> productUpdateRequestValidator,
        IProductsRepository productRepository,
        IMapper mapper)
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
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

    public async Task<ProductResponseDto?> AddProductAsync(ProductAddRequestDto productAddRequest)
    {
        if (productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        var validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        var productInput = _mapper.Map<Product>(productAddRequest);
        var addedProduct = await _productRepository.AddProductAsync(productInput);

        if (addedProduct == null)
        {
            return null;
        }

        var addedProductResponse = _mapper.Map<ProductResponseDto>(addedProduct);

        return addedProductResponse;
    }

    public async Task<ProductResponseDto?> UpdateProductAsync(ProductUpdateRequestDto productUpdateRequest)
    {
        var existingProduct = await _productRepository.GetProductByConditionAsync(temp => temp.ProductID == productUpdateRequest.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid Product ID");
        }

        var validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }


        var product = _mapper.Map<Product>(productUpdateRequest);

        var updatedProduct = await _productRepository.UpdateProductAsync(product);

        var updatedProductResponse = _mapper.Map<ProductResponseDto>(updatedProduct);

        return updatedProductResponse;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var existingProduct = await _productRepository.GetProductByConditionAsync(temp => temp.ProductID == productId);

        if (existingProduct == null)
        {
            return false;
        }

        return await _productRepository.DeleteProductAsync(productId);
    }
}