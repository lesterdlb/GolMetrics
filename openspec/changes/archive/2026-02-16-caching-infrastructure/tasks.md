## 1. ICacheService Interface

- [x] 1.1 Create `ICacheService` interface in `Features/FootballData/` with `GetOrSetAsync<T>(string endpoint, Dictionary<string, string> parameters, Func<Task<T>> fetchFactory, TimeSpan ttl)` method

## 2. CacheService Implementation

- [x] 2.1 Create `CacheService` as `internal sealed class` in `Features/FootballData/` implementing `ICacheService`, injecting `GolMetricsDbContext`, `TimeProvider`, and `ILogger<CacheService>`
- [x] 2.2 Implement private SHA-256 key generation method that sorts parameters alphabetically, concatenates with endpoint, and returns hex hash string
- [x] 2.3 Implement `GetOrSetAsync<T>` cache hit path: query by `QueryHash` where `ExpiresAt > now`, deserialize `ResponseData` to `T`
- [x] 2.4 Implement `GetOrSetAsync<T>` cache miss path: invoke `fetchFactory`, create new `CachedQuery`, serialize response, set `ExpiresAt = now + ttl`
- [x] 2.5 Implement expired entry refresh: detect expired entry, invoke `fetchFactory`, update existing `CachedQuery` with new `ResponseData` and `ExpiresAt`
- [x] 2.6 Implement fail-open behavior: wrap `SaveChangesAsync` in try/catch, log warning on failure, return fetched data regardless

## 3. Service Registration

- [x] 3.1 Register `ICacheService` / `CacheService` as scoped in `DependencyInjection.cs`

## 4. Unit Tests

- [x] 4.1 Test cache hit returns stored data without calling fetch factory
- [x] 4.2 Test cache miss invokes fetch factory and stores result
- [x] 4.3 Test expired entry triggers refresh and updates stored data
- [x] 4.4 Test deterministic key generation with parameters in different order
- [x] 4.5 Test different endpoints produce different cache keys
- [x] 4.6 Test fail-open behavior when SaveChangesAsync throws
