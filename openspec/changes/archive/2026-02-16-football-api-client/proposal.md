## Why

TICK-009: The current `FootballApiClient` is a thin passthrough to `HttpClient.GetStringAsync()` with no error handling, rate limit management, per-user API key resolution, or caching integration. The football-data spec requires all of these capabilities. Without them, API failures propagate as unhandled exceptions, rate limits are invisible to users, and every request hits the external API even for duplicate queries.

## What Changes

- Rewrite `FootballApiClient` to resolve per-user API keys via `ICurrentUserService` + `IEncryptionService`, falling back to the system default key from configuration
- Add `x-apisports-key` header injection via a delegating handler
- Integrate `ICacheService` for transparent response caching on all read endpoints
- Add rate limit tracking by reading `x-ratelimit-requests-limit` and `x-ratelimit-requests-remaining` response headers
- Add error handling: return `Result<string>` instead of raw strings, mapping HTTP 429 to `FootballErrors.RateLimitExceeded`, 5xx to `FootballErrors.ApiUnavailable`, and empty/error responses to `FootballErrors.InvalidParameters`
- Define `FootballErrors` with static error properties
- Update `FootballPlugin` to handle `Result<string>` returns from the client

## Capabilities

### New Capabilities

(none)

### Modified Capabilities

- `football-data`: Adding per-user API key resolution, caching integration, rate limit header tracking, and Result-based error handling to `IFootballApiClient` and its implementation

## Impact

- **Files modified**: `IFootballApiClient.cs`, `FootballApiClient.cs`, `DependencyInjection.cs` (HttpClient pipeline configuration)
- **Files created**: `FootballErrors.cs`, `ApiKeyDelegatingHandler.cs` (delegating handler for header injection)
- **Dependencies**: No new NuGet packages required (uses existing `ICacheService`, `IEncryptionService`, `ICurrentUserService`)
- **Breaking change to internal interface**: `IFootballApiClient` methods will return `Result<string>` instead of `Task<string>`, requiring updates to `FootballPlugin` callers
