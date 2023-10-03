﻿
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
