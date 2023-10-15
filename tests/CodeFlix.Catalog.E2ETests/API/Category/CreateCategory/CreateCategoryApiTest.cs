using System.Net;
using CodeFlix.Catalog.Api.ApiModels.Response;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.E2ETests.API.Category.CreateCategory;
using Microsoft.AspNetCore.Mvc;

namespace CodeFlix.Catalog.E2ETests.API.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest : IDisposable
{
    private readonly CreateCategoryApiTestFixture _fixture;
    public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("E2E/API", "Category/Create - Endpoints")]

    public async Task CreateCategory()
    {
        var input = _fixture.GetExampleCategory();

        var (response, output) = await _fixture.
            ApiClient.Post<ApiResponse<CategoryModelOutput>>(
            "/categories",
            input);

        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Data!.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be(input.IsActive);
        output.Data.Id.Should().NotBeEmpty();
        output.Data.CreatedAt.Should().NotBeSameDateAs(default);

        var dbCategory = await _fixture
        .Persistence.GetById(output.Data.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should()
            .NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("E2E/API", "Category/Create - Endpoints")]
    [MemberData(
        nameof(CreateCategoryApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCategoryApiTestDataGenerator))]
    public async Task ThrowWhenCantInstantiateAggregate(
        CreateCategoryInput input,
        string expectedMessage)

    {
        var (response, output) = await _fixture.
            ApiClient.Post<ProblemDetails>(
                "/categories",
                input);

        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors occurred.");
        output!.Detail.Should().Be(expectedMessage);
        output.Type.Should().Be("UnProcessableEntity");
        output.Status.Should().Be((int)HttpStatusCode.UnprocessableEntity);

    }

    public void Dispose()
        => _fixture.CleanPersistence();
}
