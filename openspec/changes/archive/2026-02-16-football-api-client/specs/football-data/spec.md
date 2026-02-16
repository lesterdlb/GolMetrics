## MODIFIED Requirements

### Requirement: Football API Client

The system SHALL provide `IFootballApiClient` as a typed HttpClient for API-Football v3.

#### Scenario: API key resolution

- **WHEN** a request to API-Football is made
- **THEN** `ApiKeyDelegatingHandler` SHALL resolve the API key by looking up the current user via `ICurrentUserService.UserId` and `UserManager<User>`
- **AND** if the user has a non-null `EncryptedApiKey`, it SHALL decrypt it via `IEncryptionService.Decrypt()`
- **AND** if the user has no stored key or is unauthenticated, it SHALL fall back to the system default key from `IConfiguration["ApiFootball:ApiKey"]`
- **AND** it SHALL set the `x-apisports-key` header on the outgoing request

#### Scenario: Request execution with caching

- **WHEN** a football data request is made (GetTopScorers, GetStandings, GetRecentResults, GetUpcomingMatches, GetTeamStatistics)
- **THEN** `FootballApiClient` SHALL use `ICacheService.GetOrSetAsync()` to check for a cached response before making the HTTP call
- **AND** the cache key SHALL be derived from the endpoint path and query parameters
- **AND** the cache TTL SHALL be 1 hour
- **AND** it SHALL set the base URL to `https://v3.football.api-sports.io`

#### Scenario: Result return type

- **WHEN** a football data method completes
- **THEN** it SHALL return `Result<string>` instead of `string`
- **AND** on success, the `Result<string>` SHALL contain the response body string
- **AND** on failure, the `Result<string>` SHALL contain the appropriate `FootballErrors` error

#### Scenario: Rate limit tracking

- **WHEN** a response is received from API-Football
- **THEN** `FootballApiClient` SHALL read the `x-ratelimit-requests-remaining` header
- **AND** if the value is `0`, it SHALL return `Result.Failure(FootballErrors.RateLimitExceeded)`

#### Scenario: HTTP 429 handling

- **WHEN** API-Football returns HTTP 429 (Too Many Requests)
- **THEN** `FootballApiClient` SHALL return `Result.Failure(FootballErrors.RateLimitExceeded)`

#### Scenario: HTTP 5xx handling

- **WHEN** API-Football returns an HTTP 5xx status code
- **THEN** `FootballApiClient` SHALL return `Result.Failure(FootballErrors.ApiUnavailable)`

#### Scenario: Empty or error response handling

- **WHEN** API-Football returns a successful HTTP status but the response body contains an `errors` object that is non-empty
- **THEN** `FootballApiClient` SHALL return `Result.Failure(FootballErrors.InvalidParameters)`

#### Scenario: ValidateApiKeyAsync unchanged

- **WHEN** `ValidateApiKeyAsync(string apiKey)` is called
- **THEN** it SHALL continue to accept an explicit API key parameter
- **AND** it SHALL set the `x-apisports-key` header directly on the request (bypassing the delegating handler resolution)
- **AND** it SHALL return `Task<bool>` (not `Result`)

### Requirement: API-Football Configuration

The system SHALL use API-Football v3 with the following configuration constraints.

#### Scenario: API authentication and rate limits

- **WHEN** communicating with API-Football
- **THEN** the base URL SHALL be `https://v3.football.api-sports.io`
- **AND** the authentication header SHALL be `x-apisports-key`
- **AND** the system SHALL monitor rate limit headers: `x-ratelimit-requests-limit` and `x-ratelimit-requests-remaining`
- **AND** the free tier limit is 100 requests/day

#### Scenario: Configuration settings

- **WHEN** the football API client is configured
- **THEN** `appsettings.json` SHALL include an `ApiFootball` section with `BaseUrl` (string, default `https://v3.football.api-sports.io`) and `ApiKey` (string, system default key)

#### Scenario: Response envelope structure

- **WHEN** a response is received from API-Football
- **THEN** it SHALL follow the envelope structure: `{ "get", "parameters", "errors", "results", "paging", "response" }`
- **AND** the actual data SHALL be in the `response` array

#### Scenario: League and season conventions

- **WHEN** league or season parameters are used
- **THEN** the season format SHALL be a 4-digit year (e.g., `2024`)
- **AND** common league IDs SHALL be: Premier League=39, La Liga=140, Serie A=135, Bundesliga=78, Ligue 1=61

## ADDED Requirements

### Requirement: API Key Delegating Handler

The system SHALL provide `ApiKeyDelegatingHandler` as an `HttpMessageHandler` in the `FootballApiClient` HTTP pipeline.

#### Scenario: Handler registration

- **WHEN** the football services are registered in `DependencyInjection.cs`
- **THEN** `ApiKeyDelegatingHandler` SHALL be registered as a transient service
- **AND** it SHALL be added to the `IFootballApiClient` HttpClient pipeline via `AddHttpMessageHandler<ApiKeyDelegatingHandler>()`

#### Scenario: Scoped service resolution

- **WHEN** the handler processes a request
- **THEN** it SHALL create a new `IServiceScope` via `IServiceScopeFactory` to resolve `ICurrentUserService`, `UserManager<User>`, and `IEncryptionService`
- **AND** it SHALL dispose the scope after resolving the API key

### Requirement: Feature Errors

The system SHALL define football data errors in `FootballErrors.cs`.

#### Scenario: Error definitions

- **WHEN** football data errors are needed
- **THEN** `FootballErrors` SHALL define static properties:
- **AND** `RateLimitExceeded` SHALL be `Error("Football.RateLimitExceeded", "API-Football rate limit exceeded. Please try again later.", ErrorCategory.BadRequest)`
- **AND** `ApiUnavailable` SHALL be `Error("Football.ApiUnavailable", "API-Football service is currently unavailable.", ErrorCategory.BadRequest)`
- **AND** `InvalidParameters` SHALL be `Error("Football.InvalidParameters", "The provided parameters returned an error from API-Football.", ErrorCategory.BadRequest)`
