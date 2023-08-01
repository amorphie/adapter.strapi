using System.Text.Json;


public class StrapiDataAdapter : IDataAdapter
{

    private readonly IHttpClientFactory _httpClientFactory;

    public StrapiDataAdapter(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;


    public async ValueTask<dynamic> Search(string entity, int page, int pageSize, string? keyword, Dictionary<string, dynamic>? filters)
    {

        var message = BuildHttpRequestMessage(entity, HttpMethod.Get);

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(message);

        var response = await httpResponseMessage.Content.ReadAsStringAsync();


        return response;
    }

    public ValueTask<dynamic> Upsert(string entity, JsonElement data)
    {
        throw new NotImplementedException();
    }


    private static HttpRequestMessage BuildHttpRequestMessage(string entity, HttpMethod method)
    {
        var url = Environment.GetEnvironmentVariable("STRAPI_URL") ?? throw new ArgumentNullException("Parameter is not suplied as enviroment variable", "STRAPI_URL");
        var token = Environment.GetEnvironmentVariable("STRAPI_TOKEN") ?? throw new ArgumentNullException("Parameter is not suplied as enviroment variable", "STRAPI_TOKEN");

        UriBuilder uri = new(url)
        {
            Path = $"api/{entity}"
        };

        var httpRequestMessage = new HttpRequestMessage(
            method,
            uri.Uri);

        httpRequestMessage.Headers.Authorization = new("Bearer", token);

        return httpRequestMessage;
    }

}