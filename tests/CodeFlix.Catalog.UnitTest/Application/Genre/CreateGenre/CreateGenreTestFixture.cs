

using CodeFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using CodeFlix.Catalog.UnitTest.Application.Genre.Common;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.Create;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{

    public CreateGenreInput GetExampleInput()
        => new CreateGenreInput(
            GetValidGenreName(),
            GetRandomBoolean());
    public CreateGenreInput GetExampleInput(string name)
        => new CreateGenreInput(
            name,
            GetRandomBoolean());
    public CreateGenreInput GetExampleInputWithCategories()
    {
        var numberOfCategoriesIds = GetRandomNumber();
        var caregoriesIds = Enumerable
            .Range(1, numberOfCategoriesIds)
            .Select(_ => Guid.NewGuid())
            .ToList();

        return new CreateGenreInput(
            GetValidGenreName(),
            GetRandomBoolean(),
            caregoriesIds);
    }

    private int GetRandomNumber(int min = 1, int max = 10)
    {
        var random = new Random();
        return random.Next(min, max);
    }

}