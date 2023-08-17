using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Features;
using amorphie.adapter.strapi.Service;
using System.Diagnostics.Eventing.Reader;

public sealed class StrapiDataAdapter : IDataAdapter
{
    private readonly IStrapiService _strapiService;
    public StrapiDataAdapter(IStrapiService strapiService) => _strapiService = strapiService;

    public async ValueTask<List<JsonObject>> Search(string entity, int page, int pageSize, string? keyword, Dictionary<string, dynamic>? filters)
    {
        var response = await _strapiService.GetEntity(entity, page, pageSize);

        List<JsonObject> result = new();

        foreach (var item in response["data"].AsArray())
        {
            result.Add(item.AsObject());
        }

        return result; 
    }

    public ValueTask<dynamic> Upsert(string entity, JsonElement data)
    {
        throw new NotImplementedException();
    }
}