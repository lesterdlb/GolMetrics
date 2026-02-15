namespace GolMetrics.API.Core.Abstractions;

public interface ISlice
{
    void RegisterEndpoints(IEndpointRouteBuilder routes);
}