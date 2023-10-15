
using CodeFlix.Catalog.E2ETests.API.Category.Common;

namespace CodeFlix.Catalog.E2ETests.API.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTestCollection : ICollectionFixture<GetCategoryApiTestFixture>
{
}

public class GetCategoryApiTestFixture : CategoryBaseFixture
{
}
