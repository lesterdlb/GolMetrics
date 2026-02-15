namespace GolMetrics.API.Core.Authorization;

public static class EndpointExtensions
{
    public static RouteHandlerBuilder RequirePermissions(
        this RouteHandlerBuilder builder,
        params string[] permissions)
    {
        return builder.RequireAuthorization(policy =>
        {
            policy.AddAuthenticationSchemes("Bearer");
            policy.AddRequirements(new PermissionRequirement(permissions));
        });
    }
}