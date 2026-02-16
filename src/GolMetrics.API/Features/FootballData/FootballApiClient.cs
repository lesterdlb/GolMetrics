using System.Net;
using System.Text.Json;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Features.FootballData;

internal sealed class FootballApiClient(
    HttpClient httpClient,
    ICacheService cacheService) : IFootballApiClient
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(1);

    public Task<Result<string>> GetTopScorersAsync(int leagueId, int season,
        CancellationToken cancellationToken = default)
        => CachedRequestAsync(
            "/players/topscorers",
            new Dictionary<string, string>
            {
                ["league"] = leagueId.ToString(),
                ["season"] = season.ToString()
            },
            cancellationToken);

    public Task<Result<string>> GetStandingsAsync(int leagueId, int season,
        CancellationToken cancellationToken = default)
        => CachedRequestAsync(
            "/standings",
            new Dictionary<string, string>
            {
                ["league"] = leagueId.ToString(),
                ["season"] = season.ToString()
            },
            cancellationToken);

    public Task<Result<string>> GetRecentResultsAsync(int teamId, int last,
        CancellationToken cancellationToken = default)
        => CachedRequestAsync(
            "/fixtures",
            new Dictionary<string, string>
            {
                ["team"] = teamId.ToString(),
                ["last"] = last.ToString()
            },
            cancellationToken);

    public Task<Result<string>> GetUpcomingMatchesAsync(int leagueId, int? teamId, string fromDate,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["league"] = leagueId.ToString(),
            ["next"] = "10"
        };

        if (teamId.HasValue)
            parameters["team"] = teamId.Value.ToString();

        return CachedRequestAsync("/fixtures", parameters, cancellationToken);
    }

    public Task<Result<string>> GetTeamStatisticsAsync(int teamId, int leagueId, int season,
        CancellationToken cancellationToken = default)
        => CachedRequestAsync(
            "/teams/statistics",
            new Dictionary<string, string>
            {
                ["team"] = teamId.ToString(),
                ["league"] = leagueId.ToString(),
                ["season"] = season.ToString()
            },
            cancellationToken);

    public async Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/status");
        request.Headers.Add("x-apisports-key", apiKey);
        var response = await httpClient.SendAsync(request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    private async Task<Result<string>> CachedRequestAsync(
        string endpoint,
        Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        return await cacheService.GetOrSetAsync(
            endpoint,
            parameters,
            () => ExecuteRequestAsync(endpoint, parameters, cancellationToken),
            CacheTtl,
            cancellationToken);
    }

    private async Task<Result<string>> ExecuteRequestAsync(
        string endpoint,
        Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        var url = $"{endpoint}?{queryString}";

        var response = await httpClient.GetAsync(url, cancellationToken);

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
            return Result<string>.Failure(FootballErrors.RateLimitExceeded);

        if ((int)response.StatusCode >= 500)
            return Result<string>.Failure(FootballErrors.ApiUnavailable);

        if (response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var remainingValues)
            && int.TryParse(remainingValues.FirstOrDefault(), out var remaining)
            && remaining == 0)
            return Result<string>.Failure(FootballErrors.RateLimitExceeded);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (HasApiErrors(body))
            return Result<string>.Failure(FootballErrors.InvalidParameters);

        return body;
    }

    private static bool HasApiErrors(string responseBody)
    {
        try
        {
            using var document = JsonDocument.Parse(responseBody);
            if (document.RootElement.TryGetProperty("errors", out var errors))
            {
                return errors.ValueKind switch
                {
                    JsonValueKind.Object => errors.EnumerateObject().Any(),
                    JsonValueKind.Array => errors.GetArrayLength() > 0,
                    _ => false
                };
            }

            return false;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}