using Battleship.Api;
using Battleship.Api.Endpoints;
using Battleship.Application;
using Battleship.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

app.UseExceptionHandler(_ => { })
   .UseHttpsRedirection()
   .UseCors()
   .UseSwagger()
   .UseSwaggerUI();

app.MapGameEndpoints();

await app.RunAsync();
