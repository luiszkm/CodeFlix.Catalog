﻿
using CodeFlix.Catalog.IntegrationTests.Base;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection : ICollectionFixture<UnitOfWorkTestFixture> { }
public class UnitOfWorkTestFixture : BaseFixture
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
}
