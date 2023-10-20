using CodeFlix.Catalog.Domain.Domain.Exceptions;

namespace CodeFlix.Catalog.UnitTest.Domain.Entity.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
    private readonly GenreTestFixture _fixture;

    public GenreTest(GenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]

    public void Instantiate()
    {
        var genreName = "Horror";
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        var genre = new DomainEntity.Genre(genreName);
        int a;

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().BeTrue();
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
        (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]

    public void InstantiateWithIsActive(bool isActive)
    {
        var genreName = _fixture.GetValidName();
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        var genre = new DomainEntity.Genre(genreName, isActive);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(isActive);
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
        (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Activate))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]

    public void Activate(bool isActive)
    {
        var genreName = _fixture.GetValidName();
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        var genre = new DomainEntity.Genre(genreName, isActive);
        genre.Activate();

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(true);
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
        (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Deactivate(bool isActive)
    {
        var genreName = _fixture.GetValidName();
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        var genre = new DomainEntity.Genre(genreName, isActive);
        genre.Deactivate();

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(false);
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
        (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(UpdateGenre))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void UpdateGenre(bool isActive)
    {
        var genreName = _fixture.GetValidName();
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        var genre = _fixture.GetValidGenre(isActive);
        genre.Update(genreName);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.IsActive.Should().Be(isActive);
        genre.CreatedAt.Should().BeBefore(DateTime.Now);
        (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateThrowWhenEmpty))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void InstantiateThrowWhenEmpty(string? name)
    {
        var action = () => new DomainEntity.Genre(name!);
        action.Should().Throw<EntityValidationException>()
        .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateThrowWhenEmpty))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateThrowWhenEmpty(string? name)
    {
        var genre = _fixture.GetValidGenre();
        var action = () => genre.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(AddCategory))]
    [Trait("Domain", "Genre - Aggregates")]

    public void AddCategory()
    {
        var genre = _fixture.GetValidGenre();
        var categoryGuid = Guid.NewGuid();

        genre.AddCategory(categoryGuid);

        genre.Should().NotBeNull();
        genre.Categories.Should().NotBeEmpty();
        genre.Categories.Should().HaveCount(1);
        genre.Categories.Should().Contain(categoryGuid);

    }
    [Fact(DisplayName = nameof(AddManyCategory))]
    [Trait("Domain", "Genre - Aggregates")]

    public void AddManyCategory()
    {
        var genre = _fixture.GetValidGenre();
        var categoryGuid = Guid.NewGuid();
        var categoryGuid2 = Guid.NewGuid();
        var categoryGuid3 = Guid.NewGuid();

        genre.AddCategory(categoryGuid);
        genre.AddCategory(categoryGuid2);
        genre.AddCategory(categoryGuid3);

        genre.Should().NotBeNull();
        genre.Categories.Should().NotBeEmpty();
        genre.Categories.Should().HaveCount(3);
        genre.Categories.Should().Contain(categoryGuid);
        genre.Categories.Should().Contain(categoryGuid2);
        genre.Categories.Should().Contain(categoryGuid3);

    }

    [Fact(DisplayName = nameof(RemoveCategory))]
    [Trait("Domain", "Genre - Aggregates")]

    public void RemoveCategory()
    {
        var genre = _fixture.GetValidGenre();
        var categoryGuid = Guid.NewGuid();
        genre.AddCategory(categoryGuid);

        genre.Should().NotBeNull();
        genre.Categories.Should().NotBeEmpty();
        genre.Categories.Should().HaveCount(1);
        genre.Categories.Should().Contain(categoryGuid);

        genre.RemoveCategory(categoryGuid);

        genre.Should().NotBeNull();
        genre.Categories.Should().BeEmpty();
        genre.Categories.Should().HaveCount(0);
        genre.Categories.Should().NotContain(categoryGuid);

    }
    [Fact(DisplayName = nameof(RemoveWhitManyCategory))]
    [Trait("Domain", "Genre - Aggregates")]

    public void RemoveWhitManyCategory()
    {
        var genre = _fixture.GetValidGenre(categoriesLength: 10);
        var categoryGuid = Guid.NewGuid();
        genre.AddCategory(categoryGuid);

        genre.Should().NotBeNull();
        genre.Categories.Should().NotBeEmpty();
        genre.Categories.Should().HaveCount(11);
        genre.Categories.Should().Contain(categoryGuid);

        genre.RemoveCategory(categoryGuid);

        genre.Should().NotBeNull();
        genre.Categories.Should().HaveCount(10);
        genre.Categories.Should().NotContain(categoryGuid);


    }
    [Fact(DisplayName = nameof(RemoveCategoryThatAlreadyContainsInTheList))]
    [Trait("Domain", "Genre - Aggregates")]

    public void RemoveCategoryThatAlreadyContainsInTheList()
    {
        var genre = _fixture.GetValidGenre(categoriesLength: 5);
        var categoryGuidToRemove = genre.Categories[0];
        genre.RemoveCategory(categoryGuidToRemove);

        genre.Should().NotBeNull();
        genre.Categories.Should().HaveCount(4);
        genre.Categories.Should().NotContain(categoryGuidToRemove);

    }
    [Fact(DisplayName = nameof(RemoveAllCategoryT))]
    [Trait("Domain", "Genre - Aggregates")]

    public void RemoveAllCategoryT()
    {
        var genre = _fixture.GetValidGenre(categoriesLength: 10);
        genre.RemoveAllCategories();

        genre.Should().NotBeNull();
        genre.Categories.Should().BeEmpty();
        genre.Categories.Should().HaveCount(0);
    }
}
