
using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Infra.Data.EF;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;



[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "GetCategory - use-case")]

    public async Task CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new Repository.CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var input = _fixture.GetInput();

        var useCase = new UseCase.CreateCategory(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default);

    }




}
