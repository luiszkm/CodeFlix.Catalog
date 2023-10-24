

using CodeFlix.Catalog.UnitTest.Application.Genre.Common;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.DeleteGenre;

[CollectionDefinition(nameof(DeleteGenreTestFixture))]
public class DeleteGenreTestFixtureCollection : ICollectionFixture<DeleteGenreTestFixture>
{
}
public class DeleteGenreTestFixture : GenreUseCasesBaseFixture
{
}
