using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Refit;

namespace amorphie.adapter.strapi.Service;

public interface IStrapiService
{
    [Get("/api/{entity}?pagination[page]={page}&pagination[pageSize]={pageSize}&populate=*")]
    Task<JsonNode> GetEntity(string entity,int page,int pageSize);
}
