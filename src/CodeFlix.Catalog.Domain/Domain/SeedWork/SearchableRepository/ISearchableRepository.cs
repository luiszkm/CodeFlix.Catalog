using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Domain.Domain.SeedWork.SearchableRepository;

public interface ISearchableRepository<TAggregate>
    where TAggregate : AggregateRoot
{

    Task<SearchOutput<TAggregate>> Search(
        SearchInput input,
        CancellationToken cancellationToken
        );

}
