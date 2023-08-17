using amorphie.adapter.strapi.Service;
using amorphie.core.Extension;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Refit;

Environment.SetEnvironmentVariable("STRAPI_URL", "http://localhost:1337");
Environment.SetEnvironmentVariable("STRAPI_TOKEN", "2969433ddff5a1e8c11f05c21fe7aedb211e6d104c62fbd1afabf7c256945fc519ff2c92fbdac94ca551a1facdf8cdee53f3e9defbfd4134178518d59127a321eef9f6fdef54f65a368e18e54b53dd77f7a09b24a0d9cc8fa3c9d9a2f51b02071d86c9d3f99a599d10c072aee27b6d7b43cf6f43e10e25dc85b4a59068e998a5");

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
