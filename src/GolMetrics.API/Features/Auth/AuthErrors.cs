using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Features.Auth;

public static class AuthErrors
{
    public static Error DuplicateEmail => new(
        "Auth.DuplicateEmail",
        "A user with this email already exists.",
        ErrorCategory.Conflict);

    public static Error InvalidPassword => new(
        "Auth.InvalidPassword",
        "The password does not meet the required format.",
        ErrorCategory.BadRequest);

    public static Error InvalidCredentials => new(
        "Auth.InvalidCredentials",
        "Invalid email or password.",
        ErrorCategory.Unauthorized);

    public static Error InvalidRefreshToken => new(
        "Auth.InvalidRefreshToken",
        "The refresh token is invalid, expired, or has been revoked.",
        ErrorCategory.Unauthorized);
}
