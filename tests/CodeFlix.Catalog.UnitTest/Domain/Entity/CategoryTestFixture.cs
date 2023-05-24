namespace CodeFlix.Catalog.UnitTest.Domain.Entity;
public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory()
    =>  new ("category name", "category description");

    
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CategoryTestFixture>
{ }