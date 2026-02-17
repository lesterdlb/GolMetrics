namespace GolMetrics.API.Features.FootballData;

public interface ICacheService
{
    Task<string?> GetAsync(
        string endpoint,
        Dictionary<string, string> parameters,
        CancellationToken cancellationToken = default);

    Task SetAsync(
        string endpoint,
        Dictionary<string, string> parameters,
        string value,
        TimeSpan ttl,
        CancellationToken cancellationToken = default);
}