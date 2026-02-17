## Context

The test suite contains ~24 unit test files using in-memory EF Core, Moq, and Bogus. The testing-standards spec requires Testcontainers PostgreSQL for integration tests, but no integration tests exist yet. All tests currently run against `InMemoryDatabase`, which does not validate PostgreSQL-specific behavior (snake_case naming, enum-as-string, migrations, concurrency tokens).

The API uses ASP.NET Identity + JWT Bearer auth with permission-based authorization. External services (ISemanticKernelService, IFootballApiClient) must be mocked. The goal is to test Auth and Chat API flows end-to-end through real HTTP endpoints against a real PostgreSQL database.

## Goals / Non-Goals

**Goals:**

- Stand up a shared Testcontainers PostgreSQL fixture that applies EF Core migrations
- Create a `CustomWebApplicationFactory` that wires up the real API pipeline with the test database
- Provide a helper to register users and obtain JWT tokens for authenticated requests
- Write integration tests for Auth endpoints: register, login, refresh token
- Write integration tests for Chat endpoints: create conversation, send message, get conversations, get conversation messages
- Mock external services (ISemanticKernelService, IFootballApiClient) at the DI level

**Non-Goals:**

- Testing UserManagement or FootballData endpoints (future work)
- E2E browser testing (covered by the e2e-testing spec with Playwright)
- Load/performance testing
- Splitting into a separate test project (keep in existing `GolMetrics.API.Tests`)

## Decisions

### 1. Shared PostgreSQL container via `ICollectionFixture`

Use xUnit's `IAsyncLifetime` + `CollectionFixture` pattern to spin up a single `postgres:16-alpine` container shared across all integration tests. This avoids per-test container overhead while maintaining test isolation through database transactions or per-test cleanup.

**Alternative considered:** Per-class container via `IClassFixture` — rejected because spinning up a container per test class adds significant time. A single shared container with `Respawn` or transaction rollback is standard practice.

### 2. WebApplicationFactory with service replacement

Use `WebApplicationFactory<Program>` to boot the real API pipeline. Override the DI container to:
- Replace the `GolMetricsDbContext` connection string with the Testcontainers PostgreSQL instance
- Replace `ISemanticKernelService` with a mock that returns deterministic responses
- Replace `IFootballApiClient` with a mock
- Keep all other services real (MediatR, FluentValidation, exception handlers, Identity, JWT)

**Alternative considered:** Building a manual test host — rejected because `WebApplicationFactory` provides the full middleware pipeline including auth, routing, and exception handling.

### 3. Test isolation via Respawn

Use the `Respawn` library to reset the database between tests. This is faster than re-applying migrations per test and provides clean state without container restarts.

**Alternative considered:** Transaction rollback per test — works for simple cases but can interfere with tests that verify commit behavior or multi-SaveChanges flows.

### 4. Auth helper for JWT token acquisition

Create a helper method that registers a test user via `POST /api/auth/register` and returns the JWT token. Tests that need authentication call this helper and set the `Authorization` header. This tests the real auth pipeline rather than bypassing it.

### 5. Keep tests in existing project

Add integration tests to `tests/GolMetrics.API.Tests/` under an `Integration/` folder with `[Trait("Category", "Integration")]`. The testing-standards spec mentions separate project naming (`GolMetrics.API.Tests.Integration`) but scoping this change to the existing project avoids solution restructuring. A future change can split the projects if needed.

## Risks / Trade-offs

- **[Docker required for CI]** Integration tests need Docker to run Testcontainers. CI runners must have Docker available. → Mitigation: Filter with `--filter Category=Integration` to skip in environments without Docker.
- **[Shared container state leakage]** Tests sharing a container could leak state if Respawn is not configured correctly. → Mitigation: Respawn runs before each test; verify checkpoint configuration includes all tables except `__EFMigrationsHistory`.
- **[Test execution time]** Container startup adds ~5-10s to the first test run. → Mitigation: Single shared container, parallelization within the collection via `Respawn`.
- **[Mock drift]** Mocked `ISemanticKernelService` responses may diverge from real behavior over time. → Mitigation: This is acceptable for integration tests; E2E tests with stubs cover the full path.
