

using CodeFlix.Catalog.Domain.Domain.Entity;

namespace CodeFlix.Catalog.Infra.Data.EF.Models;
public class GenreCategories
{
    public GenreCategories(
        Guid genreId,
        Guid categoryId)
    {
        GenreId = genreId;
        CategoryId = categoryId;
    }


    public Guid GenreId { get; set; }
    public Genre? Genre { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}
