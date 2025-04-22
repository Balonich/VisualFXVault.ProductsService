using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Products;

namespace BusinessLogicLayer.Services;

public class ProductService
{
    private readonly IProductsRepository _productRepository;

    public ProductService(IProductsRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetProductsAsync();
    }
    
    public async Task<IEnumerable<Product>> GetProductsByConditionAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _productRepository.GetProductsByConditionAsync(predicate);
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _productRepository.GetProductAsync(productId);
    }

    public async Task AddProductAsync(Product product)
    {
        await _productRepository.AddProductAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateProductAsync(product);
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        await _productRepository.DeleteProductAsync(productId);
    }
}