
using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFlix.Catalog.E2ETests.Base;
public class CustomWebApplicationFactory<TStartup>
: WebApplicationFactory<TStartup>
where TStartup : class
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("E2ETest");
        builder.ConfigureServices(service =>
        {
            var serviceProvider = service.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider
                    .GetService<CodeflixCatalogDbContext>();
                ArgumentNullException.ThrowIfNull(context, nameof(context));
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
        });
        base.ConfigureWebHost(builder);
    }
}
