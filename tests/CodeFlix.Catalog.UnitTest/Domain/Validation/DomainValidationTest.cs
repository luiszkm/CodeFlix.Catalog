using Bogus;
using CodeFlix.Catalog.Domain.Domain.Exceptions;
using CodeFlix.Catalog.Domain.Domain.Validation;

namespace CodeFlix.Catalog.UnitTest.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact (DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]

    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
       Action action =
            ()=> DomainValidation.NotNull(value, "value");
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]

    public void NotNullThrowWhenNull()
    {
        string? value = null;
        Action action =
             () => DomainValidation.NotNull(value, "fieldName");
        action.Should().Throw<EntityValidationException>()
            .WithMessage("fieldName should not be empty or null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]

    public  void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "fieldName");
        action.Should().Throw<EntityValidationException>()
        .WithMessage("fieldName should not be empty or null");

    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
   

    public void NotNullOrEmptyOk()
    {
        var target = Faker.Commerce.ProductName();
        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "fieldName");
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("a", 3)]
    [InlineData("a1", 3)]

    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        Action action =
            () => DomainValidation.MinLength(target, minLength , "fieldName");
        action.Should().Throw<EntityValidationException>()
        .WithMessage($"fieldName should be at {minLength} characters long");
    }


    [Theory(DisplayName = nameof(GetValuesGreaterThanMin))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("aaa", 3)]
    [InlineData("abajur", 3)]

    public void GetValuesGreaterThanMin(string target, int minLength)
    {
        Action action =
            () => DomainValidation.MinLength(target, minLength, "fieldName");
        action.Should().NotThrow();
    }


    [Fact(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    

    public void MaxLengthThrowWhenGreater()
    {
        var target = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        var maxLength = 255;

        Action action =
            () => DomainValidation.MaxLength(target, maxLength, "fieldName");
        action.Should().Throw<EntityValidationException>()
        .WithMessage($"fieldName should be less or equal {maxLength} characters long");
    }

    [Fact(DisplayName = nameof(GetValuesLessThanMax))]
    [Trait("Domain", "DomainValidation - Validation")]


    public void GetValuesLessThanMax()
    {
        var target = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        var maxLength = 260;
        Action action =
            () => DomainValidation.MaxLength(target, maxLength, "fieldName");
        action.Should().NotThrow();
    }

}
