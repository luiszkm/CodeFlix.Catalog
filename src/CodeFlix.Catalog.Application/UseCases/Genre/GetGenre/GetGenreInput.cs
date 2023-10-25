using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.GetGenre;
public class GetGenreInput : IRequest<GenreModelOutput>
{
    public GetGenreInput(Guid id)
    => Id = id;

    public Guid Id { get; set; }
}
