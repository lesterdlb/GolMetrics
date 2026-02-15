using GolMetrics.API.Core.Abstractions;

namespace GolMetrics.API.Features.FootballData;

internal sealed class FootballApiClient(HttpClient httpClient) : IFootballApiClient
{
    public Task<string> GetTopScorersAsync(int leagueId, int season, CancellationToken cancellationToken = default)
        => httpClient.GetStringAsync($"/players/topscorers?league={leagueId}&season={season}", cancellationToken);

    public Task<string> GetStandingsAsync(int leagueId, int season, CancellationToken cancellationToken = default)
        => httpClient.GetStringAsync($"/standings?league={leagueId}&season={season}", cancellationToken);

    public Task<string> GetRecentResultsAsync(int teamId, int last, CancellationToken cancellationToken = default)
        => httpClient.GetStringAsync($"/fixtures?team={teamId}&last={last}", cancellationToken);

    public Task<string> GetUpcomingMatchesAsync(int leagueId, int? teamId, string fromDate,
        CancellationToken cancellationToken = default)
    {
        var url = $"/fixtures?league={leagueId}&next=10";
        if (teamId.HasValue)
            url += $"&team={teamId.Value}";
        return httpClient.GetStringAsync(url, cancellationToken);
    }

    public Task<string> GetTeamStatisticsAsync(int teamId, int leagueId, int season,
        CancellationToken cancellationToken = default)
        => httpClient.GetStringAsync($"/teams/statistics?team={teamId}&league={leagueId}&season={season}",
            cancellationToken);

    public async Task<bool> ValidateApiKeyAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/status");
        request.Headers.Add("x-apisports-key", apiKey);
        var response = await httpClient.SendAsync(request, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}