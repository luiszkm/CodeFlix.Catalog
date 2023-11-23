using Bogus;
using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Base;
public class BaseFixture
{
    public BaseFixture()
        => Faker = new Faker("pt_BR");

    protected Faker Faker { get; set; }
    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription =
            Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
            categoryDescription =
                categoryDescription[..10_000];
        return categoryDescription;
    }

    public bool GetRandomBoolean()
    {
        var random = new Random();
        return random.Next(0, 2) == 1;
    }
    public DomainEntity.Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()

        );
    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
        => Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory())
            .ToList();

    public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new CodeflixCatalogDbContext(
            new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;
    }

    public void ClearDatabase()
        => CreateDbContext().Database.EnsureDeleted();

}
