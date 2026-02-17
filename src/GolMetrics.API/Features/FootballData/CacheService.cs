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
    public async Task<string?> GetAsync(
        string endpoint,
        Dictionary<string, string> parameters,
        CancellationToken cancellationToken = default)
    {
        var queryHash = GenerateKey(endpoint, parameters);
        var now = timeProvider.GetUtcNow().UtcDateTime;

        var cached = await dbContext.CachedQueries
            .FirstOrDefaultAsync(c => c.QueryHash == queryHash, cancellationToken);

        return cached is not null && cached.ExpiresAt > now ? cached.ResponseData : null;
    }

    public async Task SetAsync(
        string endpoint,
        Dictionary<string, string> parameters,
        string value,
        TimeSpan ttl,
        CancellationToken cancellationToken = default)
    {
        var queryHash = GenerateKey(endpoint, parameters);
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var sortedParams = JsonSerializer.Serialize(
            parameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));

        try
        {
            var cached = await dbContext.CachedQueries
                .FirstOrDefaultAsync(c => c.QueryHash == queryHash, cancellationToken);

            if (cached is not null)
            {
                cached.ResponseData = value;
                cached.ExpiresAt = now + ttl;
            }
            else
            {
                dbContext.CachedQueries.Add(new CachedQuery
                {
                    QueryHash = queryHash,
                    Endpoint = endpoint,
                    Params = sortedParams,
                    ResponseData = value,
                    ExpiresAt = now + ttl
                });
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to persist cache entry for endpoint {Endpoint}", endpoint);
        }
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