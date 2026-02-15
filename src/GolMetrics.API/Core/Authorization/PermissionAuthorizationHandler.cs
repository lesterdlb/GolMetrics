using Microsoft.AspNetCore.Authorization;

namespace GolMetrics.API.Core.Authorization;

internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private const string PermissionsClaimType = "permissions";

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userPermissions = context.User.Claims
            .Where(c => c.Type == PermissionsClaimType)
            .Select(c => c.Value)
            .ToHashSet();

        if (requirement.Permissions.All(p => userPermissions.Contains(p)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}