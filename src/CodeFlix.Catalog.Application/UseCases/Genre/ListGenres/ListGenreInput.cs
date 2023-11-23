

using CodeFlix.Catalog.Application.Common;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.ListGenres;
public class ListGenreInput : PaginatedListInput, IRequest<ListGenresOutput>
{
    public ListGenreInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc) : base(page, perPage, search, sort, dir)
    {
    }


}
