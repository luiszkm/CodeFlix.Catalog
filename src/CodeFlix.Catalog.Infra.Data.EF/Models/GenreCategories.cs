

using CodeFlix.Catalog.Domain.Domain.Entity;

namespace CodeFlix.Catalog.Infra.Data.EF.Models;
public class GenreCategories
{
    public GenreCategories(
        Guid categoryId,
        Guid genreId)
    {
        CategoryId = categoryId;
        GenreId = genreId;
    }

    public Guid CategoryId { get; set; }
    public Guid GenreId { get; set; }
    public Category? Category { get; set; }
    public Genre? Genre { get; set; }
}
