

using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.CreateGenre;
public interface ICreateGenre : IRequestHandler<CreateGenreInput, GenreModelOutput>
{
}
