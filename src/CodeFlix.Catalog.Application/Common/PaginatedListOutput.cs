using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Application.Common;
public abstract class PaginatedListOutput<TOutoutItem>
{
    protected PaginatedListOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<TOutoutItem> items)
    {
        Page = page;
        PerPage = perPage;
        Total = total;
        Items = items;
    }


    public int Page { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<TOutoutItem> Items { get; set; }
}
