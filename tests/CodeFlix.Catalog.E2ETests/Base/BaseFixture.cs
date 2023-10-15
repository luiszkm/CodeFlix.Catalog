using Bogus;
using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.Extensions.Configuration;

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
            var config = WebAppFactory.Services
                .GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(config);
            _dbConnectionString = ((IConfiguration)config)
                .GetConnectionString("CatalogDb");
        }

        protected Faker Faker { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public ApiClient ApiClient { get; set; }
        public HttpClient HttpClient { get; set; }

        private readonly string _dbConnectionString;
        public CodeflixCatalogDbContext CreateApiDbContext()
        {
            var context = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                    .UseMySql(
                        _dbConnectionString,
                        ServerVersion.AutoDetect(_dbConnectionString))
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