using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.Listcategory;
public interface IListCategories :
    IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{

}
