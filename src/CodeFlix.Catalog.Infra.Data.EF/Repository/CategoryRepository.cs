
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

    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _categories.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);


        if (!String.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));


        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken);


        return new(input.Page, input.PerPage, total, items);
    }

    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    private IQueryable<Category> AddOrderToQuery(
        IQueryable<Category> query,
        string orderBy,
        SearchOrder order)

    {
        var orderQuery = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name)
                .ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name)
                .ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdAt", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };

        return orderQuery;
    }
}
