using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using FluentValidation;

namespace APILayer.Extensions;

public static class ProductEndpoints
{
    public static void MapProducts(this WebApplication app)
    {
        var productGroup = app.MapGroup("/api/products").WithTags("Products");
        GetAllProducts(productGroup);
        GetProduct(productGroup);
        GetProductsBySearchQuery(productGroup);
        AddProduct(productGroup);
        UpdateProduct(productGroup);
        DeleteProduct(productGroup);
    }

    private static void GetAllProducts(RouteGroupBuilder productGroup)
    {
        productGroup.MapGet("/", async (ProductService productService) =>
        {
            var products = await productService.GetAllProductsAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .WithDescription("Get all products")
        .WithOpenApi();
    }

    private static void GetProduct(RouteGroupBuilder productGroup)
    {
        productGroup.MapGet("/search/product-id/{productId:guid}", async (Guid productId, ProductService productService) =>
        {
            var product = await productService.GetProductByIdAsync(productId);
            return product != null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProduct")
        .WithDescription("Get a product by ID")
        .WithOpenApi();
    }

    private static void GetProductsBySearchQuery(RouteGroupBuilder productGroup)
    {
        productGroup.MapGet("/search/{searchQuery}", async (string searchQuery, ProductService productService) =>
        {
            var products = await productService.GetProductsBySearchQueryAsync(searchQuery);
            return Results.Ok(products);
        })
        .WithName("GetProductsBySearchQuery")
        .WithDescription("Get products by search query")
        .WithOpenApi();
    }

    private static void AddProduct(RouteGroupBuilder productGroup)
    {
        productGroup.MapPost("/", async (ProductAddRequestDto productAddRequest, ProductService productService, IValidator<ProductAddRequestDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(productAddRequest);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var product = await productService.AddProductAsync(productAddRequest);

            if (product == null)
            {
                return Results.Problem("Failed to add product");
            }

            return Results.Created($"/api/products/search/product-id/{product.ProductID}", product);
        })
        .WithName("AddProduct")
        .WithDescription("Add a new product")
        .WithOpenApi();
    }

    private static void UpdateProduct(RouteGroupBuilder productGroup)
    {
        productGroup.MapPut("/", async (ProductUpdateRequestDto productUpdateRequest, ProductService productService, IValidator<ProductUpdateRequestDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(productUpdateRequest);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var product = await productService.UpdateProductAsync(productUpdateRequest);

            return product != null ? Results.Ok(product) : Results.Problem("Failed to update product");
        })
        .WithName("UpdateProduct")
        .WithDescription("Update an existing product")
        .WithOpenApi();
    }

    private static void DeleteProduct(RouteGroupBuilder productGroup)
    {
        productGroup.MapDelete("/{productId:guid}", async (Guid productId, ProductService productService) =>
        {
            var result = await productService.DeleteProductAsync(productId);
            return result ? Results.Ok(true) : Results.NotFound();
        })
        .WithName("DeleteProduct")
        .WithDescription("Delete a product by ID")
        .WithOpenApi();
    }
}