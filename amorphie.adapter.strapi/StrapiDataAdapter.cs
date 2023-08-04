using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Features;
using amorphie.adapter.strapi.Service;

public sealed class StrapiDataAdapter : IDataAdapter
{
    private readonly IStrapiService _strapiService;
    public StrapiDataAdapter(IStrapiService strapiService)=> _strapiService = strapiService;

    public async ValueTask<string> Search(string entity, int page, int pageSize, string? keyword, Dictionary<string, dynamic>? filters)
    {
        var response = await _strapiService.GetEntity(entity, page, pageSize);

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
}