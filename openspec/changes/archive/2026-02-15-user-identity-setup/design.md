## Context

The application requires user identity management as the foundation for all authenticated features (chat, API key management, football data access). ASP.NET Identity provides the user store, password hashing, and role management. The User entity must extend `IdentityUser<Guid>` to integrate with Identity while adding domain-specific fields (BYOK encrypted API key, audit fields). A flat permission model (all registered users get all permissions) is sufficient for the current single-role design.

Existing infrastructure already in place: Entity base class, EntityConfiguration base, GolMetricsDbContext, MediatR pipeline, Result pattern, exception handlers.

## Goals / Non-Goals

**Goals:**
- Define User entity with Identity integration and audit fields
- Configure EF Core mapping for User with unique email, concurrency token, encrypted key constraints
- Establish permission constants and authorization handler for JWT-based permission enforcement
- Register Identity, JWT Bearer, and authorization services in DI
- Provide `RequirePermissions()` endpoint extension for declarative authorization

**Non-Goals:**
- Role-based access control with multiple roles (single role model only)
- User registration/login endpoints (covered by authentication spec, TICK-005)
- User profile or API key management endpoints (covered by user-management spec)
- Refresh token support
- External identity providers (OAuth, OIDC)

## Decisions

### User entity extends IdentityUser<Guid> directly (not Entity base)

**Decision**: User does not inherit from the abstract `Entity` base class. Instead, it extends `IdentityUser<Guid>` and manually declares audit fields (`CreatedBy`, `LastModifiedBy`, `CreatedAtUtc`, `UpdatedAtUtc`, `Version`).

**Rationale**: C# does not support multiple inheritance. `IdentityUser<Guid>` already provides `Id` (Guid), so inheriting from `Entity` would create a conflict. Duplicating audit fields on User is the simplest approach.

**Alternative considered**: Composition via an owned type for audit fields. Rejected because it adds complexity for a single entity edge case.

### UserConfiguration implements IEntityTypeConfiguration directly

**Decision**: `UserConfiguration` implements `IEntityTypeConfiguration<User>` rather than extending `EntityConfiguration<TEntity>`.

**Rationale**: `EntityConfiguration<TEntity>` expects `TEntity : Entity`, but `User : IdentityUser<Guid>`. The configuration manually maps the same audit/concurrency columns.

### Flat permission model with JWT claims

**Decision**: All permissions are embedded as individual `permissions` claims in the JWT. All registered users receive all permissions.

**Rationale**: Simplest model for the current requirements. Permission checks happen entirely from the JWT without database lookups, which is fast and stateless. The `PermissionAuthorizationHandler` reads claims and matches against endpoint requirements.

**Alternative considered**: Database-backed role-permission mapping. Rejected because the single-role model makes this unnecessary overhead.

### Permission enforcement via RequirePermissions() extension

**Decision**: Endpoints declare required permissions via `RequirePermissions(Permissions.Users.Read)` which creates an inline authorization policy with `PermissionRequirement`.

**Rationale**: Declarative, readable, and consistent. Avoids named policy proliferation while keeping authorization logic in one place.

## Risks / Trade-offs

- **[Flat permission model limits granularity]** All users get all permissions. If different user tiers are needed later, the permission model must be extended to support role-permission mapping. Mitigation: The `PermissionAuthorizationHandler` already checks individual claims, so adding role-based permission assignment only requires changing token generation.

- **[JWT permissions are static until token refresh]** If permissions change, users must re-authenticate to get updated claims. Mitigation: Acceptable for the current single-role model where permissions never change per-user.

- **[User audit fields duplicated from Entity]** The User entity manually declares audit fields instead of inheriting from Entity. Mitigation: Only one entity has this pattern; the `AuditableEntityInterceptor` handles both Entity-derived and User entities via convention.
