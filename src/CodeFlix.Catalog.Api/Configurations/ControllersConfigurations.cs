using CodeFlix.Catalog.Api.FiltersExceptions;

namespace CodeFlix.Catalog.Api.Configurations;

public static class ControllersConfigurations
{
    public static IServiceCollection AddControllersConfigurations(
        this IServiceCollection services)
    {
        services.AddControllers(options =>
            options.Filters.Add(typeof(ApiGlobalExceptionFilter)));
        services.AddDocumentation();
        return services;
    }

    private static IServiceCollection AddDocumentation(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static WebApplication UseDocumentation(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
