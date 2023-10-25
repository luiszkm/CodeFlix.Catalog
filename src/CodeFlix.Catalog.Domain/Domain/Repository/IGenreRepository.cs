

using CodeFlix.Catalog.Domain.Domain.Entity;
using CodeFlix.Catalog.Domain.Domain.SeedWork;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Domain.Domain.Repository;

public interface IGenreRepository :
    IGenericRepository<Genre>,
    ISearchableRepository<Genre>
{
}
