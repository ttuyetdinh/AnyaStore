using AnyaStore.Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOcelot();

builder.AddAppAuthentication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
