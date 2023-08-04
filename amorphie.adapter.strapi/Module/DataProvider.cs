using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using amorphie.core.Module.minimal_api;
using amorphie.core.Base;


public class DataProviderModule : BaseRoute
{
    public DataProviderModule(WebApplication app)
        : base(app)
    {

    }

    public override string? UrlFragment => "data";

    public override void AddRoutes(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapGet("{entity}", SearchMethod);
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

        string result;

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
