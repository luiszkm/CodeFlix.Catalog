

using CodeFlix.Catalog.Infra.Data.EF;

namespace CodeFlix.Catalog.E2ETests.API.Category.Common;
public class CategoryPersistence
{
    private readonly CodeflixCatalogDbContext _context;

    public CategoryPersistence(CodeflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<DomainEntity.Category?> GetById(Guid id)
        => await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
}
