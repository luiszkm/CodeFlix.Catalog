﻿using CodeFlix.Catalog.Domain.Domain.Entity;
using CodeFlix.Catalog.Domain.Domain.SeedWork;
using CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Domain.Domain.Repository;
public interface ICategoryRepository : IGenericRepository<Category>,
    ISearchableRepository<Category>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellation);

}
