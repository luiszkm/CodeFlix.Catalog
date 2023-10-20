
using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
public class UpdateGenreInput : IRequest<GenreModelOutput>
{
    public UpdateGenreInput(
        Guid id,
        string name,
        bool? isActive = null)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
}
