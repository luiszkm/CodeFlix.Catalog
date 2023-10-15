
using CodeFlix.Catalog.Api.ApiModels.Category;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.E2ETests.API.Category.Common;

namespace CodeFlix.Catalog.E2ETests.API.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture> { }
public class UpdateCategoryApiTestFixture : CategoryBaseFixture
{

    public UpdateCategoryApiInput GetExampleInput(Guid? id = null)
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
            );
}
