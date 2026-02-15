# Testing Standards

## Purpose

Defines testing conventions, tooling, and coverage standards for backend tests.

## Requirements

### Requirement: Test Pyramid

The system SHALL follow a test pyramid distribution.

#### Scenario: Coverage distribution

- **WHEN** tests are written
- **THEN** unit tests SHALL comprise ~70% of test coverage (handlers, validators, services)
- **AND** integration tests SHALL comprise ~25% of test coverage (database, external API clients)
- **AND** E2E tests SHALL comprise ~5% of test coverage (full user flows)

#### Scenario: Coverage target

- **WHEN** overall code coverage is measured
- **THEN** it SHALL exceed 70%

### Requirement: Unit Testing

The system SHALL use xUnit with supporting libraries for unit tests.

#### Scenario: Test framework

- **WHEN** unit tests are written
- **THEN** they SHALL use xUnit as the test framework
- **AND** they SHALL use FluentAssertions for assertions
- **AND** they SHALL use Moq for mocking interfaces (`ISemanticKernelService`, `IFootballApiClient`, `IEncryptionService`)
- **AND** they SHALL use Bogus for test data generation

#### Scenario: Handler test coverage

- **WHEN** handler tests are written
- **THEN** they SHALL verify correct `Result<T>` return values for both success and failure paths

#### Scenario: Validator test coverage

- **WHEN** validator tests are written
- **THEN** they SHALL verify FluentValidation rules for valid and invalid inputs

### Requirement: Integration Testing

The system SHALL use Testcontainers for real database testing.

#### Scenario: Database container lifecycle

- **WHEN** an integration test starts
- **THEN** it SHALL spin up a PostgreSQL container using `postgres:16-alpine` via Testcontainers
- **AND** it SHALL apply migrations via `Database.MigrateAsync()`

#### Scenario: Container disposal

- **WHEN** an integration test completes
- **THEN** it SHALL dispose the container

#### Scenario: Repository tests

- **WHEN** repository tests are written
- **THEN** they SHALL verify actual EF Core queries against a real database

### Requirement: Mocking External Services

The system SHALL never call external APIs during automated tests.

#### Scenario: Fake API client

- **WHEN** unit or integration tests need an API client
- **THEN** they SHALL use fake implementations of `IFootballApiClient`

#### Scenario: No external calls

- **WHEN** automated tests execute
- **THEN** external API calls SHALL never be made

### Requirement: Test Organization

Test projects SHALL follow consistent naming and categorization conventions.

#### Scenario: Project naming

- **WHEN** test projects are created
- **THEN** they SHALL follow the naming convention `GolMetrics.API.Tests.Unit` and `GolMetrics.API.Tests.Integration`

#### Scenario: Test categorization

- **WHEN** tests are written
- **THEN** they SHALL be categorized via `[Trait("Category", "Unit")]` or `[Trait("Category", "Integration")]`

#### Scenario: Filtered unit test execution

- **WHEN** `dotnet test --filter Category=Unit` is run
- **THEN** only unit tests SHALL execute

#### Scenario: Filtered integration test execution

- **WHEN** `dotnet test --filter Category=Integration` is run
- **THEN** only integration tests SHALL execute
