
using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository;

[Collection(nameof(GenreRepositoryTestFixture))]
public class GenreRepositoryTest
{
    private readonly GenreRepositoryTestFixture _fixture;

    public GenreRepositoryTest(GenreRepositoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(InsertGenre))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repository")]

    public async Task InsertGenre()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(category =>
            exampleGenre.AddCategory(category.Id));

        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var genreRepository = new Repository.GenreRepository(dbContext);

        await genreRepository.Insert(exampleGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDbContext
            .Genres.FindAsync(exampleGenre.Id);

        dbGenre.Should().NotBeNull();
        dbGenre.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);

        var dbCategories = await assertsDbContext
            .GenreCategories
            .Where(r => r.GenreId == exampleGenre.Id)
            .ToListAsync();

        dbCategories.Should().NotBeNull();
        dbCategories.Should().HaveCount(categoriesListExample.Count);

        dbCategories.ForEach(relation =>
        {
            var expectedCategory = categoriesListExample
                .FirstOrDefault(c => c.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });

    }
}
