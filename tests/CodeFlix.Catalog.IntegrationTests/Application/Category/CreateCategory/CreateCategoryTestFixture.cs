

using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.IntegrationTests.Application.Category.Common;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixturCollection : ICollectionFixture<CreateCategoryTestFixture>
{
}
public class CreateCategoryTestFixture : categoryuseCasesBaseFixture
{
    public CreateCategoryInput GetInput()
    {
        var category = GetExampleCategory();

        return new CreateCategoryInput
            (category.Name,
            category.Description,
            category.IsActive);
    }
}
