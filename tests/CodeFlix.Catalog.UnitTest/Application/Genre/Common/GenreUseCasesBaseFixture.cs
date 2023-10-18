
using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.UnitTest.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTest.Application.Genre.Common;
public class GenreUseCasesBaseFixture : BaseFixture
{
    public Mock<IGenreRepository> GetGenreRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
    public Mock<ICategoryRepository> GetCategoryRepositoryMock()
        => new();
    public string GetValidGenreName()
    => Faker.Commerce.Categories(1)[0];
}
