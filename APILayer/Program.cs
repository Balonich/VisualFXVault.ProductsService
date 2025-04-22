using APILayer.Middleware;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using DataAccessLayer;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.Services.AddDataAccessServices(builder.Configuration);

builder.Services.AddScoped<ProductService>();

builder.Services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseHttpsRedirection();

app.UseExceptionHandlingMiddleware();

var productsGroup = app.MapGroup("/api/products");

productsGroup.MapGet("/", async (ProductService productService) =>
{
    var products = await productService.GetAllProductsAsync();
    return Results.Ok(products);
})
.WithName("GetAllProducts")
.WithOpenApi();

productsGroup.MapGet("/search/product-id/{productId:guid}", async (Guid productId, ProductService productService) =>
{
    var product = await productService.GetProductByIdAsync(productId);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithOpenApi();

productsGroup.MapGet("/search/{searchString}", async (string searchString, ProductService productService) =>
{
    var products = await productService.SearchProductsAsync(searchString);
    return Results.Ok(products);
})
.WithName("SearchProducts")
.WithOpenApi();

productsGroup.MapPost("/", async (ProductAddRequest productRequest, ProductService productService, IValidator<ProductAddRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(productRequest);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var newProduct = await productService.AddProductAsync(productRequest);
    return Results.CreatedAtRoute("GetProductById", new { productId = newProduct.ProductID }, newProduct);
})
.WithName("AddProduct")
.WithOpenApi();

productsGroup.MapPut("/", async (ProductUpdateRequest productRequest, ProductService productService, IValidator<ProductUpdateRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(productRequest);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var updatedProduct = await productService.UpdateProductAsync(productRequest);
    return updatedProduct is not null ? Results.Ok(updatedProduct) : Results.NotFound();
})
.WithName("UpdateProduct")
.WithOpenApi();

productsGroup.MapDelete("/{productId:guid}", async (Guid productId, ProductService productService) =>
{
    var deleted = await productService.DeleteProductAsync(productId);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteProduct")
.WithOpenApi();

app.Run();
