using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Domain.Exceptions;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Category.UpdateCategory;

using UseCase = CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Category", "Update Category - UseCase")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async Task UpdateCategory(
        DomainEntity.Category categoryExample,
        UseCase.UpdateCategoryInput input
    )
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(repositoryMock => repositoryMock.Get(
                categoryExample.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryExample);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(categoryExample.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.CreatedAt.Should().Be(categoryExample.CreatedAt);

        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Get(
                    categoryExample.Id,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Update(
                    categoryExample,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(unitOfWorkMock =>
                unitOfWorkMock.Commit(
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryDoesNotExist))]
    [Trait("Category", "Update Category - UseCase")]
    public async Task ThrowWhenCategoryDoesNotExist()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();

        repositoryMock.Setup(repositoryMock => repositoryMock.Get(
                input.Id,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"category '{input.Id}' not found"));


        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);


        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Get(
                    input.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Category", "Update Category - UseCase")]
    [MemberData(
               nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
                      parameters: 10,
                      MemberType = typeof(UpdateCategoryDataGenerator))]

    public async Task UpdateCategoryWithoutProvidingIsActive(
        DomainEntity.Category exampleCategory,
        UseCase.UpdateCategoryInput exampleInput)
    {
        var input = new UseCase.UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.Description
        );

        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(repositoryMock => repositoryMock.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleInput.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Update(
                    exampleCategory,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(unitOfWorkMock =>
                unitOfWorkMock.Commit(
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingName))]
    [Trait("Category", "Update Category - UseCase")]
    [MemberData(
             nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate),
                    parameters: 10,
                    MemberType = typeof(UpdateCategoryDataGenerator))]

    public async Task UpdateCategoryWithoutProvidingName(
      DomainEntity.Category exampleCategory,
      UseCase.UpdateCategoryInput exampleInput)
    {
        var input = new UseCase.UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name
        );

        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(repositoryMock => repositoryMock.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleInput.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        repositoryMock.Verify(repositoryMock =>
                repositoryMock.Update(
                    exampleCategory,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(unitOfWorkMock =>
                unitOfWorkMock.Commit(
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Category", "Update Category - UseCase")]
    [MemberData(
        nameof(UpdateCategoryDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateCategoryDataGenerator))]
    public async Task ThrowWhenCantUpdateCategory(
        UseCase.UpdateCategoryInput input,
        string exceptionMessage)
    {
        var exampleCategory = _fixture.GetExampleCategory();
        input.Id = exampleCategory.Id;
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(repositoryMock => repositoryMock.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var task = async () => await useCase.Handle(
            input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);

        repositoryMock.Verify(repositoryMock =>
            repositoryMock.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()), Times.Once);



    }

}