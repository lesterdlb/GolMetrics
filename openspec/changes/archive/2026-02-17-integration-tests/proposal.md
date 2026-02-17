## Why

TICK-018: The test suite currently uses in-memory EF Core for database-dependent tests, which does not catch PostgreSQL-specific behavior (snake_case naming, enum-as-string mapping, migration integrity, concurrency tokens). The testing-standards spec requires Testcontainers with `postgres:16-alpine` for integration tests, but this has not been implemented. Adding real database integration tests for Auth and Chat flows closes this gap and brings the project closer to the 25% integration test coverage target.

## What Changes

- Add `Testcontainers.PostgreSql` NuGet package to the test project
- Create a shared PostgreSQL container fixture using `IAsyncLifetime` for test lifecycle management
- Create a `CustomWebApplicationFactory` that replaces the real database with the Testcontainers PostgreSQL instance
- Implement Auth integration tests: register, login, refresh token, and token revocation flows against real HTTP endpoints
- Implement Chat integration tests: create conversation, send message, get conversations, get messages flows with authenticated requests
- Tag all new tests with `[Trait("Category", "Integration")]` for filtered execution

## Capabilities

### New Capabilities

- `integration-testing`: Testcontainers PostgreSQL infrastructure, WebApplicationFactory setup, and integration tests for Auth and Chat API flows

### Modified Capabilities

_None_ - the testing-standards spec already defines integration testing requirements; this change implements them without modifying the spec.

## Impact

- **Test project**: `tests/GolMetrics.API.Tests/` gains new integration test files and shared fixtures
- **Dependencies**: `Testcontainers.PostgreSql` and `Microsoft.AspNetCore.Mvc.Testing` NuGet packages added
- **CI**: Integration tests require Docker; filtered execution via `--filter Category=Integration`
- **No production code changes**: This is a test-only change
