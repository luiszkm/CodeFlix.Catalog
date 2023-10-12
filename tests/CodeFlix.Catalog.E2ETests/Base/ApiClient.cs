using System.Text.Json;
using System.Text;

namespace CodeFlix.Catalog.E2ETests.Base;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }



    public async Task<(HttpResponseMessage response, TOutput?)>
        Post<TOutput>(
            string route,
            object payload
        )
    {
        var inputString = JsonSerializer.Serialize(payload);
        var inputContent = new StringContent(inputString, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(route, inputContent);

        var outputString = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<TOutput>(
            outputString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return (response, output);
    }
}




