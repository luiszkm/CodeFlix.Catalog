

using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.ListGenres;
public interface IListGenres : IRequestHandler<ListGenreInput, ListGenresOutput>
{
}
