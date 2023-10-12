using Bogus;
using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CodeFlix.Catalog.E2ETests.Base
{
    public class BaseFixture
    {
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
        }

        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }
        public HttpClient HttpClient { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                    .UseInMemoryDatabase("e2e-tests-db")
                    .Options
            );

            return context;
        }


    }
}