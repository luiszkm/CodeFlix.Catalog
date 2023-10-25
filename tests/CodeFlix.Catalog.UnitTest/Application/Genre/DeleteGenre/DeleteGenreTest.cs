
using CodeFlix.Catalog.Application.Exceptions;
using UseCase = CodeFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
using Moq;
using CodeFlix.Catalog.Application.Interfaces;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.DeleteGenre;

[Collection(nameof(DeleteGenreTestFixture))]
public class DeleteGenreTest
{
    private readonly DeleteGenreTestFixture _fixture;


    public DeleteGenreTest(DeleteGenreTestFixture fixture)
        => _fixture = fixture;


    [Fact(DisplayName = nameof(DeleteGenre))]
    [Trait("Application ", "Delete Genre - Use Cases")]

    public async Task DeleteGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var exampleGenre = _fixture.GetExampleGenre();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        var useCase = new UseCase.DeleteGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object);

        var input = new UseCase.DeleteGenreInput(exampleGenre.Id);

        await useCase.Handle(input, CancellationToken.None);

        genreRepositoryMock.Verify(r => r.Delete(
                It.Is<DomainEntity.Genre>(x => x == exampleGenre),
                It.IsAny<CancellationToken>()),
            Times.Once);

        genreRepositoryMock.Verify(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application ", "Delete Genre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();

        var exampleId = Guid.NewGuid();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Genre '{exampleId}' not found"));

        var useCase = new UseCase.DeleteGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object);

        var input = new UseCase.DeleteGenreInput(exampleId);

        var action = async () => await useCase.Handle(input, CancellationToken.None);


        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{exampleId}' not found");

        genreRepositoryMock.Verify(x => x.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
