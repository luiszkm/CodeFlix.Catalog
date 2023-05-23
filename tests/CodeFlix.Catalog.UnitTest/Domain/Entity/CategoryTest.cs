

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
        var category = new DomainEntity(validData.Name, validData.Description);
        var dateTimeAfter = DateTime.Now;

        // Assert

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);
        Assert.True(category.IsActive);
        

    }
 

}
