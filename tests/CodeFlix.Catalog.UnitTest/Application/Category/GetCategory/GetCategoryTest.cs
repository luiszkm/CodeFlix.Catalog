using CodeFlix.Catalog.Application.Exceptions;
using Moq;
using UseCases = CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace CodeFlix.Catalog.UnitTest.Application.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;
    public GetCategoryTest(GetCategoryTestFixture ficture)
    {
        _fixture = ficture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - use case")]

    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleCategory = _fixture.GetValidInput();

        repositoryMock.Setup(repositoryMock =>
        repositoryMock.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        var input = new UseCases.GetCategoryInput(exampleCategory.Id);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repositoryMock =>
        repositoryMock.Get(
                       It.IsAny<Guid>(),
                       It.IsAny<CancellationToken>()),
                       Times.Once());

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesExist))]
    [Trait("Application", "GetCategory - use case")]

    public async Task NotFoundExceptionWhenCategoryDoesExist()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(repositoryMock =>
        repositoryMock.Get(
             It.IsAny<Guid>(),
             It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{exampleGuid}'  not found"));

        var input = new UseCases.GetCategoryInput(exampleGuid);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(repositoryMock =>
        repositoryMock.Get(
                  It.IsAny<Guid>(),
                  It.IsAny<CancellationToken>()),
                  Times.Once());

    }
}
