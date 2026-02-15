using GolMetrics.API;
using GolMetrics.API.Core.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApiServices()
    .AddDatabase()
    .AddAuthenticationServices()
    .AddErrorHandling()
    .AddCors();

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapSliceEndpoints();

await app.RunAsync();