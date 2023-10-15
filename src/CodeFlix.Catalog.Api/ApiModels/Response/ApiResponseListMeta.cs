namespace CodeFlix.Catalog.Api.ApiModels.Response;

public class ApiResponseListMeta
{
    public ApiResponseListMeta(
        int currentPage,
        int perPage,
        int total)
    {
        Total = total;
        PerPage = perPage;
        CurrentPage = currentPage;
    }

    public int Total { get; set; }
    public int PerPage { get; set; }
    public int CurrentPage { get; set; }

}
