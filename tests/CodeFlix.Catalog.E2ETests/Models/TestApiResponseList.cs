namespace CodeFlix.Catalog.E2ETests.Models;
public class TestApiResponseList<TOutputItem>
    : TestApiResponse<List<TOutputItem>>
{
    public TestApiResponseListMeta? Meta { get; set; }

    public TestApiResponseList(List<TOutputItem> data) : base(data) { }

    public TestApiResponseList()
    { }

    public TestApiResponseList(
        List<TOutputItem> data,
        TestApiResponseListMeta meta
    ) : base(data)
        => Meta = meta;
}

public class TestApiResponseListMeta
{
    public int Total { get; set; }
    public int PerPage { get; set; }
    public int CurrentPage { get; set; }

    public TestApiResponseListMeta()
    { }

    public TestApiResponseListMeta(
        int total,
        int perPage,
        int currentPage)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
    }
}

public class TestApiResponse<TOutput>
{
    public TOutput? Data { get; set; }

    public TestApiResponse()
    { }

    public TestApiResponse(TOutput data)
    {
        Data = data;
    }
}
