## Why

TICK-006: Authenticated users currently have no way to view their profile or manage their API-Football BYOK key through the API. The User entity and EncryptionService infrastructure exist, but no endpoints expose this functionality. Users need self-service profile access and API key management to use the BYOK feature defined in the user-management spec.

## What Changes

- Add `GET /api/user/profile` endpoint (GetProfile slice) returning user profile with `hasApiKey` indicator
- Add `PUT /api/user/api-key` endpoint (UpdateApiKey slice) that validates the key against API-Football `/status`, encrypts with AES-256, and stores it
- Add `UserErrors.cs` with domain-specific error definitions (InvalidApiKey, UserNotFound, ApiValidationUnavailable)
- Add `User` section to `EndpointNames.cs` with route constants
- Wire up permission-based authorization (`user:read`, `user:write`) on both endpoints

## Capabilities

### New Capabilities

_None_ -- the `user-management` capability spec already exists and covers these requirements.

### Modified Capabilities

_None_ -- the existing `user-management` spec already defines GetProfile, UpdateApiKey, UserErrors, and EncryptionService requirements. This change implements them without modifying any spec-level behavior.

## Impact

- **New files**: `GetProfile.cs`, `UpdateApiKey.cs`, `UserErrors.cs` in `src/GolMetrics.API/Features/UserManagement/`
- **Modified files**: `EndpointNames.cs` (add User section)
- **Dependencies**: Uses existing `IEncryptionService`, `IFootballApiClient.ValidateApiKeyAsync()`, `ICurrentUserService`, `UserManager<User>`
- **No new NuGet packages or migrations required** -- User entity and EncryptedApiKey column already exist
