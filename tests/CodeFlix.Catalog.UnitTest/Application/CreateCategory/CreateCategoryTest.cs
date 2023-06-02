
using CodeFlix.Catalog.Application.Interfaces;

using CodeFlix.Catalog.Domain.Domain.Entity;
using CodeFlix.Catalog.Domain.Domain.Repository;
using Moq;

using UseCases = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.UnitTest.Application.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]

public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
        => _fixture = fixture ;
    

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]

    public async void CreateCategory()
    {
        // Arrange
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            categoryRepositoryMock.Object,
            unitOfWorkMock.Object
            );

        var input = _fixture.GetValidInput();
           
        // Act

        var output = await useCase.Handle(input, CancellationToken.None);
        // Assert

        categoryRepositoryMock.Verify(
            repository => repository.Insert(
              It.IsAny<Category>(),
              It.IsAny<CancellationToken>()),
              Times.Once
            );
        unitOfWorkMock.Verify(
            unitOfWorkMock => unitOfWorkMock.Commit(
               It.IsAny<CancellationToken>()),
            Times.Once
            );

        Assert.NotNull(output);
        Assert.True(output.Id != Guid.Empty);
        Assert.Equal(input.Name, output.Name);
        Assert.Equal(input.Description, output.Description);
        Assert.Equal(input.IsActive, output.IsActive);
        Assert.True(output.CreatedAt != default);
    }

    [Theory(DisplayName = nameof(CreateCategory_WithInvalidInput))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInput))]

    public async void CreateCategory_WithInvalidInput()
    {
        // Arrange
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(
            categoryRepositoryMock.Object,
            unitOfWorkMock.Object
            );
      //  var input = _fixture.Ge


    }

    public static IEnumerable<object[]> GetInvalidInput(){
        return new List<object[]>();
    }

}
