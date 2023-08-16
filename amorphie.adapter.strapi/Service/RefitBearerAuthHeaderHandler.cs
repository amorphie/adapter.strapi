using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace amorphie.adapter.strapi.Service;

public class RefitBearerAuthHeaderHandler : DelegatingHandler
{

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("STRAPI_TOKEN") ?? throw new ArgumentNullException("Parameter is not suplied as enviroment variable", "STRAPI_TOKEN"));
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
