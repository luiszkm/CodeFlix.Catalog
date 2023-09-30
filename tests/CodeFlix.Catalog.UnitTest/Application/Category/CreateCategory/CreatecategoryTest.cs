using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Domain.Exceptions;
using Moq;
using UseCases = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;

namespace CodeFlix.Catalog.UnitTest.Application.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreatecategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;
    public CreatecategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var validCategory = _fixture.GetValidInput();

        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Category>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

        unitOfWorkMock.Verify(
             unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
             Times.Once);

        output.Name.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }


    [Theory(DisplayName = nameof(CreateCategoryThrowWhencantInstantiate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]
    public async void CreateCategoryThrowWhencantInstantiate(
        CreateCategoryInput input,
        string excepetionMessage)
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
                       repositoryMock.Object,
                       unitOfWorkMock.Object);

        Func<Task> action = async () =>
        await useCase.Handle(input, CancellationToken.None);
        await action.Should().
         ThrowAsync<EntityValidationException>()
         .WithMessage(excepetionMessage);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyname))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyname()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName());



        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Category>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

        unitOfWorkMock.Verify(
             unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
             Times.Once);

        output.Name.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlynameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlynameAndDescription()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription());



        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Category>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

        unitOfWorkMock.Verify(
             unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
             Times.Once);

        output.Name.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlynameAndDescriptionAndIsActive))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlynameAndDescriptionAndIsActive()
    {
        var repositoryMock = _fixture.GetCategoryRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.CreateCategory(
                       repositoryMock.Object,
                                  unitOfWorkMock.Object);

        var input = new CreateCategoryInput(
                       _fixture.GetValidCategoryName(),
                       _fixture.GetValidCategoryDescription(),
                        false);
        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Category>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

        unitOfWorkMock.Verify(
             unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
             Times.Once);

        output.Name.Should().NotBeNull();
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(false);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));

    }

    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();

        var invalidInputNullName = fixture.GetValidInput();
        invalidInputNullName.Name = "1";
        invalidInputsList.Add(new object[] { invalidInputNullName, "Name should be at 3 characters long" });

        var invalidInputShortName = fixture.GetInvalidInputShortName();
        invalidInputsList.Add(new object[] { invalidInputShortName, "Name should be at 3 characters long" });

        var invalidInputToLongName = fixture.GetInvalidInputTooLongName();
        invalidInputsList.Add(new object[] { invalidInputToLongName, "Name should be less or equal 255 characters long" });



        var invalidInputNullDescription = fixture.GetInvalidInputCategoryNull();
        invalidInputsList.Add(new object[] { invalidInputNullDescription, "Description should not be null" });

        var invalidInputToLongDescription = fixture.GetInvalidInputTooLongDescription();
        invalidInputsList.Add(new object[] { invalidInputToLongDescription, "Description should be less or equal 10000 characters long" });





        return invalidInputsList;

    }
}
