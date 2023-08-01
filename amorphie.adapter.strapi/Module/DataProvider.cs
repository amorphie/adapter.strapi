using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Module.minimal_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Text.Json;

namespace novartz.socialize.Module;

public class StudentModule : BaseRoute
{
    public StudentModule(WebApplication app)
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
        [AsParameters] DataProviderSearch searchData
        )
    {
        return Results.NoContent();
    }


     protected async ValueTask<IResult> UpsertMethod( 
        [FromRoute] string entity,   
        [FromBody] JsonElement body
        )
    {
        return  Results.NoContent();
    }
}
