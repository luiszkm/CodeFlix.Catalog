using Microsoft.AspNetCore.Mvc;
using System.Net;
using CodeFlix.Catalog.Api.ApiModels.Category;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.E2ETests.API.Category.CreateCategory;
using CodeFlix.Catalog.E2ETests.Extensions;
using CodeFlix.Catalog.Api.ApiModels.Response;

namespace CodeFlix.Catalog.E2ETests.API.Category.UpdateCategory;



[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest : IDisposable
{
    private readonly UpdateCategoryApiTestFixture _fixture;

    public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task UpdateCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var exampleInput = _fixture.GetExampleInput(exampleCategory.Id);
        var input = _fixture.GetExampleInput();

        var (response, output) = await _fixture.
            ApiClient.Put<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Data!.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be((bool)input.IsActive!);
        output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());

        var dbCategory = await _fixture.Persistence.GetById(output.Data.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());


    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyName()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var input = new UpdateCategoryApiInput(
            _fixture.GetValidCategoryName());


        var (response, output) = await _fixture.
            ApiClient.Put<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Data!.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(exampleCategory.Description);
        output.Data.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());

        var dbCategory = await _fixture.Persistence.GetById(output.Data.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());


    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyNameAndDescription))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyNameAndDescription()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var input = new UpdateCategoryApiInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription());
        var (response, output) = await _fixture.
            ApiClient.Put<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Data!.Id.Should().Be(exampleCategory.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());

        var dbCategory = await _fixture.Persistence.GetById(output.Data.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.Data.CreatedAt.TrimMilliseconds().Should().Be(exampleCategory.CreatedAt.TrimMilliseconds());
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFoundCategory))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task ThrowWhenNotFoundCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = _fixture.GetExampleCategory();
        var randomGuid = Guid.NewGuid();
        var input = _fixture.GetExampleInput();

        var (response, output) = await _fixture.
            ApiClient.Put<ProblemDetails>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output!.Detail.Should().Be($"Category '{exampleCategory.Id}' not found.");
        output.Type.Should().Be("Not Found");
        output.Status.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Theory(DisplayName = nameof(ThrowWhencantInstantiateCategory))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async Task ThrowWhencantInstantiateCategory(
        UpdateCategoryApiInput input,
        string expectedMessage)

    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[11];
        var (response, output) = await _fixture.
            ApiClient.Put<ProblemDetails>(
                $"/categories/{exampleCategory.Id}", input);

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
