using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
namespace CodeFlix.Catalog.Api.Configurations;

public static class ConnectionsConfigurations
{
    public static IServiceCollection AddAppConnections(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddDbConnection(config);
        //services.AddTestConnections();

        return services;
    }

    private static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("CatalogDb");
        services.AddDbContext<CodeflixCatalogDbContext>(
            options => options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString)));
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
