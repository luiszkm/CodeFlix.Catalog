using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.Infra.Data.EF.Repository;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

namespace CodeFlix.Catalog.IntegrationTests.Application.Category.ListCategory;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ShouldGetAllCategories))]
    [Trait("Integration/Application", "ListCategories - use-case")]

    public async Task ShouldGetAllCategories()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync();

        var repository = new Repository.CategoryRepository(dbContext);
        var useCase = new UseCase.ListCategories(repository);

        var searchInput = new UseCase.ListCategoriesInput(1, 20);
        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList
                .Find(category => category.Id == outputItem.Id);

            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenEmpty))]
    [Trait("Integration/Application", "ListCategories - use-case")]

    public async Task SearchReturnsEmptyWhenEmpty()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new Repository.CategoryRepository(dbContext);
        var useCase = new UseCase.ListCategories(repository);

        var input = new UseCase.ListCategoriesInput(1, 20);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }


    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/Application", "ListCategories - use-case")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]

    public async Task SearchReturnsPaginated(
        int quantityCategoriesToGenerate,
        int page,
        int perPage,
        int expectedCount
        )
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync();

        var repository = new Repository.CategoryRepository(dbContext);
        var useCase = new UseCase.ListCategories(repository);

        var input = new UseCase.ListCategoriesInput(page, perPage);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(expectedCount);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList
                .Find(category => category.Id == outputItem.Id);

            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }


    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
    )
    {
        var categoryNamesList = new List<string>() {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        };

        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(
            categoryNamesList);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync();

        var repository = new Repository.CategoryRepository(dbContext);
        var useCase = new UseCase.ListCategories(repository);

        var input = new UseCase.ListCategoriesInput(page, perPage, search);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
    }


    [Theory(DisplayName = nameof(SearchOrdered))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(
     string orderBy,
     string order
 )
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var repository = new CategoryRepository(dbContext);
        var useCaseOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var useCase = new UseCase.ListCategories(repository);

        var input = new ListCategoriesInput(1, 20, "", orderBy, useCaseOrder);
        var output = await useCase.Handle(input, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneCategoriesListOrder(
            exampleCategoriesList,
            input.Sort,
            input.Dir
        );

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);
        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var outputItem = output.Items[i];
            var exampleItem = expectedOrderedList[i];
            outputItem.Should().NotBeNull();
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Id.Should().Be(exampleItem.Id);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
}
