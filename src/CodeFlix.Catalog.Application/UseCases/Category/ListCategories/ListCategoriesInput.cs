using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
public class ListCategoriesInput : PaginatedListInput, IRequest, IRequest<ListCategoriesOutput>
{
    public ListCategoriesInput(
        int page,
        int perPage,
        string search,
        string sort,
        SearchOrder dir) :
        base(page, perPage, search, sort, dir)
    {
    }
}
