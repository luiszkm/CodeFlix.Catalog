using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.UnitTest.Common;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>
{
}
public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{


}
