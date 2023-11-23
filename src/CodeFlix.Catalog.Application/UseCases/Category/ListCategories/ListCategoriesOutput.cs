

using CodeFlix.Catalog.Application.Common;
using CodeFlix.Catalog.Application.UseCases.Category.Common;

namespace CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
public class ListCategoriesOutput : PaginatedListOutput<CategoryModelOutput>
{
    public ListCategoriesOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<CategoryModelOutput> items) :
        base(page, perPage, total, items)
    {
    }
}
