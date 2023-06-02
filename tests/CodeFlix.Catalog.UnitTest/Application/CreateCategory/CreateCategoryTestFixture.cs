

using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.CreateCategory;


[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection
    :ICollectionFixture<CreateCategoryTestFixture>
{

}

public class CreateCategoryTestFixture : BaseFixture
{
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
    public bool GetRandomBoolean()
    {
        return Faker.Random.Bool();
    }

    public CreateCategoryInput GetValidInput()
         => new(
                 GetValidCategoryName(),
                 GetValidCategoryDescription(),
                 GetRandomBoolean()
                );

    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
         => new ();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
         => new ();


}
