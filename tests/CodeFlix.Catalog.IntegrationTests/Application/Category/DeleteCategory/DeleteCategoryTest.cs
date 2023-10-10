using CodeFlix.Catalog.Application.Exceptions;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;

using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.IntegrationTests.Application.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Integration/Application", "GetCategory - use-case")]

    public async Task DeleteCategory()
    {
        var exampleCategory = _fixture.GetExampleCategory();
        var exampleList = _fixture.GetExampleCategoriesList(10);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(exampleList);
        var trackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();
        trackingInfo.State = EntityState.Detached;

        var repository = new Repository.CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);

        var input = new UseCase.DeleteCategoryInput(exampleCategory.Id);

        await useCase.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var dbCategoryDeleted = await assertDbContext.Categories.
            FindAsync(exampleCategory.Id);

        var dbCategories = await assertDbContext.Categories.ToListAsync();

        dbCategoryDeleted.Should().BeNull();
        dbCategories.Should().HaveCount(10);

    }

    [Fact(DisplayName = nameof(DeleteCategoryThrowsWhernNotFound))]
    [Trait("Integration/Application", "GetCategory - use-case")]

    public async Task DeleteCategoryThrowsWhernNotFound()
    {
        var exampleList = _fixture.GetExampleCategoriesList(10);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(exampleList);
        await dbContext.SaveChangesAsync();


        var repository = new Repository.CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.DeleteCategory(repository, unitOfWork);

        var input = new UseCase.DeleteCategoryInput(Guid.NewGuid());

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");

    }
}
