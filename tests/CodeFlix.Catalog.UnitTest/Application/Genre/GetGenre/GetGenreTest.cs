using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using UseCase = CodeFlix.Catalog.Application.UseCases.Genre.GetGenre;

using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture)
    => _fixture = fixture;


    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Application ", "Get Genre - Use Cases")]
    public async Task GetGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: _fixture.GetRandomIdList());

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        var useCase = new UseCase.GetGenre(
            genreRepositoryMock.Object);

        var input = new UseCase.GetGenreInput(exampleGenre.Id);

        GenreModelOutput output =
            await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(exampleGenre.Name);
        output.IsActive.Should().Be(exampleGenre.IsActive);
        output.Categories.Should().HaveCount(exampleGenre.Categories.Count);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);

        foreach (var expectedId in exampleGenre.Categories)
            output.Categories.Should().Contain(c => c.Id == expectedId);


        genreRepositoryMock.Verify(r => r.Get(
                           It.Is<Guid>(x => x == exampleGenre.Id),
                           It.IsAny<CancellationToken>()),
                       Times.Once);

    }


    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application ", "Get Genre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var exampleId = Guid.NewGuid();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Genre '{exampleId}' not found"));

        var useCase = new UseCase.GetGenre(
            genreRepositoryMock.Object);

        var input = new UseCase.GetGenreInput(exampleId);

        var action = async () => await useCase.Handle(input, CancellationToken.None);


        await action.Should().ThrowAsync<NotFoundException>()
             .WithMessage($"Genre '{exampleId}' not found");

        genreRepositoryMock.Verify(x => x.Get(
                It.Is<Guid>(x => x == exampleId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

}
