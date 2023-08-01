using amorphie.core.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataAdapter, StrapiDataAdapter>();

builder.Services.AddHttpClient();


var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();


app.AddRoutes();

app.Run();
