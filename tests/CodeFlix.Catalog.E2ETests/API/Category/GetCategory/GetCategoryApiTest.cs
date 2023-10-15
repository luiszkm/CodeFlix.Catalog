

using System.Net;
using CodeFlix.Catalog.Api.ApiModels.Response;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.E2ETests.API.Category.CreateCategory;
using Microsoft.AspNetCore.Mvc;

namespace CodeFlix.Catalog.E2ETests.API.Category.GetCategory;



[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest : IDisposable
{
    private readonly GetCategoryApiTestFixture _fixture;

    public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("E2E/API", "Category/Get - Endpoints")]

    public async Task GetCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var (response, output) = await _fixture.
            ApiClient.Get<ApiResponse<CategoryModelOutput>>(
                               $"/categories/{exampleCategory.Id}");

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Data!.Name.Should().Be(exampleCategory.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.IsActive.Should().Be(exampleCategory.IsActive);
        output.Data.Id.Should().Be(exampleCategory.Id);
        output.Data.CreatedAt.Should().BeSameDateAs(exampleCategory.CreatedAt);
    }


    [Fact(DisplayName = nameof(ThrowWhenNotFoundCategory))]
    [Trait("E2E/API", "Category/Get - Endpoints")]

    public async Task ThrowWhenNotFoundCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = _fixture.GetExampleCategory();

        var (response, output) = await _fixture.
            ApiClient.Get<ProblemDetails>(
                $"/categories/{exampleCategory.Id}");

        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output!.Detail.Should().Be($"Category '{exampleCategory.Id}' not found.");
        output.Type.Should().Be("Not Found");
        output.Status.Should().Be((int)HttpStatusCode.NotFound);
    }
    public void Dispose()
        => _fixture.CleanPersistence();
}
