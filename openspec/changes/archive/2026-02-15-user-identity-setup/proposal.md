## Why

TICK-004: The application needs a User entity integrated with ASP.NET Identity to support authentication, authorization, and BYOK API key storage. This is foundational infrastructure that all protected features depend on.

## What Changes

- Define `User` entity extending `IdentityUser<Guid>` with audit fields and BYOK `EncryptedApiKey` property
- Configure `GolMetricsDbContext` as `IdentityDbContext<User, IdentityRole<Guid>, Guid>` with snake_case naming, enum-as-string, and audit field automation
- Implement `UserConfiguration` for EF Core entity mapping (unique email index, encrypted key constraints, concurrency token)
- Define permission constants (`Permissions` static class) with `conversations:read/write` and `user:read/write`
- Implement `PermissionAuthorizationHandler` to check JWT `permissions` claims against endpoint requirements
- Provide `RequirePermissions()` extension method for endpoint authorization
- Register ASP.NET Identity, JWT Bearer authentication, and authorization handler in DI

## Capabilities

### New Capabilities
- `user-identity`: User entity definition, ASP.NET Identity integration, permission constants, and permission-based authorization infrastructure

### Modified Capabilities

## Impact

- `src/GolMetrics.API/Features/UserManagement/User.cs` - new entity
- `src/GolMetrics.API/Features/UserManagement/UserConfiguration.cs` - new EF config
- `src/GolMetrics.API/Core/Authorization/Permissions.cs` - permission constants
- `src/GolMetrics.API/Core/Authorization/PermissionRequirement.cs` - authorization requirement
- `src/GolMetrics.API/Core/Authorization/PermissionAuthorizationHandler.cs` - authorization handler
- `src/GolMetrics.API/Core/Authorization/EndpointExtensions.cs` - RequirePermissions extension
- `src/GolMetrics.API/Core/Persistence/GolMetricsDbContext.cs` - Identity DbContext
- `src/GolMetrics.API/DependencyInjection.cs` - Identity and auth service registration
- NuGet: `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.AspNetCore.Authentication.JwtBearer`
- EF Core migration for Identity tables + User entity
