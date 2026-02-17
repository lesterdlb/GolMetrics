## 1. Dependencies and Project Setup

- [x] 1.1 Add NuGet packages to `tests/GolMetrics.API.Tests/GolMetrics.API.Tests.csproj`: `Testcontainers.PostgreSql`, `Microsoft.AspNetCore.Mvc.Testing`, `Respawn`
- [x] 1.2 Verify project builds: `dotnet build tests/GolMetrics.API.Tests/`

## 2. Testcontainers PostgreSQL Fixture

- [x] 2.1 Create `tests/GolMetrics.API.Tests/Integration/PostgreSqlFixture.cs`: implement `IAsyncLifetime` that starts a `postgres:16-alpine` container, applies EF Core migrations via `Database.MigrateAsync()`, initializes a Respawn checkpoint, and exposes the connection string and a `ResetDatabaseAsync()` method
- [x] 2.2 Create `tests/GolMetrics.API.Tests/Integration/IntegrationTestCollection.cs`: define an xUnit `[CollectionDefinition]` with `ICollectionFixture<PostgreSqlFixture>`

## 3. CustomWebApplicationFactory

- [x] 3.1 Create `tests/GolMetrics.API.Tests/Integration/CustomWebApplicationFactory.cs`: subclass `WebApplicationFactory<Program>` that accepts the Testcontainers connection string, replaces `GolMetricsDbContext` registration with the test PostgreSQL instance, replaces `ISemanticKernelService` with a mock returning a deterministic string, and replaces `IFootballApiClient` with a mock
- [x] 3.2 Make `Program` class accessible to the test project by adding `[assembly: InternalsVisibleTo("GolMetrics.API.Tests")]` to the API project or a `partial class Program` marker

## 4. Integration Test Base Class

- [x] 4.1 Create `tests/GolMetrics.API.Tests/Integration/IntegrationTestBase.cs`: base class annotated with `[Collection]` and `[Trait("Category", "Integration")]` that receives `PostgreSqlFixture`, creates `CustomWebApplicationFactory` and `HttpClient`, provides `RegisterAndAuthenticateAsync()` helper returning an authenticated `HttpClient`, and calls `ResetDatabaseAsync()` in setup via `IAsyncLifetime.InitializeAsync`

## 5. Auth Integration Tests

- [x] 5.1 Create `tests/GolMetrics.API.Tests/Integration/Auth/RegisterIntegrationTests.cs`: test successful registration (HTTP 200 with tokens), duplicate email (HTTP 409), and invalid input (HTTP 400)
- [x] 5.2 Create `tests/GolMetrics.API.Tests/Integration/Auth/LoginIntegrationTests.cs`: test successful login (HTTP 200 with tokens), invalid credentials (HTTP 401), and nonexistent user (HTTP 401)
- [x] 5.3 Create `tests/GolMetrics.API.Tests/Integration/Auth/RefreshTokenIntegrationTests.cs`: test successful refresh (HTTP 200 with new tokens) and invalid token (HTTP 401)
- [x] 5.4 Verify Auth tests pass: `dotnet test tests/GolMetrics.API.Tests/ --filter "Category=Integration&FullyQualifiedName~Auth"`

## 6. Chat Integration Tests

- [x] 6.1 Create `tests/GolMetrics.API.Tests/Integration/Chat/CreateConversationIntegrationTests.cs`: test successful creation (HTTP 200), unauthenticated (HTTP 401), and invalid title (HTTP 400)
- [x] 6.2 Create `tests/GolMetrics.API.Tests/Integration/Chat/SendMessageIntegrationTests.cs`: test send to existing conversation (HTTP 200 with persisted messages), auto-create conversation (HTTP 200), nonexistent conversation (HTTP 404), and other user's conversation (HTTP 404)
- [x] 6.3 Create `tests/GolMetrics.API.Tests/Integration/Chat/GetConversationsIntegrationTests.cs`: test returns user conversations ordered by recency (HTTP 200) and empty list (HTTP 200)
- [x] 6.4 Create `tests/GolMetrics.API.Tests/Integration/Chat/GetConversationMessagesIntegrationTests.cs`: test returns messages ordered by timestamp (HTTP 200) and conversation not found (HTTP 404)
- [x] 6.5 Verify Chat tests pass: `dotnet test tests/GolMetrics.API.Tests/ --filter "Category=Integration&FullyQualifiedName~Chat"`

## 7. Final Verification

- [x] 7.1 Run all integration tests: `dotnet test tests/GolMetrics.API.Tests/ --filter Category=Integration`
- [x] 7.2 Run all unit tests to verify no regressions: `dotnet test tests/GolMetrics.API.Tests/ --filter Category=Unit`
