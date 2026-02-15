# Authentication

## Purpose

Handles user registration, login, JWT token generation, and current user resolution via ASP.NET Identity and JWT Bearer authentication.

## Requirements

### Requirement: User Registration

The system SHALL allow new users to register via `POST /api/auth/register`.

#### Scenario: Successful registration

- **WHEN** a valid registration request is received with `email` and `password`
- **THEN** the system SHALL create a new user via `UserManager<User>.CreateAsync()`
- **AND** it SHALL generate a JWT token via `ITokenService`
- **AND** it SHALL return HTTP 201 with the token and expiry

#### Scenario: Duplicate email

- **WHEN** a registration request is received with an email that already exists
- **THEN** the system SHALL return HTTP 409 Conflict
- **AND** the error SHALL use `AuthErrors.DuplicateEmail`

#### Scenario: Invalid password format

- **WHEN** a registration request is received with a password that does not meet ASP.NET Identity requirements
- **THEN** the system SHALL return HTTP 400 Bad Request
- **AND** the error SHALL use `AuthErrors.InvalidPassword`

### Requirement: User Login

The system SHALL allow existing users to log in via `POST /api/auth/login`.

#### Scenario: Successful login

- **WHEN** a valid login request is received with correct `email` and `password`
- **THEN** the system SHALL validate credentials via `SignInManager<User>.CheckPasswordSignInAsync()`
- **AND** it SHALL generate a JWT token via `ITokenService`
- **AND** it SHALL return HTTP 200 with the token and expiry

#### Scenario: Invalid credentials

- **WHEN** a login request is received with incorrect email or password
- **THEN** the system SHALL return HTTP 401 Unauthorized
- **AND** the error SHALL use `AuthErrors.InvalidCredentials`

### Requirement: JWT Token

The system SHALL generate JWT tokens with HMAC-SHA256 signing.

#### Scenario: Token claims

- **WHEN** a JWT token is generated
- **THEN** it SHALL include claims: `sub` (user ID), `email`, `roles`, and individual `permissions` claims
- **AND** it SHALL have a 7-day expiry

#### Scenario: Permission claims generation

- **WHEN** `ITokenService` generates a JWT
- **THEN** it SHALL include all user permissions as individual `permissions` claims: `conversations:read`, `conversations:write`, `user:read`, `user:write`
- **AND** all registered users SHALL receive all permissions (single role model)

#### Scenario: Token service

- **WHEN** token generation is needed
- **THEN** `ITokenService` / `TokenService` SHALL create and sign the token
- **AND** the signing key SHALL be read from configuration

### Requirement: Current User Service

The system SHALL provide `ICurrentUserService` / `CurrentUserService` for resolving the authenticated user.

#### Scenario: Authenticated request

- **WHEN** an authenticated HTTP request is processed
- **THEN** `ICurrentUserService` SHALL read claims from `HttpContext.User`
- **AND** it SHALL expose `UserId`, `Email`, and `Permissions`

#### Scenario: Anonymous request

- **WHEN** an unauthenticated HTTP request accesses `ICurrentUserService`
- **THEN** `UserId` SHALL return `null`

### Requirement: Feature Errors

The system SHALL define authentication errors in `AuthErrors.cs`.

#### Scenario: Error definitions

- **WHEN** authentication errors are needed
- **THEN** `AuthErrors` SHALL define static properties: `DuplicateEmail` (Conflict), `InvalidPassword` (BadRequest), `InvalidCredentials` (Unauthorized)
