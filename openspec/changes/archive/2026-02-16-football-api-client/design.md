## Context

The current `FootballApiClient` is a minimal typed HttpClient that calls API-Football endpoints and returns raw `Task<string>`. It lacks:
- Per-user API key resolution (users store encrypted keys via `UpdateApiKey` slice)
- Caching integration (`ICacheService` exists but is unused by the client)
- Rate limit header tracking (`x-ratelimit-requests-limit`, `x-ratelimit-requests-remaining`)
- Error handling (HTTP errors propagate as unhandled `HttpRequestException`)

The `User` entity already has `EncryptedApiKey`, `IEncryptionService` handles AES-256 decryption, `ICurrentUserService` provides the current user's identity, and `ICacheService` provides database-backed caching with TTL.

## Goals / Non-Goals

**Goals:**
- Return `Result<string>` from all `IFootballApiClient` data methods so callers handle errors via the Result pattern
- Resolve per-user API keys with fallback to system default key
- Integrate caching for all read endpoints via `ICacheService`
- Track rate limit headers and return `FootballErrors.RateLimitExceeded` when remaining = 0
- Map HTTP 429 and 5xx responses to appropriate `FootballErrors`

**Non-Goals:**
- Retry/backoff logic (defer to a future change)
- Circuit breaker pattern
- Response deserialization into typed models (callers continue to receive raw JSON strings)
- Changing the `ValidateApiKeyAsync` method signature (it accepts an explicit key, not per-user resolution)

## Decisions

### 1. DelegatingHandler for API key injection

**Decision**: Use an `ApiKeyDelegatingHandler` in the HttpClient pipeline to inject the `x-apisports-key` header on every request.

**Rationale**: Separates auth concerns from business logic. The handler resolves the user's decrypted key via `ICurrentUserService` + `IEncryptionService` + `UserManager<User>`, falling back to `IConfiguration["ApiFootball:ApiKey"]`. This keeps `FootballApiClient` focused on endpoint logic.

**Alternative considered**: Injecting the key inside each method call. Rejected because it duplicates resolution logic across 5+ methods.

**Note**: The handler must be registered as a transient service and added to the HttpClient pipeline via `AddHttpMessageHandler<ApiKeyDelegatingHandler>()`. Since it depends on scoped services (`ICurrentUserService`, `UserManager<User>`), it will use `IServiceScopeFactory` to create a scope per request.

### 2. Result<string> return type for data methods

**Decision**: Change `IFootballApiClient` data methods from `Task<string>` to `Task<Result<string>>`. Keep `ValidateApiKeyAsync` as `Task<bool>`.

**Rationale**: Aligns with the project's Result pattern. Callers can pattern-match on success/failure without catching exceptions.

**Alternative considered**: Throwing custom exceptions. Rejected because the project convention is `Result.Failure()` for business errors.

### 3. Caching inside FootballApiClient methods

**Decision**: Wrap each API call with `ICacheService.GetOrSetAsync()` directly inside `FootballApiClient` methods, using the endpoint path and parameters as the cache key.

**Rationale**: `ICacheService` already handles key hashing and TTL. Placing caching at the client level ensures all callers benefit transparently. TTL of 1 hour for all endpoints (football data doesn't change frequently within a match day).

**Alternative considered**: Caching at the plugin/handler level. Rejected because it would require caching awareness in multiple callers.

### 4. Rate limit tracking via response headers

**Decision**: After each HTTP response, read `x-ratelimit-requests-remaining`. If the value is `0` or the response is HTTP 429, return `FootballErrors.RateLimitExceeded`.

**Rationale**: The free tier allows 100 requests/day. Proactively checking the remaining count prevents wasting a request that will fail.

**Implementation**: Done inside `FootballApiClient` after `HttpClient.SendAsync()`, before processing the response body. This keeps it co-located with the HTTP call logic.

### 5. FootballErrors static class

**Decision**: Create `FootballErrors.cs` in `Features/FootballData/` with static `Error` properties: `RateLimitExceeded`, `ApiUnavailable`, `InvalidParameters`.

**Rationale**: Follows the project pattern (`UserErrors`, `AuthErrors`).

## Risks / Trade-offs

- **[Scoped service in DelegatingHandler]** DelegatingHandlers registered with `IHttpClientFactory` are pooled and long-lived, but `ICurrentUserService` and `UserManager<User>` are scoped. Using `IServiceScopeFactory` inside the handler to resolve per-request avoids captive dependency issues, but adds a small allocation per request. This is acceptable for external API call overhead.

- **[Cache key collisions]** `ICacheService` hashes endpoint + parameters with SHA-256, which has negligible collision probability. No mitigation needed.

- **[Stale cache during live matches]** 1-hour TTL means live match data could be up to 1 hour old. This is acceptable for the current use case (statistics chatbot, not live scores). A future change could add endpoint-specific TTLs.

- **[ValidateApiKeyAsync bypass]** `ValidateApiKeyAsync` takes an explicit key parameter and sends it directly (used during key setup). It bypasses the delegating handler's key resolution. This is intentional -- validation happens before the key is stored.
