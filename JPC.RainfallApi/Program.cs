using JPC.RainfallApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddSingleton(configuration);

// Use the Startup class for configuration
var startup = new Startup(configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
var env = app.Environment;

if (env.IsDevelopment() || env.IsProduction())
{
    startup.Configure(app, env);
}

app.Run();
