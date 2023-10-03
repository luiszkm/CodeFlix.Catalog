

using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;


[Collection(nameof(CategoryRepositoryTestFixture))]

public class CategoryRepositoryTest
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

        var dbCategory = await (_fixture.CreateDbContext())
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

        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext());


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

        var dbCategory = await (_fixture.CreateDbContext())
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

        var dbCategory = await (_fixture.CreateDbContext())
            .Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().BeNull();
        dbCategory.Should().NotBe(exampleCategory);

    }


}
