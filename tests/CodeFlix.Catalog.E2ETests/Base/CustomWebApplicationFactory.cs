


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
        builder.ConfigureServices(service =>
        {
            var dboptions = service.FirstOrDefault(
                   x => x.ServiceType == typeof(
                       DbContextOptions<CodeflixCatalogDbContext>));

            if (dboptions is not null) service.Remove(dboptions);

            service.AddDbContext<CodeflixCatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase("e2e-test-db");
            });

        });
        base.ConfigureWebHost(builder);
    }
}
