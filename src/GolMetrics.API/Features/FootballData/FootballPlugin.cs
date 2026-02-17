using System.ComponentModel;
using GolMetrics.API.Core.Abstractions;
using Microsoft.SemanticKernel;

namespace GolMetrics.API.Features.FootballData;

internal sealed class FootballPlugin(IFootballApiClient footballApiClient)
{
    [KernelFunction("GetTopScorers")]
    [Description("Gets the top scorers for a specific league and season")]
    public async Task<string> GetTopScorersAsync(
        [Description("The league ID (e.g., 39 for Premier League, 140 for La Liga)")]
        int leagueId,
        [Description("The season year (e.g., 2024)")]
        int season)
    {
        var result = await footballApiClient.GetTopScorersAsync(leagueId, season);
        return result.IsSuccess ? result.Value! : result.Error!.Message;
    }

    [KernelFunction("GetStandings")]
    [Description("Gets the league standings for a specific league and season")]
    public async Task<string> GetStandingsAsync(
        [Description("The league ID (e.g., 39 for Premier League, 140 for La Liga)")]
        int leagueId,
        [Description("The season year (e.g., 2024)")]
        int season)
    {
        var result = await footballApiClient.GetStandingsAsync(leagueId, season);
        return result.IsSuccess ? result.Value! : result.Error!.Message;
    }

    [KernelFunction("GetRecentResults")]
    [Description("Gets the most recent match results for a specific team")]
    public async Task<string> GetRecentResultsAsync(
        [Description("The team ID")] int teamId,
        [Description("Number of recent matches to retrieve")]
        int last)
    {
        var result = await footballApiClient.GetRecentResultsAsync(teamId, last);
        return result.IsSuccess ? result.Value! : result.Error!.Message;
    }

    [KernelFunction("GetUpcomingMatches")]
    [Description("Gets upcoming matches for a specific league, optionally filtered by team")]
    public async Task<string> GetUpcomingMatchesAsync(
        [Description("The league ID (e.g., 39 for Premier League, 140 for La Liga)")]
        int leagueId,
        [Description("Optional team ID to filter matches")]
        int? teamId = null,
        [Description("Start date in YYYY-MM-DD format")]
        string? fromDate = null)
    {
        var result = await footballApiClient.GetUpcomingMatchesAsync(leagueId, teamId,
            fromDate ?? DateTime.UtcNow.ToString("yyyy-MM-dd"));
        return result.IsSuccess ? result.Value! : result.Error!.Message;
    }

    [KernelFunction("GetTeamStatistics")]
    [Description("Gets detailed statistics for a specific team in a league and season")]
    public async Task<string> GetTeamStatisticsAsync(
        [Description("The team ID")] int teamId,
        [Description("The league ID")] int leagueId,
        [Description("The season year (e.g., 2024)")]
        int season)
    {
        var result = await footballApiClient.GetTeamStatisticsAsync(teamId, leagueId, season);
        return result.IsSuccess ? result.Value! : result.Error!.Message;
    }
}