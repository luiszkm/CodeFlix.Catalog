
using CodeFlix.Catalog.Application.Common;
using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Application.UseCases.Genre.ListGenres;
public class ListGenresOutput : PaginatedListOutput<GenreModelOutput>
{
    public ListGenresOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<GenreModelOutput> items) : base(page, perPage, total, items)
    {
    }


    public static ListGenresOutput FromSearchOutput(
        SearchOutput<DomainEntity.Genre> searchOutput
    ) => new(
        searchOutput.CurrentPage,
        searchOutput.PerPage,
        searchOutput.Total,
        searchOutput.Items
            .Select(GenreModelOutput.FromGenre)
            .ToList()
    );

    internal void FillWithCategoryNames(IReadOnlyList<DomainEntity.Category> categories)
    {
        foreach (GenreModelOutput item in Items)
            foreach (GenreModelOutput.GenreModelOutputCategory categoryOutput in item.Categories)
                categoryOutput.Name = categories?.FirstOrDefault(
                    category => category.Id == categoryOutput.Id
                )?.Name;
    }
}
