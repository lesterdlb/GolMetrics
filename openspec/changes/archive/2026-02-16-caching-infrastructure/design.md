## Context

The CachedQuery entity and its EF Core configuration already exist in `Features/FootballData/`. The entity stores cached API-Football responses with SHA-256 hashed keys, JSONB columns for params/response data, and a TTL-based expiration field. The database migration is in place. What's missing is the service layer (`ICacheService` / `CacheService`) that encapsulates cache key generation, lookup, storage, and TTL management.

## Goals / Non-Goals

**Goals:**
- Provide `ICacheService` with a single `GetOrSetAsync<T>` method for transparent caching
- Implement SHA-256 key generation with deterministic parameter normalization
- Support configurable TTL per call site (30d historical, 1h current season, 5min live)
- Fail-open on cache write errors
- Last-write-wins on concurrent misses

**Non-Goals:**
- Distributed caching (Redis, Memcached) - PostgreSQL-backed only for now
- Cache invalidation API or manual purge endpoints
- Background cache warming or preloading
- Rate limiting integration (separate concern)

## Decisions

### 1. Single generic method over granular cache operations

**Decision**: `ICacheService` exposes only `GetOrSetAsync<T>(string endpoint, Dictionary<string, string> parameters, Func<Task<T>> fetchFactory, TimeSpan ttl)` rather than separate Get/Set/Delete methods.

**Rationale**: The caching spec defines a single cache-aside pattern. Exposing lower-level operations invites inconsistent usage and bypasses the key generation/TTL logic. Callers provide a fetch factory; the service handles everything else.

**Alternative considered**: Separate `GetAsync<T>` / `SetAsync<T>` methods. Rejected because it pushes cache logic to callers and risks inconsistent key generation.

### 2. ICacheService in FootballData feature folder

**Decision**: Place `ICacheService` and `CacheService` in `Features/FootballData/` since caching is currently scoped to API-Football responses only.

**Rationale**: Follows the vertical slice architecture. If caching is later needed elsewhere, the interface can be extracted to `Core/`.

**Alternative considered**: Place in `Core/Abstractions/`. Premature - only one consumer exists.

### 3. SHA-256 key generation as private implementation detail

**Decision**: Key generation logic lives inside `CacheService` as a private method, not exposed on the interface.

**Rationale**: Callers pass endpoint + params; the hashing strategy is an implementation detail. This allows changing the algorithm without affecting consumers.

### 4. TimeProvider for testability

**Decision**: Inject `TimeProvider` (built-in .NET 8+) instead of using `DateTime.UtcNow` directly.

**Rationale**: Enables deterministic testing of TTL expiration without real clock dependencies.

### 5. JSON deserialization inside the service

**Decision**: `CacheService` deserializes `ResponseData` (stored as JSON string) back to `T` on cache hits and serializes `T` to JSON on cache misses before storing.

**Rationale**: The CachedQuery entity stores `ResponseData` as a string (JSONB column). The service needs to handle serialization boundaries so callers work with typed objects.

## Risks / Trade-offs

- **[Unbounded cache growth]** No eviction beyond TTL expiration. Mitigated by: expired entries are overwritten on next miss; a cleanup job can be added later if needed.
- **[Concurrent duplicate fetches]** Two concurrent misses for the same key both call API-Football. Mitigated by: acceptable trade-off vs. distributed locking complexity; last write wins.
- **[Single database round-trip per lookup]** Cache check requires a DB query. Mitigated by: unique index on `QueryHash` ensures fast lookups; acceptable latency for reducing external API calls.
