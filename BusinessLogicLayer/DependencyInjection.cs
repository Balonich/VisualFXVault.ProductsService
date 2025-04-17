using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IValidator<ProductAddRequest>, ProductAddRequestValidator>();
            services.AddScoped<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();
            return services;
        }
    }
}