namespace CodeFlix.Catalog.UnitTest.Application.Category.DeleteCategory;

using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using Moq;
using UseCases = CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Category", "Delete Category - UseCase")]

    public async Task DeleteCategory()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryExample = _fixture.GetValidCategory();

        repositoryMock.Setup(repositoryMock =>
        repositoryMock.Get(
            categoryExample.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(categoryExample);


        var input = new DeleteCategoryInput(categoryExample.Id);
        var useCase = new UseCases.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repositoryMock =>
        repositoryMock.Get(
             categoryExample.Id,
             It.IsAny<CancellationToken>()),
             Times.Once());

        repositoryMock.Verify(repositoryMock =>
      repositoryMock.Delete(
           categoryExample,
           It.IsAny<CancellationToken>()),
           Times.Once());

        unitOfWorkMock.Verify(unitOfWorkMock =>
        unitOfWorkMock.Commit(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotfound))]
    [Trait("Category", "Delete Category - UseCase")]

    public async Task ThrowWhenCategoryNotfound()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleGuid = Guid.NewGuid();

        repositoryMock.Setup(repositoryMock =>
        repositoryMock.Get(
            exampleGuid,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));


        var input = new DeleteCategoryInput(exampleGuid);
        var useCase = new UseCases.DeleteCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var task = async () =>
           await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(repositoryMock =>
        repositoryMock.Get(
             exampleGuid,
             It.IsAny<CancellationToken>()),
             Times.Once());

    }
}
