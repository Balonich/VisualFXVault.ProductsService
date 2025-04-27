using DataAccessLayer.Database;
using DataAccessLayer.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;

        var connectionString = connectionStringTemplate
            .Replace("$SQLSERVER_HOST", Environment.GetEnvironmentVariable("SQLSERVER_HOST"))
            .Replace("$SQLSERVER_USER", Environment.GetEnvironmentVariable("SQLSERVER_USER"))
            .Replace("$SQLSERVER_PASSWORD", Environment.GetEnvironmentVariable("SQLSERVER_PASSWORD"));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}