using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace CodeFlix.Catalog.UnitTest.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]

public class UpdateCategoryInputValidatorTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application ", "UpdateCategoryInputValidator - useCases")]

    public void DontValidateWhenEmptyGuid()
    {
        var input = _fixture.GetValidInput(Guid.Empty);

        var validator = new UpdateCategoryInputValidator();

        var result = validator.Validate(input);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.First().ErrorMessage.Should().Be("'Id' must not be empty.");

    }

    [Fact(DisplayName = nameof(ValidateWhenValidInput))]
    [Trait("Application ", "UpdateCategoryInputValidator - useCases")]

    public void ValidateWhenValidInput()
    {
        var input = _fixture.GetValidInput();

        var validator = new UpdateCategoryInputValidator();

        var result = validator.Validate(input);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);

    }
}
