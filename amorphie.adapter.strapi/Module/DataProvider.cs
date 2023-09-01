using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using amorphie.core.Module.minimal_api;
using amorphie.core.Base;
using System.Text.Json.Nodes;

public class DataProviderModule : BaseRoute
{
    public DataProviderModule(WebApplication app)
        : base(app)
    {

    }

    public override string? UrlFragment => "data";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {

        routeGroupBuilder.MapGet("{entity}", SearchMethod).WithOpenApi(generatedOperation =>
        {
            generatedOperation.Description = "This operation directly queries Strapi Collections. Strapi Collections are like database tables. These collections can be queried with pagination and full text search capabilities.";

            generatedOperation.Parameters[0].Description = "Collection Type name on Strapi to query";
            generatedOperation.Parameters[1].Description = "Which page will ve returned? Page index starts with 1";
            generatedOperation.Parameters[2].Description = "How many records does the page contain? Minimum value is 1. Maximum value is 100";
            generatedOperation.Parameters[3].Description = "Full text search keyword.";

            return generatedOperation;
        });

        routeGroupBuilder.MapPost("{entity}", UpsertMethod);
    }

    protected async ValueTask<IResult> SearchMethod(
            [FromRoute] string entity,
            [AsParameters] DtoSearchBase searchData,
            HttpContext context,
            IDataAdapter adapter
            )
    {
        Dictionary<string, dynamic>? filters = null;

        // We need for reference filtering. Additional dynamic query string parameters must read by QueryString
        // context... context.Request.QueryString

        List<JsonObject> result;

        try
        {
            result = await adapter.Search(entity, searchData.Page, searchData.PageSize, searchData.Keyword, filters);
            return Results.Ok(result);
        }
        catch (Exception)
        {
            return Results.BadRequest();

        }
        finally
        {
            // Special log ? 
        }
    }

    //protected async ValueTask<IResult> UpsertMethod( 
    protected IResult UpsertMethod(
        [FromRoute] string entity,
        [FromBody] JsonElement body
        )
    {
        return Results.NoContent();
    }
}
