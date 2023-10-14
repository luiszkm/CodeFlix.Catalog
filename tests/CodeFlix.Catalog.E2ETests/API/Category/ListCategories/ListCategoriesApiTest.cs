using System.Net;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.E2ETests.Extensions;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

namespace CodeFlix.Catalog.E2ETests.API.Category.ListCategories;

[Collection(nameof(ListCategoriesApiTestFixture))]
public class ListCategoriesApiTest : IDisposable
{
    private readonly ListCategoriesApiTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public ListCategoriesApiTest(
        ListCategoriesApiTestFixture fixture,
        ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }


    [Fact(DisplayName = nameof(ListCategoriesByDefault))]
    [Trait("E2E/API", "Category/List - Endpoints")]

    public async Task ListCategoriesByDefault()
    {
        var defaultPerPage = 15;
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories");

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(defaultPerPage);
        output.PerPage.Should().Be(defaultPerPage);
        output.Page.Should().Be(1);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var expectedItem = exampleCategoriesList.First(x => x.Id == outputItem.Id);
            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }


    [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ItemsEmptyWhenPersistenceEmpty()
    {
        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories");
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Total.Should().Be(0);
        output.Items.Should().BeEmpty();

    }

    [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    public async Task ListCategoriesAndTotal()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var input = new ListCategoriesInput(page: 1, perPage: 5);
        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(input.PerPage);
        output.PerPage.Should().Be(input.PerPage);
        output.Page.Should().Be(1);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var expectedItem = exampleCategoriesList.First(x => x.Id == outputItem.Id);
            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }

    [Theory(DisplayName = nameof(ListCategoriesPaginated))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task ListCategoriesPaginated(
        int quantityCategories,
        int page,
        int perPage,
        int expectedCount
        )
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategories);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var input = new ListCategoriesInput(page, perPage);
        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(expectedCount);
        output.PerPage.Should().Be(input.PerPage);
        output.Page.Should().Be(input.Page);

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var expectedItem = exampleCategoriesList.First(x => x.Id == outputItem.Id);
            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }


    [Theory(DisplayName = nameof(ListCategoriesSearchByText))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task ListCategoriesSearchByText(
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
        var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(categoryNamesList);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var input = new ListCategoriesInput(page, perPage, search);

        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output.PerPage.Should().Be(input.PerPage);
        output.Page.Should().Be(input.Page);
        output!.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);


        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var expectedItem = exampleCategoriesList.First(x => x.Id == outputItem.Id);
            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.TrimMilliseconds().Should().Be(expectedItem.CreatedAt.TrimMilliseconds());
        }
    }

    [Theory(DisplayName = nameof(ListCategoriesOrdered))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]


    public async Task ListCategoriesOrdered(
        string orderBy,
        string order)
    {

        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new ListCategoriesInput(
            page: 1,
            perPage: 20,
            sort: orderBy,
            dir: inputOrder
        );

        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.PerPage.Should().Be(input.PerPage);
        output.Page.Should().Be(input.Page);
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);

        var expectedOrderedList = _fixture.CloneCategoriesListOrder(
            exampleCategoriesList,
            input.Sort,
            input.Dir);

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
        }
    }

    [Theory(DisplayName = nameof(ListCategoriesOrderedByDate))]
    [Trait("EndToEnd/API", "Category/List - Endpoints")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]

    public async Task ListCategoriesOrderedByDate(
        string orderBy,
        string order)
    {

        var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new ListCategoriesInput(
            page: 1,
            perPage: 20,
            sort: orderBy,
            dir: inputOrder
        );

        var (response, output) = await _fixture.
            ApiClient.Get<ListCategoriesOutput>(
                "/categories", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.PerPage.Should().Be(input.PerPage);
        output.Page.Should().Be(input.Page);
        output!.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);

        DateTime? lastItemDate = null;

        foreach (CategoryModelOutput outputItem in output.Items)
        {
            var expectedItem = exampleCategoriesList.First(x => x.Id == outputItem.Id);
            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.TrimMilliseconds().Should()
                .Be(expectedItem.CreatedAt.TrimMilliseconds());

            if (lastItemDate != null)
            {
                if (order == "asc") Assert.True(outputItem.CreatedAt >= lastItemDate);
                else Assert.True(outputItem.CreatedAt <= lastItemDate);
            }
            lastItemDate = outputItem.CreatedAt;
        }

    }
    public void Dispose()
        => _fixture.CleanPersistence();
}
