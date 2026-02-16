## Why

The CachedQuery entity and EF Core configuration already exist, but there is no service layer to use them. API-Football requests currently hit the external API on every call, wasting rate-limited requests and increasing latency. An ICacheService is needed to encapsulate cache key generation, lookup, storage, and TTL management so that the FootballData feature can transparently cache API responses.

## What Changes

- Introduce `ICacheService` interface with a `GetOrSetAsync<T>` method that encapsulates SHA-256 key generation, cache lookup, storage, and TTL management
- Implement `CacheService` backed by the existing `CachedQuery` entity and EF Core
- Apply TTL strategy: 30 days (historical), 1 hour (current season), 5 minutes (live/upcoming)
- Fail-open on cache write errors (return fetched data without caching)
- Last-write-wins on concurrent cache misses (no distributed locking)

## Capabilities

### New Capabilities

- `caching`: Cache service abstraction and implementation for API-Football response caching with TTL-based expiration

### Modified Capabilities


## Impact

- `src/GolMetrics.API/Features/FootballData/` - new ICacheService interface and CacheService implementation
- `src/GolMetrics.API/DependencyInjection.cs` - register ICacheService/CacheService
- `tests/GolMetrics.API.Tests/` - unit tests for CacheService (key generation, hit/miss/expiry, fail-open behavior)
- No breaking changes to existing APIs or database schema (CachedQuery entity and migration already exist)
