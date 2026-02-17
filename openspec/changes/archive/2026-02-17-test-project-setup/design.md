## Context

The test project (`tests/GolMetrics.API.Tests/`) has 9 unit test files covering Auth, FootballData, and UserManagement handlers. Missing coverage includes: core abstractions (Result, Entity, ValidationBehavior), all FluentValidation validators, and all Chat feature handlers. The `testing-standards` spec requires >70% coverage with Bogus for test data generation.

## Goals / Non-Goals

**Goals:**
- Add unit tests for core abstractions: Result, Result<T>, Error, ErrorCategory, ResultExtensions, ValidationBehavior, EntityConfiguration
- Add unit tests for all 6 FluentValidation validators across Auth, Chat, and UserManagement
- Add unit tests for 4 Chat feature handlers: CreateConversation, GetConversations, GetConversationMessages, SendMessage
- Add Bogus package and create shared Faker configurations for domain entities
- Follow existing test patterns: constructor DI, Moq for interfaces, `[Trait("Category", "Unit")]`

**Non-Goals:**
- Integration tests with Testcontainers (separate change)
- Splitting into Unit/Integration test projects (separate change)
- E2E tests
- Frontend tests

## Decisions

### 1. Keep single test project
**Decision**: Add tests to existing `GolMetrics.API.Tests` rather than splitting into Unit/Integration projects.
**Rationale**: The spec calls for a split, but this change only adds unit tests. Splitting is deferred to when integration tests are introduced.

### 2. Use InMemoryDatabase for Chat handler tests
**Decision**: Use `Microsoft.EntityFrameworkCore.InMemory` for Chat handler tests that require `GolMetricsDbContext`.
**Rationale**: Existing tests (TokenServiceTests, CacheServiceTests) already use InMemory. Consistency within the unit test suite matters more than switching to Testcontainers for unit tests. Integration tests (future change) will use Testcontainers.

### 3. Test file organization
**Decision**: Mirror production code structure:
- `Core/Results/` for Result/Error tests
- `Core/Behaviors/` for ValidationBehavior tests
- `Features/Auth/Validators/` for Auth validator tests
- `Features/Chat/` for Chat handler and validator tests
- `Features/UserManagement/Validators/` for UserManagement validator tests

### 4. Bogus Faker setup
**Decision**: Add a `Common/Fakers/` directory with entity Faker configs (ConversationFaker, MessageFaker) reusable across test classes.
**Rationale**: Avoids duplication of test data setup across Chat handler tests. Keep Fakers minimal - only for entities used in multiple test classes.

### 5. InternalsVisibleTo
**Decision**: Ensure the API project exposes internals to the test project so we can test `internal sealed` validators and handlers directly.
**Rationale**: Vertical slice classes are `internal sealed` by convention. Tests must access them. The project likely already has this configured.

## Risks / Trade-offs

- **InMemory vs real DB**: InMemory doesn't enforce constraints, FK relationships, or snake_case naming. Accepted for unit tests; integration tests will cover DB behavior.
- **Validator test coverage**: Testing FluentValidation rules is straightforward but verbose. Each rule needs valid/invalid scenarios. Worth it because validators are the first line of defense against bad input.
- **Chat handler complexity**: `SendMessage.Handler` has significant logic (auto-create conversation, chat history retrieval, AI service call, error handling). Tests will need careful mocking of `GolMetricsDbContext` or use InMemory provider.
