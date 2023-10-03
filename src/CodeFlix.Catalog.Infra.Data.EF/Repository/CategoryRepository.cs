
using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Domain.Entity;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Infra.Data.EF.Repository;
public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCatalogDbContext _context;

    public CategoryRepository(CodeflixCatalogDbContext context)
    {
        _context = context;
    }

    private DbSet<Category> _categories => _context.Set<Category>();



    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
     => await _categories.AddAsync(aggregate, cancellationToken);




    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.AsNoTracking()
            .FirstOrDefaultAsync(
             c => c.Id == id,
             cancellationToken);

        NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");

        return category!;
    }

    public async Task Update(Category aggregate, CancellationToken _)
    {
        _categories.Update(aggregate);
        await Task.CompletedTask;
    }

    public async Task Delete(Category aggregate, CancellationToken _)
        => Task.FromResult(_categories.Remove(aggregate));

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
