using Elyssa.Core.Interfaces;
using Elyssa.Infrastructure.Data;
using Elyssa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elyssa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("deployDatabase"),
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}
