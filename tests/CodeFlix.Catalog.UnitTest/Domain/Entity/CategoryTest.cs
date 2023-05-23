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
}
