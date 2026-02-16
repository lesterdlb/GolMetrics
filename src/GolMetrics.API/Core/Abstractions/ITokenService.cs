using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.UserManagement;

namespace GolMetrics.API.Core.Abstractions;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<Guid>> ValidateRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokenAsync(string token, CancellationToken cancellationToken = default);
}