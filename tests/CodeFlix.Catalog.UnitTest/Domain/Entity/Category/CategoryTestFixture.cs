using CodeFlix.Catalog.UnitTest.Common;

namespace CodeFlix.Catalog.UnitTest.Domain.Entity.Category;
public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture() : base()
    { }

    public string GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }
        if (categoryName.Length > 255)
        {
            categoryName = categoryName[..255];
        }
        return categoryName;
    }
    public string GetValidCategoryDescription()
    {
        var categoryDescription = "";
        if (categoryDescription.Length > 10_000)
        {
            categoryDescription = categoryDescription[..10_000];
        }

        return categoryDescription;
    }
    public DomainEntity.Category GetValidCategory()
    => new(
        GetValidCategoryName(),
        GetValidCategoryDescription()
        );
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }