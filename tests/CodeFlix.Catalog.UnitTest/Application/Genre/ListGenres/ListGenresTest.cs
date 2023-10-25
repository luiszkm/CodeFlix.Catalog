using Castle.DynamicProxy;
using UseCase = CodeFlix.Catalog.Application.UseCases.Genre.ListGenres;


using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.ListGenres;

[Collection(nameof(ListGenresTestFixture))]
public class ListGenresTest
{

    private readonly ListGenresTestFixture _fixture;

    public ListGenresTest(ListGenresTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("Application ", "List Genre - Use Cases")]
    public async Task ListGenres()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var genresListExample = _fixture.GetGenresListExample();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();


        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: _fixture.GetRandomIdList());
        var input = _fixture.GetExampleInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Genre>)genresListExample,
            total: new Random().Next(50, 200)
        );

        genreRepositoryMock.Setup(r => r.Search(
                It.IsAny<SearchInput>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListGenres(
            genreRepositoryMock.Object);


        UseCase.ListGenresOutput output =
            await useCase.Handle(input, CancellationToken.None);

        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<GenreModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryGenre = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            repositoryGenre.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryGenre!.Name);
            outputItem.IsActive.Should().Be(repositoryGenre.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryGenre!.CreatedAt);
            outputItem.Categories.Should()
                .HaveCount(repositoryGenre.Categories.Count);
            foreach (var expectedId in repositoryGenre.Categories)
                outputItem.Categories.Should().Contain(relation => relation.Id == expectedId);
        });

        genreRepositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);



    }


    [Fact(DisplayName = nameof(ListGenresEmpty))]
    [Trait("Application ", "List Genre - Use Cases")]
    public async Task ListGenresEmpty()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var input = _fixture.GetExampleInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Genre>)new List<DomainEntity.Genre>(),
            total: new Random().Next(50, 200)
        );

        genreRepositoryMock.Setup(r => r.Search(
                It.IsAny<SearchInput>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListGenres(
            genreRepositoryMock.Object);

        UseCase.ListGenresOutput output =
            await useCase.Handle(input, CancellationToken.None);

        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(0);

        genreRepositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);



    }

    [Fact(DisplayName = nameof(ListGenresUseDefaultInputValues))]
    [Trait("Application ", "List Genre - Use Cases")]
    public async Task ListGenresUseDefaultInputValues()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

        var input = new UseCase.ListGenreInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Genre>)new List<DomainEntity.Genre>(),
            total: 0
        );

        genreRepositoryMock.Setup(r => r.Search(
                It.IsAny<SearchInput>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListGenres(
            genreRepositoryMock.Object);

        UseCase.ListGenresOutput output =
            await useCase.Handle(input, CancellationToken.None);



        genreRepositoryMock.Verify(
            x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == 1
                    && searchInput.PerPage == 15
                    && searchInput.Search == ""
                    && searchInput.OrderBy == ""
                    && searchInput.Order == SearchOrder.Asc
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);



    }
}
