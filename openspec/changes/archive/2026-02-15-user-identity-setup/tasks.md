## 1. User Entity and Configuration

- [x] 1.1 Create `src/GolMetrics.API/Features/UserManagement/User.cs` - `public sealed class User : IdentityUser<Guid>` with `EncryptedApiKey`, audit fields (`CreatedBy`, `LastModifiedBy`, `CreatedAtUtc`, `UpdatedAtUtc`), and `Version` (uint)
- [x] 1.2 Create `src/GolMetrics.API/Features/UserManagement/UserConfiguration.cs` - `internal sealed class UserConfiguration : IEntityTypeConfiguration<User>` with unique index on `Email`, `EncryptedApiKey` max length 512, `Version` as row version, `CreatedBy` and `CreatedAtUtc` as required

## 2. Authorization Infrastructure

- [x] 2.1 Create `src/GolMetrics.API/Core/Authorization/Permissions.cs` - `public static class Permissions` with nested `Conversations` (`Read`, `Write`) and `Users` (`Read`, `Write`) constants in `resource:action` format
- [x] 2.2 Create `src/GolMetrics.API/Core/Authorization/PermissionRequirement.cs` - `public sealed class PermissionRequirement(string[] permissions) : IAuthorizationRequirement` with `Permissions` property
- [x] 2.3 Create `src/GolMetrics.API/Core/Authorization/PermissionAuthorizationHandler.cs` - `internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>` that reads `permissions` claims from JWT and succeeds if all required permissions are present
- [x] 2.4 Create `src/GolMetrics.API/Core/Authorization/EndpointExtensions.cs` - `public static class EndpointExtensions` with `RequirePermissions(this RouteHandlerBuilder, params string[])` that calls `RequireAuthorization` with Bearer scheme and `PermissionRequirement`

## 3. Database Context

- [x] 3.1 Create `src/GolMetrics.API/Core/Persistence/GolMetricsDbContext.cs` - `public class GolMetricsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>` with `DbSet<Conversation>`, `DbSet<Message>`, `DbSet<CachedQuery>`, and `OnModelCreating` applying configurations from assembly

## 4. Dependency Injection

- [x] 4.1 Add `AddAuthenticationServices()` extension method in `src/GolMetrics.API/DependencyInjection.cs` - register `Identity<User, IdentityRole<Guid>>` with EF stores, configure JWT Bearer with issuer/audience/signing key validation from config, register `PermissionAuthorizationHandler` as singleton, register `ICurrentUserService`/`CurrentUserService` as scoped, register `IHttpContextAccessor`

## 5. Verification

- [x] 5.1 Run `dotnet build src/GolMetrics.API/` and verify no compilation errors
- [x] 5.2 Run `dotnet test tests/GolMetrics.API.Tests/` and verify all existing tests pass
