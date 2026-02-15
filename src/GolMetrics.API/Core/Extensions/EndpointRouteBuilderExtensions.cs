using GolMetrics.API.Core.Abstractions;

namespace GolMetrics.API.Core.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapSliceEndpoints(this IEndpointRouteBuilder routes)
    {
        var slices = routes.ServiceProvider.GetRequiredService<IEnumerable<ISlice>>();

        foreach (var slice in slices)
        {
            slice.RegisterEndpoints(routes);
        }

        return routes;
    }
}