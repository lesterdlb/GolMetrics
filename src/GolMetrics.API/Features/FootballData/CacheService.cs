using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GolMetrics.API.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Features.FootballData;

internal sealed class CacheService(
    GolMetricsDbContext dbContext,
    TimeProvider timeProvider,
    ILogger<CacheService> logger) : ICacheService
{
    public async Task<T> GetOrSetAsync<T>(
        string endpoint,
        Dictionary<string, string> parameters,
        Func<Task<T>> fetchFactory,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
    {
        var queryHash = GenerateKey(endpoint, parameters);
        var now = timeProvider.GetUtcNow().UtcDateTime;

        var cached = await dbContext.CachedQueries
            .FirstOrDefaultAsync(c => c.QueryHash == queryHash, cancellationToken);

        if (cached is not null && cached.ExpiresAt > now)
        {
            return JsonSerializer.Deserialize<T>(cached.ResponseData)!;
        }

        var data = await fetchFactory();
        var serialized = JsonSerializer.Serialize(data);
        var sortedParams = JsonSerializer.Serialize(
            parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));

        try
        {
            if (cached is not null)
            {
                cached.ResponseData = serialized;
                cached.ExpiresAt = now + ttl;
            }
            else
            {
                dbContext.CachedQueries.Add(new CachedQuery
                {
                    QueryHash = queryHash,
                    Endpoint = endpoint,
                    Params = sortedParams,
                    ResponseData = serialized,
                    ExpiresAt = now + ttl
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to persist cache entry for endpoint {Endpoint}", endpoint);
        }

        return data;
    }

    private static string GenerateKey(string endpoint, Dictionary<string, string> parameters)
    {
        var sorted = parameters.OrderBy(p => p.Key);
        var raw = new StringBuilder(endpoint);

        foreach (var (key, value) in sorted)
        {
            raw.Append(key);
            raw.Append(value);
        }

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw.ToString()));
        return Convert.ToHexStringLower(hash);
    }
}