using APILayer.Middleware;
using BusinessLogicLayer;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using FluentValidation;
using DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDataAccessServices(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddBusinessServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1");
        c.RoutePrefix = string.Empty;
    });
}

// API Endpoints
app.MapGet("/api/products", async (IProductService service) =>
    await service.GetAllProductsAsync());

app.MapGet("/api/products/search/product-id/{productId}", async (Guid productId, IProductService service) =>
{
    var product = await service.GetProductByIdAsync(productId);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

app.MapGet("/api/products/products/search/{searchString}", async (string searchString, IProductService service) =>
    await service.SearchProductsAsync(searchString));

app.MapPost("/api/products", async (ProductAddRequest request, IProductService service, IValidator<ProductAddRequest> validator) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);
    var product = await service.AddProductAsync(request);
    return Results.Created($"/api/products/search/product-id/{product.ProductID}", product);
});

app.MapPut("/api/products", async (ProductUpdateRequest request, IProductService service, IValidator<ProductUpdateRequest> validator) =>
{
    var validation = await validator.ValidateAsync(request);
    if (!validation.IsValid) return Results.BadRequest(validation.Errors);
    await service.UpdateProductAsync(request);
    return Results.NoContent();
});

app.MapDelete("/api/products/{productId}", async (Guid productId, IProductService service) =>
{
    await service.DeleteProductAsync(productId);
    return Results.NoContent();
});

app.Run();
