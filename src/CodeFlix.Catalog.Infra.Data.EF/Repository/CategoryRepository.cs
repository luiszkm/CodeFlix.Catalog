
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




    public Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
