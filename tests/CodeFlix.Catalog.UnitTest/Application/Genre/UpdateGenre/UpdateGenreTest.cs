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

    [Fact(DisplayName = nameof(UpdateGenreAddCategoriesIds))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task UpdateGenreAddCategoriesIds()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleGenre = _fixture.GetExampleGenre();
        var listIdsCategories = _fixture.GetRandomIdList();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
                           It.IsAny<List<Guid>>(),
                           It.IsAny<CancellationToken>()))
            .ReturnsAsync(listIdsCategories);

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            exampleGenre.Name,
            exampleGenre.IsActive,
            listIdsCategories
            );

        GenreModelOutput output = await useCase
            .Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be((bool)input!.IsActive);
        output.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        output.Categories.Should().NotBeNull();
        output.Categories.Should().HaveCount(listIdsCategories.Count);

        listIdsCategories.ForEach(id =>
        {
            output.Categories.Should().Contain(c => c.Id == id);
        });

        genreRepositoryMock.Verify(r =>
            r.Update(
                It.IsAny<DomainEntity.Genre>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

    }


    [Fact(DisplayName = nameof(UpdateGenreReplacingCategoriesIds))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task UpdateGenreReplacingCategoriesIds()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();

        var listIdsCategories = _fixture.GetRandomIdList();

        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: _fixture.GetRandomIdList());

        var newName = _fixture.GetValidGenreName();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
                           It.IsAny<List<Guid>>(),
                           It.IsAny<CancellationToken>()))
            .ReturnsAsync(listIdsCategories);

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            newName,
            exampleGenre.IsActive,
            listIdsCategories
        );

        GenreModelOutput output = await useCase
            .Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newName);
        output.IsActive.Should().Be((bool)input!.IsActive);
        output.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        output.Categories.Should().NotBeNull();
        output.Categories.Should().HaveCount(listIdsCategories.Count);

        listIdsCategories.ForEach(id =>
        {
            output.Categories.Should().Contain(c => c.Id == id);
        });

        genreRepositoryMock.Verify(r =>
                r.Update(
                    It.IsAny<DomainEntity.Genre>(),
                    It.IsAny<CancellationToken>()),
            Times.Once);

    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task ThrowWhenCategoryNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();


        var listCategoryRepositoryMock = _fixture.GetRandomIdList(10);
        var listIdsReturnedCategories = listCategoryRepositoryMock
            .GetRange(0, listCategoryRepositoryMock.Count - 2);
        var listIdsNotReturnedCategories = listCategoryRepositoryMock
            .GetRange(listCategoryRepositoryMock.Count - 2, 2);



        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: _fixture.GetRandomIdList());

        var newName = _fixture.GetValidGenreName();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(listIdsReturnedCategories);

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            newName,
            exampleGenre.IsActive,
            listCategoryRepositoryMock
        );

        var action = async () => await useCase
            .Handle(input, CancellationToken.None);

        var idsNotFoundString = string.Join(",", listIdsNotReturnedCategories);
        await action.Should().ThrowAsync<RelatedAggregateException>()
            .WithMessage($"Related category id (or ids) not found: {idsNotFoundString}");

    }



    [Fact(DisplayName = nameof(UpdateGenreWithoutCategoriesIds))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task UpdateGenreWithoutCategoriesIds()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();

        var listIdsCategories = _fixture.GetRandomIdList();

        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: listIdsCategories);

        var newName = _fixture.GetValidGenreName();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            newName,
            exampleGenre.IsActive);

        GenreModelOutput output = await useCase
            .Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newName);
        output.IsActive.Should().Be((bool)input!.IsActive);
        output.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        output.Categories.Should().NotBeNull();
        output.Categories.Should().HaveCount(listIdsCategories.Count);

        listIdsCategories.ForEach(id =>
        {
            output.Categories.Should().Contain(c => c.Id == id);
        });

        genreRepositoryMock.Verify(r =>
                r.Update(
                    It.IsAny<DomainEntity.Genre>(),
                    It.IsAny<CancellationToken>()),
            Times.Once);

    }

    [Fact(DisplayName = nameof(UpdateGenreWithEmptyCategoriesIds))]
    [Trait("Application ", "Update Genre - Use Cases")]

    public async Task UpdateGenreWithEmptyCategoriesIds()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();

        var listIdsCategories = _fixture.GetRandomIdList();

        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: listIdsCategories);

        var newName = _fixture.GetValidGenreName();

        genreRepositoryMock.Setup(r => r.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleGenre);

        var useCase = new UseCase.UpdateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object
        );

        var input = new UseCase.UpdateGenreInput(
            exampleGenre.Id,
            newName,
            exampleGenre.IsActive,
            new List<Guid>());

        GenreModelOutput output = await useCase
            .Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(newName);
        output.IsActive.Should().Be((bool)input!.IsActive);
        output.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        output.Categories.Should().NotBeNull();
        output.Categories.Should().HaveCount(0);

        genreRepositoryMock.Verify(r =>
                r.Update(
                    It.IsAny<DomainEntity.Genre>(),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWork.Verify(r =>
                           r.Commit(It.IsAny<CancellationToken>()),
                       Times.Once);

    }
}
