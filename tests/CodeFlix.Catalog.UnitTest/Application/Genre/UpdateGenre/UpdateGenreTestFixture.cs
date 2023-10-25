using CodeFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
using CodeFlix.Catalog.UnitTest.Application.Genre.Common;
namespace CodeFlix.Catalog.UnitTest.Application.Genre.UpdateGenre;

[CollectionDefinition(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTestFixtureCollection : ICollectionFixture<UpdateGenreTestFixture>
{
}
public class UpdateGenreTestFixture : GenreUseCasesBaseFixture
{
    public UpdateGenreInput GetExampleInput(Guid id)
        => new UpdateGenreInput(
            id,
            GetValidGenreName(),
            GetRandomBoolean());
}
