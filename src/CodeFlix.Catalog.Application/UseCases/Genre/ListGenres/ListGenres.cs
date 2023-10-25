

using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
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
        var searchInput = new SearchInput(
            request.Page,
            request.PerPage,
            request.Search,
            request.Sort,
            request.Dir);

        var searchOutput = await _genreRepository.Search(searchInput, cancellationToken);

        return ListGenresOutput.FromSearchOutput(searchOutput);
    }
}
