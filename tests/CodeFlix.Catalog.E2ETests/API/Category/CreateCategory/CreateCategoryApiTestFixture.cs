
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.E2ETests.API.Category.Common;

namespace CodeFlix.Catalog.E2ETests.API.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTestFixtureCollection :
    ICollectionFixture<CreateCategoryApiTestFixture>
{ }
public class CreateCategoryApiTestFixture : CategoryBaseFixture
{
    public CreateCategoryInput GetCreateCategoryInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
            );
}
