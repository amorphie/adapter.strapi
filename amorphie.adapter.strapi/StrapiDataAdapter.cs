using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Features;

public class StrapiDataAdapter : IDataAdapter
{

    private readonly IHttpClientFactory _httpClientFactory;

    public StrapiDataAdapter(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;


    public async ValueTask<string> Search(string entity, int page, int pageSize, string? keyword, Dictionary<string, dynamic>? filters)
    {
        var queryParams = new Dictionary<string, string?>();

        queryParams.Add("pagination[page]", page.ToString());
        queryParams.Add("pagination[pageSize]", pageSize.ToString());

        var message = BuildHttpRequestMessage(entity, HttpMethod.Get, queryParams);

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(message);


        var response = await httpResponseMessage.Content.ReadFromJsonAsync<JsonNode>();

        List<JsonObject> result = new();

        foreach (var item in response["data"].AsArray())
        {
            var id = (int)item["id"].AsValue();

            var returnItem = new JsonObject
            {
                ["id"] = id
            };

            foreach (var x in item["attributes"].AsObject())
            {
                returnItem.Add(x.Key, x.Value.ToString());
            }

            result.Add(returnItem);
        }


        return JsonSerializer.Serialize(result);
    }

    public ValueTask<dynamic> Upsert(string entity, JsonElement data)
    {
        throw new NotImplementedException();
    }


    private static HttpRequestMessage BuildHttpRequestMessage(string entity, HttpMethod method, Dictionary<string, string?>? queryParams)
    {
        var url = Environment.GetEnvironmentVariable("STRAPI_URL") ?? throw new ArgumentNullException("Parameter is not suplied as enviroment variable", "STRAPI_URL");
        var token = Environment.GetEnvironmentVariable("STRAPI_TOKEN") ?? throw new ArgumentNullException("Parameter is not suplied as enviroment variable", "STRAPI_TOKEN");

        UriBuilder uri = new(url)
        {
            Path = $"api/{entity}"
        };

        if (queryParams != null)
        {
            uri = new UriBuilder(QueryHelpers.AddQueryString(uri.Uri.ToString(), queryParams));
        }

        var httpRequestMessage = new HttpRequestMessage(
            method,
            uri.Uri);

        httpRequestMessage.Headers.Authorization = new("Bearer", token);

        return httpRequestMessage;
    }

}