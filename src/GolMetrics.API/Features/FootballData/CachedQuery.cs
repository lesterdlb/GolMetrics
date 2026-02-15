using GolMetrics.API.Core.Abstractions;

namespace GolMetrics.API.Features.FootballData;

public sealed class CachedQuery : Entity
{
    public string QueryHash { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string Params { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}