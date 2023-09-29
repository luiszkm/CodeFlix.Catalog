using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.UnitTest.Common;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Category.DeleteCategory;


[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }
public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{

    public DomainEntity.Category GetValidCategory()
         => new(
                 GetValidCategoryName(),
                 GetValidCategoryDescription(),
                true
                );

}
