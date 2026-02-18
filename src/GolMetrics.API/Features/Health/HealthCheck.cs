using GolMetrics.API.Core.Abstractions;

namespace GolMetrics.API.Features.Health;

internal sealed class HealthCheck : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/health", () => Results.Ok())
            .WithName("HealthCheck")
            .AllowAnonymous();
    }
}