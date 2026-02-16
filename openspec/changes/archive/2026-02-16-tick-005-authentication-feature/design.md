## Context

The API has ASP.NET Identity configured with JWT Bearer authentication (`DependencyInjection.AddAuthenticationServices()`), a `User` entity extending `IdentityUser<Guid>`, permission constants, `PermissionAuthorizationHandler`, `CurrentUserService`, and `EndpointNames.Auth` route constants. What's missing are the actual authentication endpoints (Register, Login, RefreshToken) and the `ITokenService`/`TokenService` that generates JWTs.

## Goals / Non-Goals

**Goals:**

- Implement Register, Login, and RefreshToken slices following vertical slice architecture
- Implement `ITokenService` / `TokenService` for JWT generation with permission claims
- Define `AuthErrors` for domain-specific error handling
- Add refresh token support with secure storage and rotation

**Non-Goals:**

- OAuth/social login providers (future work)
- Email verification or password reset flows
- Rate limiting on auth endpoints (separate concern)
- Two-factor authentication

## Decisions

### 1. Token Service location: `Core/Identity/TokenService.cs`

Place alongside `CurrentUserService` since both are identity infrastructure, not feature-specific. The abstraction `ITokenService` goes in `Core/Abstractions/`.

**Alternative**: Place in `Features/Auth/` - rejected because token generation is a cross-cutting identity concern used by multiple features.

### 2. Refresh tokens stored in database

Add a `RefreshToken` entity with `Token`, `UserId`, `ExpiresAtUtc`, `CreatedAtUtc`, and `IsRevoked` fields. Store in a dedicated `refresh_tokens` table. On refresh, revoke the old token and issue a new pair (access + refresh).

**Alternative**: Stateless refresh via long-lived JWTs - rejected because it prevents revocation.

### 3. Slice structure: one file per slice

Each slice (Register, Login, RefreshToken) is a single file in `Features/Auth/` containing the nested Command/Response/Validator/Handler classes. This follows the vertical slice pattern defined in the architecture spec.

### 4. All registered users get all permissions

Per the authentication spec, token generation includes all permission claims (`conversations:read`, `conversations:write`, `user:read`, `user:write`) for every user. No role-based differentiation in this phase.

### 5. Access token expiry: 7 days, Refresh token expiry: 30 days

Per the authentication spec, JWT access tokens have a 7-day expiry. Refresh tokens get a longer 30-day window.

## Risks / Trade-offs

- **[Risk] 7-day access token is long** -> Acceptable for MVP; refresh token rotation provides revocation capability. Can tighten later.
- **[Risk] No email verification** -> Users can register with any email. Acceptable for MVP; add verification in a future change.
- **[Risk] Refresh token table growth** -> Revoked tokens accumulate. Mitigated by adding a cleanup mechanism later or TTL-based deletion.
