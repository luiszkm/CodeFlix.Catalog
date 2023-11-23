

using CodeFlix.Catalog.Domain.Domain.Repository;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.ListGenres;
public class ListGenres : IListGenres, IRequest<ListGenresOutput>
{
    private readonly IGenreRepository _genreRepository;
    public ListGenres(IGenreRepository genreRepository)
    => _genreRepository = genreRepository;


    public async Task<ListGenresOutput> Handle
        (ListGenreInput request,
        CancellationToken cancellationToken)
    {
        var searchInput = request.ToSearchInput();
        var searchOutput = await _genreRepository.Search(searchInput, cancellationToken);

        return ListGenresOutput.FromSearchOutput(searchOutput);
    }
}
