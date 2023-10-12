using System.Net;
using CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace CodeFlix.Catalog.E2ETests.API.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest
{
    private readonly CreateCategoryApiTestFixture _fixture;
    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("E2E/API", "Category - Endpoints")]

    public async Task CreateCategory()
    {
        var input = _fixture.GetExampleCategory();

        var (response, output) = await _fixture.
            ApiClient.Post<CategoryModelOutput>(
            "/categories",
            input);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        var dbCategory = await _fixture.Persistence
          .GetById(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
    }
}
