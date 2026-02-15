## ADDED Requirements

### Requirement: User Entity Definition

The system SHALL define a `User` entity in `Features/UserManagement/User.cs` extending `IdentityUser<Guid>` with domain-specific properties.

#### Scenario: User entity properties

- **WHEN** a User entity is defined
- **THEN** it SHALL be a `public sealed class` extending `IdentityUser<Guid>`
- **AND** it SHALL include `EncryptedApiKey` (string, nullable) for BYOK API key storage
- **AND** it SHALL include audit fields: `CreatedBy` (Guid), `LastModifiedBy` (Guid, nullable), `CreatedAtUtc` (DateTime), `UpdatedAtUtc` (DateTime, nullable)
- **AND** it SHALL include `Version` (uint) for optimistic concurrency

### Requirement: User Entity Configuration

The system SHALL define `UserConfiguration` implementing `IEntityTypeConfiguration<User>` in `Features/UserManagement/UserConfiguration.cs`.

#### Scenario: Entity mapping configuration

- **WHEN** the User entity is configured by EF Core
- **THEN** it SHALL have a unique index on `Email`
- **AND** `EncryptedApiKey` SHALL have a max length of 512
- **AND** `Version` SHALL be configured as a row version concurrency token
- **AND** `CreatedBy` SHALL be configured as required
- **AND** `CreatedAtUtc` SHALL be configured as required

### Requirement: Permission Constants

The system SHALL define permission constants in `Core/Authorization/Permissions.cs` as a `public static class` with nested resource classes.

#### Scenario: Conversation permissions

- **WHEN** conversation authorization is needed
- **THEN** `Permissions.Conversations.Read` SHALL equal `"conversations:read"`
- **AND** `Permissions.Conversations.Write` SHALL equal `"conversations:write"`

#### Scenario: User permissions

- **WHEN** user management authorization is needed
- **THEN** `Permissions.Users.Read` SHALL equal `"user:read"`
- **AND** `Permissions.Users.Write` SHALL equal `"user:write"`

### Requirement: Permission Requirement

The system SHALL define `PermissionRequirement` implementing `IAuthorizationRequirement` in `Core/Authorization/PermissionRequirement.cs`.

#### Scenario: Requirement construction

- **WHEN** a `PermissionRequirement` is created with an array of permission strings
- **THEN** it SHALL store the permissions for evaluation by the authorization handler

### Requirement: Permission Authorization Handler

The system SHALL define `PermissionAuthorizationHandler` extending `AuthorizationHandler<PermissionRequirement>` in `Core/Authorization/PermissionAuthorizationHandler.cs`.

#### Scenario: All required permissions present

- **WHEN** the authenticated user's JWT contains all required `permissions` claims
- **THEN** the handler SHALL call `context.Succeed(requirement)`
- **AND** the request SHALL proceed to the endpoint handler

#### Scenario: Missing required permissions

- **WHEN** the authenticated user's JWT does not contain all required `permissions` claims
- **THEN** the handler SHALL not call `context.Succeed()`
- **AND** the authorization middleware SHALL return HTTP 403 Forbidden

#### Scenario: Unauthenticated request

- **WHEN** an unauthenticated request reaches a permission-protected endpoint
- **THEN** the authorization middleware SHALL return HTTP 401 Unauthorized

### Requirement: RequirePermissions Endpoint Extension

The system SHALL provide a `RequirePermissions()` extension method on `RouteHandlerBuilder` in `Core/Authorization/EndpointExtensions.cs`.

#### Scenario: Applying permission requirement to endpoint

- **WHEN** a slice calls `RequirePermissions(params string[] permissions)` on a route builder
- **THEN** it SHALL call `RequireAuthorization()` with a policy that adds the Bearer authentication scheme
- **AND** it SHALL add a `PermissionRequirement` with the specified permissions

### Requirement: Identity and Authentication Registration

The system SHALL register ASP.NET Identity, JWT Bearer authentication, and authorization services in `DependencyInjection.cs`.

#### Scenario: Identity registration

- **WHEN** `AddAuthenticationServices()` is called on `WebApplicationBuilder`
- **THEN** it SHALL register `Identity<User, IdentityRole<Guid>>` with EF Core stores and default token providers
- **AND** it SHALL configure JWT Bearer as the default authentication scheme
- **AND** it SHALL validate issuer, audience, lifetime, and signing key from configuration
- **AND** it SHALL register `PermissionAuthorizationHandler` as a singleton `IAuthorizationHandler`
- **AND** it SHALL register `ICurrentUserService` / `CurrentUserService` as scoped
- **AND** it SHALL register `IHttpContextAccessor`
