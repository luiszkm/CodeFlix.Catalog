using CodeFlix.Catalog.Domain.Domain.Exceptions;

namespace CodeFlix.Catalog.UnitTest.Domain.Entity;
public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]

    public void Instantiate()
    {
        // Arrange
        var validData = new
        {
            Name = "categorry name",
            Description = "Description",
        };
        var dateTimeBefore = DateTime.Now;
        //ACT
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True( category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    
    public void InstantiateWithActive(bool isActive)
    {
        Console.WriteLine(isActive);
        // Arrange
        var validData = new
        {
            Name = "categorry name",
            Description = "Description",
        };
        var dateTimeBefore = DateTime.Now;
        //ACT
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;
        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName =nameof(InstatiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]

    public void InstatiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = 
            () => new DomainEntity.Category(name!, "");

       var exception = Assert.Throws<EntityValidationException>( action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstatiateErrorWhenDescriptionIsNull()
    {
        Action action =
            () => new DomainEntity.Category("Category Name", null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhitNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ba")]

    public void InstatiateErrorWhitNameIsLessThan3Characters(string invalidName)
    {
        Action action =
             () => new DomainEntity.Category(invalidName, "Category OK Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhitNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    

    public void InstatiateErrorWhitNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null ,Enumerable.Range(0, 256).Select(_ => "a").ToArray());

        Action action =
             () => new DomainEntity.Category(invalidName ,"category Description" );
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);

    }

    [Fact(DisplayName = nameof(InstatiateErrorWhitDescriptionIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]


    public void InstatiateErrorWhitDescriptionIsGreaterThan255Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());

        Action action =
             () => new DomainEntity.Category("category name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10_000 characters long", exception.Message);

    }


    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var validData = new
        {
            Name = "categorry name",
            Description = "Description",
        };
        //ACT
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();
        // Assert
        Assert.True( category.IsActive);
        
      
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        var validData = new
        {
            Name = "categorry name",
            Description = "Description",
        };
        //ACT
        var category = new DomainEntity.Category(validData.Name, validData.Description, true);
        category.Deactivate();
        // Assert
        Assert.False(category.IsActive);


    }
}
