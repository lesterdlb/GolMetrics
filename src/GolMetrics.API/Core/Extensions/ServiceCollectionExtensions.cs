using System.Reflection;
using GolMetrics.API.Core.Abstractions;

namespace GolMetrics.API.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSlices(this IServiceCollection services)
    {
        var sliceTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                        && typeof(ISlice).IsAssignableFrom(t));

        foreach (var type in sliceTypes)
        {
            services.AddTransient(typeof(ISlice), type);
        }

        return services;
    }
}