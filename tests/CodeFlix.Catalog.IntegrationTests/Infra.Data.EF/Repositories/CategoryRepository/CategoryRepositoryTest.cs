

using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;


[Collection(nameof(CategoryRepositoryTestFixture))]

public class CategoryRepositoryTest : IDisposable
{

    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(InsertCategory))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]

    public async Task InsertCategory()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true))
            .Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);


    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]

    public async Task GetCategory()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.
            CategoryRepository(_fixture.CreateDbContext(true));


        var dbCategory = await categoryRepository
            .Get(exampleCategory.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }


    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]

    public async Task GetThrowIfNotFound()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleId = Guid.NewGuid();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);


        var task = async () => await categoryRepository
            .Get(exampleId, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
              .WithMessage($"Category '{exampleId}' not found.");

    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]

    public async Task UpdateCategory()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValues = _fixture.GetExampleCategory();

        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository
            .Update(exampleCategory, CancellationToken.None);

        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true))
            .Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]

    public async Task DeleteCategory()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetExampleCategory();

        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository
            .Delete(exampleCategory, CancellationToken.None);

        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await (_fixture.CreateDbContext(true))
            .Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().BeNull();
        dbCategory.Should().NotBe(exampleCategory);

    }

    [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]
    public async Task SearchReturnListAndTotal()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();


        var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var output = await categoryRepository
                .Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);


        foreach (DomainEntity.Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
                );

            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);


        }

    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]

    public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);


        var output = await categoryRepository
            .Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Total.Should().Be(0);
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Items.Should().HaveCount(0);


    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]

    public async Task SearchReturnsPaginated(
            int quantityCategoryToGenerate,
            int page,
            int perPage,
            int expectedTotalItems)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();


        var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoryToGenerate);
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        var output = await categoryRepository
            .Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Items.Should().HaveCount(expectedTotalItems);


        foreach (DomainEntity.Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );

            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }



    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repository")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Science Fiction", 1, 5, 4, 4)]
    [InlineData("Science Fiction", 1, 2, 2, 4)]
    [InlineData("Science Fiction", 2, 3, 1, 4)]
    [InlineData("Not found", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]

    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems)
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(new List<string>()
        {
            "Action",
            "Adventure",
            "Comedy",
            "Drama",
            "Horror",
            "Horror - Comedy",
            "Horror - Robots",
            "Romance",
            "Science Fiction - IA",
            "Science Fiction - Space",
            "Science Fiction - Robots",
            "Science Fiction - Future",

        });
        await dbContext.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        var output = await categoryRepository
            .Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().NotBeNull();
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);


        foreach (DomainEntity.Category outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                category => category.Id == outputItem.Id
            );

            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }

    }

    public void Dispose()
    {
        _fixture.ClearDatabase();
    }
}
