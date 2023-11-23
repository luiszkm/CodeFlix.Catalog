
using CodeFlix.Catalog.Domain.Domain.Entity;
using CodeFlix.Catalog.Domain.Domain.Repository;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Infra.Data.EF.Repository;
public class GenreRepository : IGenreRepository
{
    private readonly CodeflixCatalogDbContext _context;

    public GenreRepository(CodeflixCatalogDbContext dbContext)
    {
        _context = dbContext;
    }
    private DbSet<Genre> _genres =>
        _context.Set<Genre>();

    private DbSet<GenreCategories> _genresCategories =>
        _context.Set<GenreCategories>();

    public async Task Insert(Genre aggregate, CancellationToken cancellationToken)
    {
        await _genres.AddRangeAsync(aggregate);
        if (aggregate.Categories.Count > 0)
        {
            var relations = aggregate.Categories
                .Select(categoryId => new GenreCategories(aggregate.Id, categoryId));
            await _genresCategories.AddRangeAsync(relations, cancellationToken);
        }
    }


    public Task<Genre> Get(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Genre aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Genre aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<Genre>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
