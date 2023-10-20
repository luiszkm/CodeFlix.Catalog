

using System.Runtime.InteropServices;
using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using CodeFlix.Catalog.Domain.Domain.Exceptions;
using UseCase = CodeFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.Create;
[Collection(nameof(CreateGenreTestFixture))]
public class CreateGenreTest
{

    private readonly CreateGenreTestFixture _fixture;

    public CreateGenreTest(CreateGenreTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateGenre))]
    [Trait("Application ", "CreateGenre- Use Cases")]

    public async Task CreateGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new UseCase.CreateGenre(
                       genreRepositoryMock.Object,
                       unitOfWork.Object,
                       categoryRepositoryMock.Object);

        var input = _fixture.GetExampleInput();

        var output = await useCase
            .Handle(input, CancellationToken.None);

        genreRepositoryMock.Verify(r => r.Insert(
            It.IsAny<DomainEntity.Genre>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(r => r.Commit(
                       It.IsAny<CancellationToken>()),
                       Times.Once);
        var verifyDate = DateTime.Now.AddMilliseconds(1);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Categories.Should().BeEmpty();
        (output.CreatedAt - verifyDate).Should().BeLessThan(TimeSpan.FromSeconds(1));
        (output.CreatedAt < verifyDate).Should().BeTrue();

    }
    [Fact(DisplayName = nameof(CreateGenreWithRelatedCategories))]
    [Trait("Application ", "CreateGenre- Use Cases")]

    public async Task CreateGenreWithRelatedCategories()
    {
        var input = _fixture.GetExampleInputWithCategories();

        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        categoryRepositoryMock.Setup(
                x => x.GetIdsListByIds(
                    It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync((IReadOnlyList<Guid>)input.CategoriesId!);
        var useCase = new UseCase.CreateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object);


        var output = await useCase
            .Handle(input, CancellationToken.None);

        genreRepositoryMock.Verify(r => r.Insert(
                It.IsAny<DomainEntity.Genre>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        unitOfWork.Verify(r => r.Commit(
                It.IsAny<CancellationToken>()),
            Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.Categories.Should().NotBeEmpty();
        output.Categories.Count.Should().Be(input.CategoriesId!.Count);
        input.CategoriesId.ForEach(id =>
            output.Categories
                .Any(c => c.Id == id)
                .Should()
                .BeTrue());
    }

    [Fact(DisplayName = nameof(CreateThrowWhenRelatedCategoryNotFound))]
    [Trait("Application ", "CreateGenre- Use Cases")]

    public async Task CreateThrowWhenRelatedCategoryNotFound()
    {
        var input = _fixture.GetExampleInputWithCategories();
        var exampleGuid = input.CategoriesId!.First();
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

        categoryRepositoryMock.Setup(
            x => x.GetIdsListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(
            (IReadOnlyList<Guid>)input.CategoriesId!
                .FindAll(id => id != exampleGuid));

        var useCase = new UseCase.CreateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object);

        var action = async () => await useCase
            .Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>()
             .WithMessage($"Related category id (or ids) not found: {exampleGuid}.");
        categoryRepositoryMock.Verify(r => r.GetIdsListByIds(
                It.IsAny<List<Guid>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }



    [Theory(DisplayName = nameof(ThrowWhenNameIsInvalid))]
    [Trait("Application ", "CreateGenre- Use Cases")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]


    public async Task ThrowWhenNameIsInvalid(string name)
    {
        var input = _fixture.GetExampleInput(name);

        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var unitOfWork = _fixture.GetUnitOfWorkMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

        var useCase = new UseCase.CreateGenre(
            genreRepositoryMock.Object,
            unitOfWork.Object,
            categoryRepositoryMock.Object);


        var action = async () => await useCase
            .Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<EntityValidationException>()
            .WithMessage($"Name should not be empty or null");

    }




}
