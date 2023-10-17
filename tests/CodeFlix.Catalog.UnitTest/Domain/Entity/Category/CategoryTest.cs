using CodeFlix.Catalog.Domain.Domain.Exceptions;
using FluentAssertions;

namespace CodeFlix.Catalog.UnitTest.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryFixture;
    public CategoryTest(CategoryTestFixture categoryTestFixture)
      => _categoryFixture = categoryTestFixture;

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]

    public void Instantiate()
    {
        // Arrange
        var validCategory = _categoryFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;
        //ACT
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        Assert.Equal(validCategory.Name, category.Name);
        Assert.Equal(validCategory.Description, category.Description);
        Assert.NotEqual(default, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt >= dateTimeBefore);
        Assert.True(category.CreatedAt <= dateTimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]


    public void InstantiateWithActive(bool isActive)
    {
        Console.WriteLine(isActive);
        // Arrange
        var validCategory = _categoryFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;
        //ACT
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        // Assert
        Assert.NotNull(category);
        Assert.Equal(validCategory.Name, category.Name);
        Assert.Equal(validCategory.Description, category.Description);
        Assert.NotEqual(default, category.Id);
        Assert.NotEqual(default, category.CreatedAt);
        Assert.True(category.CreatedAt >= dateTimeBefore);
        Assert.True(category.CreatedAt <= dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InsatiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]

    public void InsatiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryFixture.GetValidCategory();

        Action action =
            () => new DomainEntity.Category(name!, validCategory.Description);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InsatiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]

    public void InsatiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action =
            () => new DomainEntity.Category(validCategory.Name, null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InsatiateErrorWhitNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ba")]
    public void InsatiateErrorWhitNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action =
             () => new DomainEntity.Category(invalidName, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at 3 characters long", exception.Message);
    }


    [Fact(DisplayName = nameof(InsatiateErrorWhitNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InsatiateErrorWhitNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action =
             () => new DomainEntity.Category(invalidName, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);

    }

    [Fact(DisplayName = nameof(InsatiateErrorWhitDescriptionIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]


    public void InsatiateErrorWhitDescriptionIsGreaterThan255Characters()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action =
             () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);

    }


    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var validCategory = _categoryFixture.GetValidCategory();
        //ACT
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();
        // Assert
        Assert.True(category.IsActive);


    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        var validCategory = _categoryFixture.GetValidCategory();
        //ACT
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();
        // Assert
        Assert.False(category.IsActive);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action =
            () => validCategory.Update(name!, validCategory.Description);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);

    }


    [Theory(DisplayName = nameof(UpdateErrorWhitNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ba")]

    public void UpdateErrorWhitNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryFixture.GetValidCategory();
        Action action =
             () => validCategory.Update(invalidName, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhitNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhitNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action =
             () => validCategory.Update(invalidName, validCategory.Description);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhitDescriptionIsGreaterThan10_000Characters))]


    public void UpdateErrorWhitDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action =
             () => validCategory.Update(validCategory.Name, invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);

    }


    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _categoryFixture.GetValidCategory();
        var categoryWithNewValues = _categoryFixture.GetValidCategory();

        validCategory.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        Assert.Equal(categoryWithNewValues.Name, validCategory.Name);
        Assert.Equal(categoryWithNewValues.Description, validCategory.Description);
        Assert.True(categoryWithNewValues.Name.Length > 3 && categoryWithNewValues.Name.Length < 255);
        Assert.True(categoryWithNewValues.Description.Length < 10_000);

    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()

    {
        var validCategory = _categoryFixture.GetValidCategory();

        var categoryWithNewValues = _categoryFixture.GetValidCategory();
        var currentDescription = validCategory.Description;

        validCategory.Update(categoryWithNewValues.Name);

        Assert.Equal(categoryWithNewValues.Name, validCategory.Name);
        Assert.Equal($"{currentDescription}", validCategory.Description);
        Assert.True(categoryWithNewValues.Name.Length > 3 && categoryWithNewValues.Name.Length < 255);

    }
}
