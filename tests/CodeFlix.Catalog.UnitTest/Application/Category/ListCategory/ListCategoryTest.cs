
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using Moq;

using UseCase = CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

namespace CodeFlix.Catalog.UnitTest.Application.Category.ListCategory;

[Collection(nameof(ListCategoryTestFixture))]
public class ListCategoryTest
{
    private readonly ListCategoryTestFixture _fixture;

    public ListCategoryTest(ListCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application ", "List Category - UseCase")]

    public async Task List()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var categoriesExampleList = _fixture.GetExampleCategoriesList();

        var input = _fixture.GetExampleInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Category>)categoriesExampleList,
            total: (new Random()).Next(10, 100)
            );



        repositoryMock.Setup(r => r.Search(
            It.Is<SearchInput>(
                si => si.Page == input.Page &&
                      si.PerPage == input.PerPage &&
                      si.Search == input.Search &&
                      si.OrderBy == input.Sort &&
                      si.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);


        var useCase = new UseCase.ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<CategoryModelOutput>)output.Items).ForEach((item) =>
        {
            var repositoryCategory = outputRepositorySearch.Items
                .FirstOrDefault(i => i.Id == item.Id);
            repositoryCategory.Should().NotBeNull();
            item.Id.Should().Be(repositoryCategory.Id);
            item.Name.Should().Be(repositoryCategory.Name);
            item.Description.Should().Be(repositoryCategory.Description);
            item.IsActive.Should().Be(repositoryCategory.IsActive);
            item.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
        });


        repositoryMock.Verify(r => r.Search(
            It.Is<SearchInput>(
                si => si.Page == input.Page &&
                      si.PerPage == input.PerPage &&
                      si.Search == input.Search &&
                      si.OrderBy == input.Sort &&
                      si.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

    }


    [Theory(DisplayName = nameof(ListCategoriesWithoutAllParams))]
    [Trait("Application ", "List Category - UseCase")]
    [MemberData(
        nameof(ListCategoriesTestDataGenerator.GetInputsWothoutAllParameter),
        parameters: 12,
        MemberType = typeof(ListCategoriesTestDataGenerator))]

    public async Task ListCategoriesWithoutAllParams(UseCase.ListCategoriesInput input)
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var categoriesExampleList = _fixture.GetExampleCategoriesList();


        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<DomainEntity.Category>)categoriesExampleList,
            total: (new Random()).Next(10, 100)
            );



        repositoryMock.Setup(r => r.Search(
            It.Is<SearchInput>(
                si => si.Page == input.Page &&
                      si.PerPage == input.PerPage &&
                      si.Search == input.Search &&
                      si.OrderBy == input.Sort &&
                      si.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);


        var useCase = new UseCase.ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<CategoryModelOutput>)output.Items).ForEach((item) =>
        {
            var repositoryCategory = outputRepositorySearch.Items
                .FirstOrDefault(i => i.Id == item.Id);
            repositoryCategory.Should().NotBeNull();
            item.Id.Should().Be(repositoryCategory.Id);
            item.Name.Should().Be(repositoryCategory.Name);
            item.Description.Should().Be(repositoryCategory.Description);
            item.IsActive.Should().Be(repositoryCategory.IsActive);
            item.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
        });


        repositoryMock.Verify(r => r.Search(
            It.Is<SearchInput>(
                si => si.Page == input.Page &&
                      si.PerPage == input.PerPage &&
                      si.Search == input.Search &&
                      si.OrderBy == input.Sort &&
                      si.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

    }


    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application ", "List Category - UseCase")]

    public async Task ListOkWhenEmpty()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();

        var input = _fixture.GetExampleInput();

        var outputRepositorySearch = new SearchOutput<DomainEntity.Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (new List<DomainEntity.Category>().AsReadOnly()),
            total: 0
            );

        repositoryMock.Setup(r => r.Search(
            It.Is<SearchInput>(
                si => si.Page == input.Page &&
                      si.PerPage == input.PerPage &&
                      si.Search == input.Search &&
                      si.OrderBy == input.Sort &&
                      si.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);


        var useCase = new UseCase.ListCategories(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);


        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);



        repositoryMock.Verify(r => r.Search(
            It.Is<SearchInput>(
                si => si.Page == input.Page &&
                      si.PerPage == input.PerPage &&
                      si.Search == input.Search &&
                      si.OrderBy == input.Sort &&
                      si.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        ), Times.Once);

    }
}
