namespace GolMetrics.API.Core.Abstractions;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
}