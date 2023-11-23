using CodeFlix.Catalog.Application.Common;

namespace CodeFlix.Catalog.Api.ApiModels.Response;

public class ApiResponseList<TItemData> : ApiResponse<IReadOnlyList<TItemData>>
{
    public ApiResponseList(
        int currentPage,
        int perPage,
        int total,
        IReadOnlyList<TItemData> data) : base(data)
    {
        Meta = new(
            currentPage,
            perPage,
            total);
    }
    public ApiResponseList(
        PaginatedListOutput<TItemData> paginatedListOutput
        ) : base(paginatedListOutput.Items)
    {
        Meta = new ApiResponseListMeta(
            paginatedListOutput.Page,
            paginatedListOutput.PerPage,
            paginatedListOutput.Total);
    }
    public ApiResponseListMeta Meta { get; private set; }
}
