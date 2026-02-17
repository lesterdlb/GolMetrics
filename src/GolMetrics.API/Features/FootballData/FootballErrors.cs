using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Features.FootballData;

public static class FootballErrors
{
    public static Error RateLimitExceeded => new(
        "Football.RateLimitExceeded",
        "API-Football rate limit exceeded. Please try again later.",
        ErrorCategory.BadRequest);

    public static Error ApiUnavailable => new(
        "Football.ApiUnavailable",
        "API-Football service is currently unavailable.",
        ErrorCategory.BadRequest);

    public static Error InvalidParameters => new(
        "Football.InvalidParameters",
        "The provided parameters returned an error from API-Football.",
        ErrorCategory.BadRequest);

    public static Error AiServiceUnavailable => new(
        "Football.AiServiceUnavailable",
        "The AI service is currently unavailable.",
        ErrorCategory.BadRequest);
}