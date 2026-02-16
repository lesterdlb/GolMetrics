using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Core.Abstractions;

public interface IFootballApiClient
{
    Task<Result<string>> GetTopScorersAsync(int leagueId, int season, CancellationToken cancellationToken = default);
    Task<Result<string>> GetStandingsAsync(int leagueId, int season, CancellationToken cancellationToken = default);
    Task<Result<string>> GetRecentResultsAsync(int teamId, int last, CancellationToken cancellationToken = default);

    Task<Result<string>> GetUpcomingMatchesAsync(int leagueId, int? teamId, string fromDate,
        CancellationToken cancellationToken = default);

    Task<Result<string>> GetTeamStatisticsAsync(int teamId, int leagueId, int season,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default);
}