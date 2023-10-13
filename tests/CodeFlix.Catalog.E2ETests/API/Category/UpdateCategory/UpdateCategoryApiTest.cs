using Microsoft.AspNetCore.Mvc;
using System.Net;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.E2ETests.API.Category.CreateCategory;

namespace CodeFlix.Catalog.E2ETests.API.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestFixture))]
public class UpdateCategoryApiTest
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

        var input = _fixture.GetExampleInput(exampleCategory.Id);

        var (response, output) = await _fixture.
            ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

        var dbCategory = await _fixture.Persistence.GetById(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);


    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyName()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var input = new UpdateCategoryInput(
            exampleCategory.Id,
            _fixture.GetValidCategoryName());


        var (response, output) = await _fixture.
            ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

        var dbCategory = await _fixture.Persistence.GetById(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);


    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyNameAndDescription))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyNameAndDescription()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = exampleCategoriesList[10];

        var input = new UpdateCategoryInput(
            exampleCategory.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription());
        var (response, output) = await _fixture.
            ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}", input);

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().NotBeNull();
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

        var dbCategory = await _fixture.Persistence.GetById(output.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFoundCategory))]
    [Trait("E2E/API", "Category/Update - Endpoints")]
    public async Task ThrowWhenNotFoundCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var exampleCategory = _fixture.GetExampleCategory();

        var input = new UpdateCategoryInput(
            exampleCategory.Id,
            _fixture.GetValidCategoryName());

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
        UpdateCategoryInput input,
        string expectedMessage)

    {
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        input.Id = exampleCategoriesList[10].Id;

        var exampleCategory = _fixture.GetExampleCategory();
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
}
