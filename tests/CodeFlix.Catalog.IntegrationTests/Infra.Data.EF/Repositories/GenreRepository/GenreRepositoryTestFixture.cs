

using CodeFlix.Catalog.IntegrationTests.Base;

namespace CodeFlix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository;

[CollectionDefinition(nameof(GenreRepositoryTestFixture))]
public class GenreRepositoryTestFixtureCollection : ICollectionFixture<GenreRepositoryTestFixture>
{
}
public class GenreRepositoryTestFixture : BaseFixture
{
    public string GetExampleGenreName()
        => Faker.Random.Word();
    public DomainEntity.Genre GetExampleGenre()
    => new(GetExampleGenreName());

}
