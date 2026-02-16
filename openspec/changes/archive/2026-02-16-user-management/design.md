## Context

The User entity (`IdentityUser<Guid>` with `EncryptedApiKey`), `IEncryptionService`/`EncryptionService` (AES-256), `IFootballApiClient.ValidateApiKeyAsync()`, and `ICurrentUserService` are already implemented. Permission constants (`user:read`, `user:write`) and the authorization infrastructure (`RequirePermissions()`, `PermissionAuthorizationHandler`) are in place.

What's missing: the two vertical slices (GetProfile, UpdateApiKey) that expose these capabilities as HTTP endpoints, plus `UserErrors.cs` for domain errors and `EndpointNames.User` for route constants.

## Goals / Non-Goals

**Goals:**

- Implement GetProfile slice returning authenticated user's profile with `hasApiKey` indicator
- Implement UpdateApiKey slice that validates BYOK key against API-Football, encrypts, and stores
- Follow existing vertical slice patterns (Login, Register) exactly
- Apply permission-based authorization on both endpoints

**Non-Goals:**

- Delete/revoke API key endpoint (not in spec)
- Admin user management or listing other users
- Changing the EncryptionService or User entity (already implemented)
- Frontend integration (separate change)

## Decisions

### GetProfile slice structure

**Decision**: Single query returning `{id, email, hasApiKey, createdAt}` via `UserManager<User>.FindByIdAsync()`.

**Rationale**: `ICurrentUserService` provides the authenticated user's ID from JWT claims. `UserManager` is already injected in Auth slices. `hasApiKey` is derived from `!string.IsNullOrEmpty(user.EncryptedApiKey)` -- no decryption needed.

### UpdateApiKey validation flow

**Decision**: Validate key against API-Football `/status` before encrypting and storing. Use `IFootballApiClient.ValidateApiKeyAsync()` which already exists.

**Alternatives considered**:
- Store first, validate lazily on first use -- rejected because spec requires upfront validation and immediate feedback
- Call API-Football directly in the handler -- rejected because `IFootballApiClient` already encapsulates this

**Error handling**: `ValidateApiKeyAsync` returns `false` for invalid keys (4xx) and throws `HttpRequestException`/`TaskCanceledException` for network failures. The handler catches exceptions and returns `UserErrors.ApiValidationUnavailable`.

### UserErrors definition

**Decision**: Static properties following `AuthErrors` pattern: `InvalidApiKey` (BadRequest), `UserNotFound` (NotFound), `ApiValidationUnavailable` (BadRequest).

**Note**: The spec defines `ApiValidationUnavailable` as BadRequest. While HTTP 502 might be more semantically correct for upstream failures, the spec explicitly states BadRequest, so we follow it. The error code `User.ApiValidationUnavailable` in the message provides enough context for clients.

### Route structure

**Decision**: `GET /api/user/profile` and `PUT /api/user/api-key` matching the spec. Added to `EndpointNames.User` following the existing nested class pattern.

## Risks / Trade-offs

- **[API-Football rate limiting]** Validation calls to `/status` on every key update could hit rate limits if abused. Mitigation: endpoint is authenticated and per-user, so abuse surface is limited.
- **[No key deletion]** Users cannot remove a stored key to fall back to system default. Mitigation: out of scope for this change; can be added later if needed.
- **[Timeout on validation]** `HttpClient` default timeout applies. If API-Football is slow, the request blocks. Mitigation: The `HttpClient` is configured with a reasonable timeout in DI. The handler catches exceptions and returns `ApiValidationUnavailable`.
