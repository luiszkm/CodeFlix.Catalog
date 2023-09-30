
using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.UnitTest.Common;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Category.ListCategory;

[CollectionDefinition(nameof(ListCategoryTestFixture))]
public class ListCategoryTestFixtureCollection :
    ICollectionFixture<ListCategoryTestFixture>
{ }

public class ListCategoryTestFixture : CategoryUseCasesBaseFixture
{


    public DomainEntity.Category GetValidCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public UpdateCategoryInput GetValidInput(Guid? id = null)
         => new(
                id ?? Guid.NewGuid(),
                 GetValidCategoryName(),
                 GetValidCategoryDescription(),
                  GetRandomBoolean()
                );


    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<DomainEntity.Category>();

        for (int i = 0; i < length; i++)
        {
            list.Add(GetValidCategory());
        }

        return list;
    }


    public ListCategoriesInput GetExampleInput()
    {
        var random = new Random();
        var result = new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(1, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
        return result;
    }


}
