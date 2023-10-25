

using CodeFlix.Catalog.UnitTest.Application.Genre.Common;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.GetGenre;

[CollectionDefinition(nameof(GetGenreTestFixture))]
public class GetGenreTestFixtureCollection : ICollectionFixture<GetGenreTestFixture>
{
}
public class GetGenreTestFixture : GenreUseCasesBaseFixture
{
}
