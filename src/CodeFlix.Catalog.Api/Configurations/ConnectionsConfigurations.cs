using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
namespace CodeFlix.Catalog.Api.Configurations;

public static class ConnectionsConfigurations
{
    public static IServiceCollection AddAppConnections(
        this IServiceCollection services)
    {
        services.AddDbConnection();
        services.AddTestConnections();

        return services;
    }

    private static IServiceCollection AddDbConnection(
        this IServiceCollection services)
    {
        services.AddDbContext<CodeflixCatalogDbContext>(
            options => options.UseInMemoryDatabase("InMemory-DSV-Database"));
        return services;
    }
    public static IServiceCollection AddTestConnections(
        this IServiceCollection services)
    {
        services.AddDbContext<CodeflixCatalogDbContext>(
            options => options.UseInMemoryDatabase("e2e-tests-db"));

        return services;
    }
}
