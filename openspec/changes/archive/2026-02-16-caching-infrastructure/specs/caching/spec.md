## ADDED Requirements

### Requirement: ICacheService interface
The system SHALL define `ICacheService` in the `Features/FootballData/` namespace with a single method: `Task<T> GetOrSetAsync<T>(string endpoint, Dictionary<string, string> parameters, Func<Task<T>> fetchFactory, TimeSpan ttl)`.

#### Scenario: Interface definition
- **WHEN** a feature needs to cache API-Football responses
- **THEN** it SHALL depend on `ICacheService` via constructor injection
- **AND** `ICacheService` SHALL expose only `GetOrSetAsync<T>` as its public contract

### Requirement: CacheService implementation
The system SHALL implement `CacheService : ICacheService` as an `internal sealed class` in `Features/FootballData/` that uses `GolMetricsDbContext` and `TimeProvider` for all cache operations.

#### Scenario: Service registration
- **WHEN** the application starts
- **THEN** `CacheService` SHALL be registered as a scoped service for `ICacheService` in `DependencyInjection.cs`

#### Scenario: Cache hit returns stored data
- **WHEN** `GetOrSetAsync<T>` is called with an endpoint and parameters
- **AND** a `CachedQuery` exists with a matching `QueryHash` and `ExpiresAt > TimeProvider.GetUtcNow()`
- **THEN** the service SHALL deserialize `ResponseData` to `T` and return it
- **AND** it SHALL NOT invoke the `fetchFactory`

#### Scenario: Cache miss fetches and stores
- **WHEN** `GetOrSetAsync<T>` is called with an endpoint and parameters
- **AND** no matching `CachedQuery` exists
- **THEN** the service SHALL invoke `fetchFactory` to get the data
- **AND** it SHALL create a new `CachedQuery` with the SHA-256 hashed key, serialized response, and `ExpiresAt` set to `TimeProvider.GetUtcNow() + ttl`
- **AND** it SHALL return the fetched data

#### Scenario: Expired cache entry is refreshed
- **WHEN** `GetOrSetAsync<T>` is called with an endpoint and parameters
- **AND** a matching `CachedQuery` exists but `ExpiresAt <= TimeProvider.GetUtcNow()`
- **THEN** the service SHALL invoke `fetchFactory` to get fresh data
- **AND** it SHALL update the existing `CachedQuery` with new `ResponseData` and `ExpiresAt`
- **AND** it SHALL return the fresh data

### Requirement: Deterministic cache key generation
The system SHALL generate cache keys by normalizing parameters alphabetically and computing a SHA-256 hash of the concatenated endpoint and sorted parameters.

#### Scenario: Same parameters in different order produce same key
- **WHEN** two calls have endpoint `"fixtures"` with parameters `{"season": "2024", "league": "39"}` and `{"league": "39", "season": "2024"}`
- **THEN** the generated `QueryHash` SHALL be identical for both calls

#### Scenario: Different endpoints produce different keys
- **WHEN** two calls have different endpoints but identical parameters
- **THEN** the generated `QueryHash` SHALL be different

### Requirement: Fail-open cache write behavior
The system SHALL return fetched data even if cache persistence fails.

#### Scenario: Database write failure during cache store
- **WHEN** `fetchFactory` succeeds but `SaveChangesAsync` throws an exception
- **THEN** the service SHALL catch the exception, log a warning, and return the fetched data
- **AND** the caller SHALL NOT receive an error

### Requirement: Concurrent cache miss tolerance
The system SHALL allow concurrent requests to independently fetch and store cached data without distributed locking.

#### Scenario: Two concurrent misses for the same key
- **WHEN** two concurrent calls miss the cache for the same key
- **THEN** both SHALL invoke their `fetchFactory` independently
- **AND** the last `SaveChangesAsync` to complete SHALL overwrite the first
