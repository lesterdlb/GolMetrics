## 1. Core Infrastructure

- [x] 1.1 Create `ITokenService` interface in `Core/Abstractions/ITokenService.cs`
- [x] 1.2 Create `TokenService` implementation in `Core/Identity/TokenService.cs` (access token generation with HMAC-SHA256, permission claims, 7-day expiry)
- [x] 1.3 Register `ITokenService` / `TokenService` as scoped in `DependencyInjection.AddAuthenticationServices()`

## 2. Refresh Token Entity

- [x] 2.1 Create `RefreshToken` entity in `Features/Auth/RefreshTokenEntity.cs` extending `Entity` with `Token`, `UserId`, `ExpiresAtUtc`, `IsRevoked`, and `User` navigation
- [x] 2.2 Create `RefreshTokenConfiguration` in `Features/Auth/RefreshTokenConfiguration.cs` (unique index on Token, FK to User, table name `refresh_tokens`)
- [x] 2.3 Add `DbSet<RefreshTokenEntity>` to `GolMetricsDbContext`
- [x] 2.4 Generate EF Core migration for `refresh_tokens` table

## 3. Token Service - Refresh Token Support

- [x] 3.1 Add refresh token generation to `TokenService` (cryptographically random token, 30-day expiry, persisted to DB)
- [x] 3.2 Add refresh token validation to `TokenService` (lookup, check expiry, check revoked)

## 4. Auth Errors and Endpoint Names

- [x] 4.1 Create `AuthErrors` in `Features/Auth/AuthErrors.cs` with `DuplicateEmail`, `InvalidPassword`, `InvalidCredentials`, `InvalidRefreshToken`
- [x] 4.2 Add `RefreshToken` route constant and `Routes.RefreshToken` to `EndpointNames.Auth`

## 5. Auth Slices

- [x] 5.1 Create Register slice in `Features/Auth/Register.cs` (Command, Response, Validator, Handler implementing ISlice)
- [x] 5.2 Create Login slice in `Features/Auth/Login.cs` (Command, Response, Validator, Handler implementing ISlice)
- [x] 5.3 Create RefreshToken slice in `Features/Auth/RefreshToken.cs` (entity renamed to RefreshTokenEntity.cs to avoid conflict)

## 6. Testing

- [x] 6.1 Write unit tests for `TokenService` (access token claims, expiry, signing)
- [x] 6.2 Write unit tests for Register handler (success, duplicate email, invalid password)
- [x] 6.3 Write unit tests for Login handler (success, invalid credentials, user not found)
- [x] 6.4 Write unit tests for RefreshToken handler (success, invalid token, expired token, revoked token)
