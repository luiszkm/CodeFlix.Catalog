

using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository;
[Collection(nameof(GenreRepositoryTestFixture))]
public class GenreRepositoryTest : IDisposable
{

    private readonly GenreRepositoryTestFixture _fixture;

    public GenreRepositoryTest(GenreRepositoryTestFixture genreRepository)
    {
        _fixture = genreRepository;
    }


    [Fact(DisplayName = nameof(InsertGenre))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repository")]

    public async Task InsertGenre()
    {

        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(category => exampleGenre.AddCategory(category.Id));

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

        var gerenCategoriesRelation = await assertsDbContext.GenresCategories
            .Where(gc => gc.GenreId == exampleGenre.Id)
            .ToListAsync();
        gerenCategoriesRelation.Should().HaveCount(categoriesListExample.Count);
    }

    public void Dispose()
    {
        _fixture.ClearDatabase();
    }
}
