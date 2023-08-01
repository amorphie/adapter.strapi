
using System.Text.Json;

public interface IDataAdapter
{
    ValueTask<dynamic> Search(string entity, int page, int pageSize, string? keyword, Dictionary<string, dynamic>? filters );
    ValueTask<dynamic> Upsert(string entity, JsonElement data);

}
