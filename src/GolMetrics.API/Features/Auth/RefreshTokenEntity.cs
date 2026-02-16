using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.UserManagement;

namespace GolMetrics.API.Features.Auth;

public sealed class RefreshTokenEntity : Entity
{
    public required string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsRevoked { get; set; }
    public User User { get; set; } = null!;
}