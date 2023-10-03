
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.Infra.Data.EF;
using CodeFlix.Catalog.IntegrationTests.Base;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;


[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }
public class CategoryRepositoryTestFixture : BaseFixture
{
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

    public List<DomainEntity.Category> GetExampleCategoriesListWithNames(List<string> names)
        => names.Select(name =>
        {
            var category = GetExampleCategory();
            category.Update(name);
            return category;
        }).ToList();

    public List<DomainEntity.Category> CloneCategoriesListOrder(
        List<DomainEntity.Category> categoriesList,
        string orderBy,
        SearchOrder order)
    {
        var listClone = new List<DomainEntity.Category>(categoriesList);
        var orderedEnumerable = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdAt", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name),
        };

        return orderedEnumerable.ToList();
    }


    public CodeflixCatalogDbContext CreateDbContext(bool preserverData = false)
    {
        var dbContext = new CodeflixCatalogDbContext(
            new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase("Integration-testes-db")
                .Options
                );

        if (preserverData) dbContext.Database.EnsureCreated();

        return dbContext;
    }

    public void ClearDatabase()
    => CreateDbContext().Database.EnsureDeleted();
}
