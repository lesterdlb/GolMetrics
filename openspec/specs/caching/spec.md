# Caching

## Purpose

Provides a query-level cache for API-Football responses using SHA-256 hashing for cache keys and TTL-based expiration.

## Requirements

### Requirement: Cache Key Generation

The system SHALL generate deterministic cache keys using SHA-256 hashing.

#### Scenario: Key generation

- **WHEN** a football API request is made with an endpoint and parameters
- **THEN** the system SHALL normalize parameters by sorting them alphabetically
- **AND** it SHALL compute a SHA-256 hash of the concatenated endpoint and sorted parameters
- **AND** this hash SHALL be used as the `QueryHash` for cache lookup

#### Scenario: Parameter order independence

- **WHEN** two requests have the same endpoint and parameters but in different order
- **THEN** the system SHALL generate the same cache key for both requests

### Requirement: Cache Lookup

The system SHALL check the cache before making external API calls.

#### Scenario: Cache hit

- **WHEN** a cache entry exists with a matching `QueryHash`
- **AND** `ExpiresAt` is greater than the current UTC time
- **THEN** the system SHALL return the stored `ResponseData` without calling API-Football

#### Scenario: Cache miss

- **WHEN** no cache entry exists with a matching `QueryHash`
- **THEN** the system SHALL call API-Football, store the response as a new `CachedQuery`, and return the response

#### Scenario: Cache expired

- **WHEN** a cache entry exists with a matching `QueryHash`
- **AND** `ExpiresAt` is less than or equal to the current UTC time
- **THEN** the system SHALL treat this as a cache miss
- **AND** it SHALL update the existing entry with the new response and expiration

### Requirement: TTL Strategy

The system SHALL apply different TTL values based on data freshness requirements.

#### Scenario: Historical data

- **WHEN** the queried data is from a completed season or historical records
- **THEN** the cache TTL SHALL be 30 days

#### Scenario: Current season data

- **WHEN** the queried data is from the current active season (standings, statistics)
- **THEN** the cache TTL SHALL be 1 hour

#### Scenario: Live or upcoming data

- **WHEN** the queried data is for upcoming or live matches
- **THEN** the cache TTL SHALL be 5 minutes

### Requirement: Cache Service Interface

The system SHALL provide `ICacheService` as the abstraction for cache operations.

#### Scenario: Service methods

- **WHEN** cache operations are needed
- **THEN** `ICacheService` SHALL expose `GetOrSetAsync<T>(endpoint, params, fetchFactory, ttl)` method
- **AND** it SHALL encapsulate key generation, lookup, storage, and TTL management

### Requirement: Error Handling

The system SHALL handle cache failures without blocking API responses.

#### Scenario: Cache write failure

- **WHEN** storing a cache entry fails (DB write error)
- **THEN** the system SHALL return the fetched API response without caching (fail-open)

#### Scenario: Concurrent cache miss

- **WHEN** two concurrent requests miss the cache for the same key
- **THEN** the system SHALL allow both to fetch from API-Football
- **AND** the last write SHALL overwrite the first (no distributed locking required)
