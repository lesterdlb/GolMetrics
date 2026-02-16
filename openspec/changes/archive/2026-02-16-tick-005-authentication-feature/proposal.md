## Why

The API has authorization infrastructure (permissions, handler, User entity) but no authentication endpoints. Users cannot register, log in, or obtain JWT tokens needed to access protected endpoints.

## What Changes

- Add `POST /api/auth/register` endpoint (Register slice)
- Add `POST /api/auth/login` endpoint (Login slice)
- Add `POST /api/auth/refresh-token` endpoint (RefreshToken slice)
- Add `ITokenService` / `TokenService` for JWT generation with permission claims
- Add `AuthErrors` for authentication-specific error definitions
- Wire authentication services in `DependencyInjection.cs`

## Capabilities

### New Capabilities

- `authentication`: Register, Login, RefreshToken slices, JWT token generation via ITokenService, and AuthErrors definitions

### Modified Capabilities

_None_ - The existing `user-identity` spec (User entity, Permissions, PermissionAuthorizationHandler, CurrentUserService) is already implemented and does not require requirement changes.

## Impact

- New feature directory: `Features/Auth/`
- New core service: `Core/Identity/TokenService.cs` + `ITokenService` abstraction
- Modified: `DependencyInjection.cs` (register TokenService)
- Modified: `EndpointNames.cs` (add Auth route constants)
- Dependencies: existing ASP.NET Identity, JWT Bearer config, User entity
