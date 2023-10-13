using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.E2ETests.API.Category.DeleteCategory;
[Collection(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTest
{
    private readonly DeleteCategoryApiTestFixture _fixture;

    public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("E2E/API", "Category/Delete - Endpoints")]

    public async Task DeleteCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var (response, output) = await _fixture.
            ApiClient.Delete<object>(
                $"/categories/{exampleCategory.Id}");

        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Should().NotBeNull();
        output.Should().BeNull();

        var persistedCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        persistedCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFoundCategory))]
    [Trait("E2E/API", "Category/Delete - Endpoints")]

    public async Task ThrowWhenNotFoundCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = _fixture.GetExampleCategory();

        var (response, output) = await _fixture.
            ApiClient.Delete<ProblemDetails>(
                $"/categories/{exampleCategory.Id}");

        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output!.Detail.Should().Be($"Category '{exampleCategory.Id}' not found.");
        output.Type.Should().Be("Not Found");
        output.Status.Should().Be((int)HttpStatusCode.NotFound);
    }
}
