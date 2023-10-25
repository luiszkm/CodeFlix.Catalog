using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.GetGenre;
public interface IGetGenre : IRequestHandler<GetGenreInput, GenreModelOutput>
{
}
