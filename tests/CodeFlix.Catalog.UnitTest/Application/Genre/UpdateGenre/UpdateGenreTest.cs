using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using CodeFlix.Catalog.Domain.Domain.Exceptions;
using UseCase = CodeFlix.Catalog.Application.UseCases.Genre.UpdateGenre;

using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.UpdateGenre;
[Collection(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTest
{
    private readonly UpdateGenreTestFixture _fixture;

    public UpdateGenreTest(UpdateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task UpdateGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var exampleGenre = _fixture.GetExampleGenre();
        genreRepositoryMock.Setup(r => r.Get(
                           It.Is<Guid>(x => x == exampleGenre.Id),
                           It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);
        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            _fixture.GetCategoryRepositoryMock().Object
           );

        var input = _fixture.GetExampleInput(exampleGenre.Id);

        GenreModelOutput output = await useCase
            .Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be((bool)input!.IsActive);
        output.CreatedAt.Should().Be(exampleGenre.CreatedAt);

    }
    [Theory(DisplayName = nameof(UpdateGenreOnlyName))]
    [Trait("Application ", "Update Genre - Use Cases")]
    [InlineData(true)]
    [InlineData(false)]

    public async Task UpdateGenreOnlyName(bool isActive)
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();

        var exampleGenre = _fixture.GetExampleGenre(isActive);
        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);
        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            _fixture.GetValidGenreName());

        GenreModelOutput output = await useCase
            .Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(isActive);
        output.CreatedAt.Should().Be(exampleGenre.CreatedAt);

    }

    [Fact(DisplayName = nameof(ThrowWhenGenreNotFound))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task ThrowWhenGenreNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();

        var exampleId = Guid.NewGuid();
        genreRepositoryMock.Setup(r => r.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Genre not found '{exampleId}"));

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = _fixture.GetExampleInput(exampleId);

        var action = async () => await useCase
            .Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Genre not found '{exampleId}");

    }


    [Theory(DisplayName = nameof(ThrowWhenGenreNotFound))]
    [Trait("Application ", "Update Genre - Use Cases")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]

    public async Task ThrowWhenNameIsInvalid(string? name)
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var exampleGenre = _fixture.GetExampleGenre();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            _fixture.GetCategoryRepositoryMock().Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            name!,
            _fixture.GetRandomBoolean());

        var action = async () => await useCase
            .Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage("Name should not be empty or null");

    }

}
