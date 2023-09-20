using CodeFlix.Catalog.Domain.Domain.SeedWork;

namespace CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory : IRepository
{
    public Task<CreateCategoryOutput> Handle(
        CreateCategoryInput input,
        CancellationToken cancellationToken);

}
