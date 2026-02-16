## ADDED Requirements

### Requirement: User Registration Slice

The system SHALL provide a Register slice at `Features/Auth/Register.cs` implementing `ISlice` as an `internal sealed class`.

#### Scenario: Successful registration

- **WHEN** a valid `POST /api/auth/register` request is received with `email` and `password`
- **THEN** the system SHALL create a new user via `UserManager<User>.CreateAsync()`
- **AND** it SHALL generate an access token and refresh token via `ITokenService`
- **AND** it SHALL return HTTP 201 with `AccessToken`, `RefreshToken`, and `ExpiresAtUtc`

#### Scenario: Duplicate email

- **WHEN** a registration request is received with an email that already exists
- **THEN** the system SHALL return `Result.Failure(AuthErrors.DuplicateEmail)`
- **AND** the endpoint SHALL return HTTP 409 Conflict via `ToProblemDetails()`

#### Scenario: Invalid password format

- **WHEN** a registration request is received with a password that does not meet ASP.NET Identity requirements
- **THEN** the system SHALL return `Result.Failure(AuthErrors.InvalidPassword)`
- **AND** the endpoint SHALL return HTTP 400 Bad Request via `ToProblemDetails()`

#### Scenario: Request validation

- **WHEN** a registration request is received with empty email or password
- **THEN** FluentValidation SHALL reject the request before the handler executes
- **AND** the system SHALL return HTTP 400 Bad Request via the `ValidationBehavior` pipeline

### Requirement: User Login Slice

The system SHALL provide a Login slice at `Features/Auth/Login.cs` implementing `ISlice` as an `internal sealed class`.

#### Scenario: Successful login

- **WHEN** a valid `POST /api/auth/login` request is received with correct `email` and `password`
- **THEN** the system SHALL validate credentials via `SignInManager<User>.CheckPasswordSignInAsync()`
- **AND** it SHALL generate an access token and refresh token via `ITokenService`
- **AND** it SHALL return HTTP 200 with `AccessToken`, `RefreshToken`, and `ExpiresAtUtc`

#### Scenario: Invalid credentials

- **WHEN** a login request is received with incorrect email or password
- **THEN** the system SHALL return `Result.Failure(AuthErrors.InvalidCredentials)`
- **AND** the endpoint SHALL return HTTP 401 Unauthorized via `ToProblemDetails()`

#### Scenario: User not found

- **WHEN** a login request is received with an email that does not exist
- **THEN** the system SHALL return `Result.Failure(AuthErrors.InvalidCredentials)`
- **AND** the response SHALL NOT distinguish between wrong email and wrong password

### Requirement: Refresh Token Slice

The system SHALL provide a RefreshToken slice at `Features/Auth/RefreshToken.cs` implementing `ISlice` as an `internal sealed class`.

#### Scenario: Successful token refresh

- **WHEN** a valid `POST /api/auth/refresh-token` request is received with a valid, non-expired, non-revoked refresh token
- **THEN** the system SHALL revoke the existing refresh token
- **AND** it SHALL generate a new access token and refresh token pair via `ITokenService`
- **AND** it SHALL return HTTP 200 with the new `AccessToken`, `RefreshToken`, and `ExpiresAtUtc`

#### Scenario: Invalid or expired refresh token

- **WHEN** a refresh token request is received with an invalid, expired, or revoked token
- **THEN** the system SHALL return `Result.Failure(AuthErrors.InvalidRefreshToken)`
- **AND** the endpoint SHALL return HTTP 401 Unauthorized via `ToProblemDetails()`

### Requirement: Token Service

The system SHALL provide `ITokenService` in `Core/Abstractions/ITokenService.cs` and `TokenService` in `Core/Identity/TokenService.cs`.

#### Scenario: Access token generation

- **WHEN** `ITokenService.GenerateAccessToken(User user)` is called
- **THEN** it SHALL create a JWT signed with HMAC-SHA256
- **AND** it SHALL include claims: `sub` (user ID), `email`, and individual `permissions` claims for all permissions (`conversations:read`, `conversations:write`, `user:read`, `user:write`)
- **AND** the token SHALL have a 7-day expiry
- **AND** the signing key, issuer, and audience SHALL be read from `TokenOptions` configuration

#### Scenario: Refresh token generation

- **WHEN** `ITokenService.GenerateRefreshToken(Guid userId)` is called
- **THEN** it SHALL create a cryptographically random token string
- **AND** it SHALL persist a `RefreshToken` entity with `Token`, `UserId`, `ExpiresAtUtc` (30 days), and `IsRevoked = false`
- **AND** it SHALL return the token string

#### Scenario: Refresh token validation

- **WHEN** `ITokenService.ValidateRefreshToken(string token)` is called
- **THEN** it SHALL look up the token in the database
- **AND** it SHALL return the associated `UserId` if the token is valid, not expired, and not revoked
- **AND** it SHALL return `Result.Failure(AuthErrors.InvalidRefreshToken)` otherwise

### Requirement: Refresh Token Entity

The system SHALL define a `RefreshToken` entity at `Features/Auth/RefreshToken.cs` extending `Entity`.

#### Scenario: Entity properties

- **WHEN** a RefreshToken entity is defined
- **THEN** it SHALL include `Token` (string, required), `UserId` (Guid, required), `ExpiresAtUtc` (DateTime, required), `IsRevoked` (bool, default false)
- **AND** it SHALL have a navigation property to `User`

#### Scenario: Entity configuration

- **WHEN** the RefreshToken entity is configured by EF Core
- **THEN** it SHALL have a unique index on `Token`
- **AND** it SHALL have a foreign key to `User` via `UserId`
- **AND** it SHALL map to the `refresh_tokens` table

### Requirement: Authentication Errors

The system SHALL define authentication errors in `Features/Auth/AuthErrors.cs` as a `public static class`.

#### Scenario: Error definitions

- **WHEN** authentication errors are needed
- **THEN** `AuthErrors.DuplicateEmail` SHALL be an `Error` with `ErrorCategory.Conflict`
- **AND** `AuthErrors.InvalidPassword` SHALL be an `Error` with `ErrorCategory.BadRequest`
- **AND** `AuthErrors.InvalidCredentials` SHALL be an `Error` with `ErrorCategory.Unauthorized`
- **AND** `AuthErrors.InvalidRefreshToken` SHALL be an `Error` with `ErrorCategory.Unauthorized`

### Requirement: Auth Endpoint Names

The system SHALL define route constants for the RefreshToken endpoint in `EndpointNames.Auth`.

#### Scenario: RefreshToken route constant

- **WHEN** the RefreshToken endpoint route is needed
- **THEN** `EndpointNames.Auth.RefreshToken` SHALL equal `"RefreshToken"`
- **AND** `EndpointNames.Auth.Routes.RefreshToken` SHALL equal `"/api/auth/refresh-token"`

### Requirement: Token Service Registration

The system SHALL register `ITokenService` / `TokenService` in `DependencyInjection.AddAuthenticationServices()`.

#### Scenario: Service registration

- **WHEN** `AddAuthenticationServices()` is called
- **THEN** it SHALL register `ITokenService` / `TokenService` as scoped
