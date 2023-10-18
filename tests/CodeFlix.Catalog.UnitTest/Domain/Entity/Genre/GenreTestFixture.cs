using CodeFlix.Catalog.UnitTest.Common;


namespace CodeFlix.Catalog.UnitTest.Domain.Entity.Genre;

[CollectionDefinition(nameof(GenreTestFixture))]

public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture>
{

}
public class GenreTestFixture : BaseFixture
{
    public string GetValidName()
        => Faker.Commerce.Categories(1)[0];

    public List<Guid> GetListCategories(int length)
    {
        List<Guid> guidList = new List<Guid>();
        for (int i = 0; i < length; i++)
        {
            Guid newGuid = Guid.NewGuid();
            guidList.Add(newGuid);
        }
        return guidList;
    }

    public DomainEntity.Genre GetValidGenre(
        bool isActive = true,
        int? categoriesLength = null)
    {
        var genre = new DomainEntity.Genre(GetValidName(), isActive);
        if (categoriesLength is not null)
        {
            foreach (var item in GetListCategories((int)categoriesLength))
            {
                genre.AddCategory(item);
            }
        }
        return genre;
    }
}
