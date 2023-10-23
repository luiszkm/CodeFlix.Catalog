
using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.Common;
public class GenreUseCasesBaseFixture : BaseFixture
{
    public List<Guid> GetRandomIdList(int? count = null)
        => Enumerable.Range(1, count ?? (new Random()).Next(1, 10))
            .Select(x => Guid.NewGuid())
            .ToList();
    public Mock<IGenreRepository> GetGenreRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
        => new();
    public string GetValidGenreName()
    => Faker.Commerce.Categories(1)[0];

    public DomainEntity.Genre GetExampleGenre(
        bool? isActive = null,
        List<Guid>? categoriesIds = null)
    {
        var genre = new DomainEntity.Genre(
            GetValidGenreName(),
            isActive ?? GetRandomBoolean());
        categoriesIds?.ForEach(genre.AddCategory);
        return genre;
    }


}
