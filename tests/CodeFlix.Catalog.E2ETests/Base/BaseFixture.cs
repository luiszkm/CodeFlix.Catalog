using Bogus;
using CodeFlix.Catalog.Infra.Data.EF;

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
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public ApiClient ApiClient { get; set; }
        public HttpClient HttpClient { get; set; }
        public CodeflixCatalogDbContext CreateApiDbContext()
        {
            var context = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                    .UseInMemoryDatabase("e2e-test-db")
                    .Options
            );

            return context;
        }

        public void CleanPersistence()
        {
            var context = CreateApiDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

    }
}