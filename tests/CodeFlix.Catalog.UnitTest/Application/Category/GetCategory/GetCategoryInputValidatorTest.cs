using CodeFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace CodeFlix.Catalog.UnitTest.Application.Category.GetCategory;


[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryTestFixture _getCategoryTestFixture;

    public GetCategoryInputValidatorTest(GetCategoryTestFixture getCategoryTestFixture)
    {
        _getCategoryTestFixture = getCategoryTestFixture;
    }

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetCategoryValidation- UseCase")]

    public void ValidationOk()
    {
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidatior();
        var result = validator.Validate(validInput);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();

    }

    [Fact(DisplayName = nameof(ValidationFail))]
    [Trait("Application", "GetCategoryValidation- UseCase")]
    public void ValidationFail()
    {
        var invalidInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidatior();
        var result = validator.Validate(invalidInput);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault().ErrorMessage.Should().Be("'Id' must not be empty.");

    }
}
