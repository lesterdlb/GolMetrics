namespace GolMetrics.API.Features.FootballData;

public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(
        string endpoint,
        Dictionary<string, string> parameters,
        Func<Task<T>> fetchFactory,
        TimeSpan ttl,
        CancellationToken cancellationToken = default);
}