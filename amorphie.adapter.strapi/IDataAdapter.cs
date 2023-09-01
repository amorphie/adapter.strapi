
using System.Text.Json;
using System.Text.Json.Nodes;

public interface IDataAdapter
{
    ValueTask<List<JsonObject>> Search(string entity, int page, int pageSize, string? keyword, Dictionary<string, dynamic>? filters);
    ValueTask<dynamic> Upsert(string entity, JsonElement data);

}
