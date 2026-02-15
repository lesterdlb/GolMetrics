namespace GolMetrics.API.Core.Abstractions;

public interface IFootballApiClient
{
    Task<string> GetTopScorersAsync(int leagueId, int season, CancellationToken cancellationToken = default);
    Task<string> GetStandingsAsync(int leagueId, int season, CancellationToken cancellationToken = default);
    Task<string> GetRecentResultsAsync(int teamId, int last, CancellationToken cancellationToken = default);

    Task<string> GetUpcomingMatchesAsync(int leagueId, int? teamId, string fromDate,
        CancellationToken cancellationToken = default);

    Task<string> GetTeamStatisticsAsync(int teamId, int leagueId, int season,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default);
}