using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.Infra.Data.EF.Repository;

namespace CodeFlix.Catalog.Api.Configurations;

public static class UseCaseConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateCategory).Assembly));
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
