
using CodeFlix.Catalog.IntegrationTests.Application.Category.Common;

namespace CodeFlix.Catalog.IntegrationTests.Application.Category.DeleteCategory;
[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }

public class DeleteCategoryTestFixture : categoryuseCasesBaseFixture
{
}
