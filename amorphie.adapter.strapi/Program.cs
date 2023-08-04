using amorphie.adapter.strapi.Service;
using amorphie.core.Extension;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Refit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataAdapter, StrapiDataAdapter>();

builder.Services.AddTransient<RefitBearerAuthHeaderHandler>();

// wait 1s and retry again 3 times when get timeout
AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .Or<TimeoutRejectedException>()
    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000));

builder.Services
    .AddRefitClient<IStrapiService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("STRAPI_URL") ?? throw new ArgumentNullException("Parameter is not suplied as enviroment variable", "STRAPI_URL")))
    .AddHttpMessageHandler<RefitBearerAuthHeaderHandler>()
    .AddPolicyHandler(retryPolicy);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.AddRoutes();

app.Run();
