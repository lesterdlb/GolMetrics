using Microsoft.AspNetCore.Authorization;

namespace GolMetrics.API.Core.Authorization;

public sealed class PermissionRequirement(string[] permissions) : IAuthorizationRequirement
{
    public string[] Permissions { get; } = permissions;
}