using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Features.UserManagement;

public static class UserErrors
{
    public static Error InvalidApiKey => new(
        "User.InvalidApiKey",
        "The provided API key is not valid.",
        ErrorCategory.BadRequest);

    public static Error UserNotFound => new(
        "User.UserNotFound",
        "The user was not found.",
        ErrorCategory.NotFound);

    public static Error ApiValidationUnavailable => new(
        "User.ApiValidationUnavailable",
        "Unable to validate the API key. The external service is unavailable.",
        ErrorCategory.BadRequest);
}