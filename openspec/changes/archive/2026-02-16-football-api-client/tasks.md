## 1. Football Errors

- [x] 1.1 Create `src/GolMetrics.API/Features/FootballData/FootballErrors.cs` with static `Error` properties: `RateLimitExceeded`, `ApiUnavailable`, `InvalidParameters`
- [x] 1.2 Verify build: `dotnet build src/GolMetrics.API/`

## 2. API Key Delegating Handler

- [x] 2.1 Create `src/GolMetrics.API/Features/FootballData/ApiKeyDelegatingHandler.cs` as a `DelegatingHandler` that resolves per-user API key via `IServiceScopeFactory` -> `ICurrentUserService` + `UserManager<User>` + `IEncryptionService`, falling back to `IConfiguration["ApiFootball:ApiKey"]`, and sets `x-apisports-key` header
- [x] 2.2 Register `ApiKeyDelegatingHandler` as transient in `src/GolMetrics.API/DependencyInjection.cs` and add to the `IFootballApiClient` HttpClient pipeline via `AddHttpMessageHandler<ApiKeyDelegatingHandler>()`
- [x] 2.3 Verify build: `dotnet build src/GolMetrics.API/`

## 3. IFootballApiClient Interface Update

- [x] 3.1 Update `src/GolMetrics.API/Core/Abstractions/IFootballApiClient.cs`: change return types of `GetTopScorersAsync`, `GetStandingsAsync`, `GetRecentResultsAsync`, `GetUpcomingMatchesAsync`, `GetTeamStatisticsAsync` from `Task<string>` to `Task<Result<string>>`; keep `ValidateApiKeyAsync` as `Task<bool>`
- [x] 3.2 Verify build (expect compilation errors in `FootballApiClient` — will be fixed in next group)

## 4. FootballApiClient Implementation

- [x] 4.1 Rewrite `src/GolMetrics.API/Features/FootballData/FootballApiClient.cs`: inject `ICacheService` alongside `HttpClient`; implement each data method using `ICacheService.GetOrSetAsync()` wrapping `HttpClient.SendAsync()`; read `x-ratelimit-requests-remaining` header from response; return `Result.Failure(FootballErrors.RateLimitExceeded)` on 429 or remaining=0; return `Result.Failure(FootballErrors.ApiUnavailable)` on 5xx; check response body for non-empty `errors` object and return `Result.Failure(FootballErrors.InvalidParameters)`; keep `ValidateApiKeyAsync` as-is with explicit key header
- [x] 4.2 Verify build: `dotnet build src/GolMetrics.API/`

## 5. Update Callers

- [x] 5.1 Update `src/GolMetrics.API/Features/UserManagement/UpdateApiKey.cs` handler if it uses `IFootballApiClient` data methods (currently only uses `ValidateApiKeyAsync` which is unchanged — verify no changes needed)
- [x] 5.2 Verify build and run tests: `dotnet build src/GolMetrics.API/ && dotnet test tests/GolMetrics.API.Tests/`

## 6. Configuration

- [x] 6.1 Ensure `appsettings.json` has `ApiFootball:ApiKey` setting for the system default key (add placeholder if missing); verify `ApiFootball:BaseUrl` already exists
- [x] 6.2 Verify full build and test: `dotnet build src/GolMetrics.API/ && dotnet test tests/GolMetrics.API.Tests/`
