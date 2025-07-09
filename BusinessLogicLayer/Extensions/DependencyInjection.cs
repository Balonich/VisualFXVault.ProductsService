using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.MessageQueue.Implementations;
using BusinessLogicLayer.MessageQueue.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<IPublisher, Publisher>();

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();

        services.AddAutoMapper(typeof(ProductAddRequestMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductUpdateRequestMappingProfile).Assembly);

        return services;
    }
}