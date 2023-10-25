
using CodeFlix.Catalog.Application.UseCases.Genre.ListGenres;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.UnitTest.Application.Genre.Common;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.ListGenres;

[CollectionDefinition(nameof(ListGenresTestFixture))]

public class ListGenresTestFixtureCollection : ICollectionFixture<ListGenresTestFixture>
{
}
public class ListGenresTestFixture : GenreUseCasesBaseFixture
{

    public ListGenreInput GetExampleInput()
    {
        var random = new Random();


        return new ListGenreInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 1) == 0 ?
                SearchOrder.Asc : SearchOrder.Desc);
    }
}
