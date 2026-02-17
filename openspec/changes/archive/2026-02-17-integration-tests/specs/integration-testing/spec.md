## ADDED Requirements

### Requirement: Testcontainers PostgreSQL fixture
The test suite SHALL provide a shared PostgreSQL container for integration tests using Testcontainers.

#### Scenario: Container startup and migration
- **WHEN** the integration test collection starts
- **THEN** a `postgres:16-alpine` container SHALL be started via Testcontainers
- **AND** EF Core migrations SHALL be applied via `Database.MigrateAsync()`

#### Scenario: Container shared across tests
- **WHEN** multiple integration test classes run
- **THEN** they SHALL share a single PostgreSQL container instance via xUnit `CollectionFixture`

#### Scenario: Container disposal
- **WHEN** all integration tests in the collection complete
- **THEN** the container SHALL be disposed

### Requirement: CustomWebApplicationFactory
The test suite SHALL provide a `WebApplicationFactory<Program>` subclass for integration tests.

#### Scenario: Database replacement
- **WHEN** the factory creates the test server
- **THEN** it SHALL replace the `GolMetricsDbContext` connection string with the Testcontainers PostgreSQL instance

#### Scenario: External service mocking
- **WHEN** the factory configures services
- **THEN** `ISemanticKernelService` SHALL be replaced with a mock returning deterministic responses
- **AND** `IFootballApiClient` SHALL be replaced with a mock

#### Scenario: Real pipeline preserved
- **WHEN** the factory creates the test server
- **THEN** MediatR, FluentValidation pipeline, exception handlers, Identity, and JWT authentication SHALL remain configured as in production

### Requirement: Database reset between tests
The test suite SHALL reset database state between tests using Respawn.

#### Scenario: Clean state per test
- **WHEN** an integration test begins
- **THEN** Respawn SHALL have cleared all user data from the database
- **AND** the `__EFMigrationsHistory` table SHALL be preserved

### Requirement: Auth integration test helper
The test suite SHALL provide a helper for creating authenticated HTTP clients.

#### Scenario: Register and authenticate
- **WHEN** a test needs an authenticated client
- **THEN** the helper SHALL register a user via `POST /api/auth/register`
- **AND** it SHALL return an `HttpClient` with the JWT `Authorization: Bearer` header set

### Requirement: Auth registration integration test
The test suite SHALL verify user registration through the API.

#### Scenario: Successful registration
- **WHEN** `POST /api/auth/register` is called with a valid email and password (6+ characters)
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain `accessToken` and `refreshToken`

#### Scenario: Duplicate email registration
- **WHEN** `POST /api/auth/register` is called with an email that already exists
- **THEN** the response SHALL be HTTP 409
- **AND** the response body SHALL be a ProblemDetails with the `Auth.DuplicateEmail` error code

#### Scenario: Invalid registration data
- **WHEN** `POST /api/auth/register` is called with an empty email or password shorter than 6 characters
- **THEN** the response SHALL be HTTP 400

### Requirement: Auth login integration test
The test suite SHALL verify user login through the API.

#### Scenario: Successful login
- **WHEN** a registered user calls `POST /api/auth/login` with correct credentials
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain `accessToken` and `refreshToken`

#### Scenario: Invalid credentials
- **WHEN** `POST /api/auth/login` is called with incorrect password
- **THEN** the response SHALL be HTTP 401
- **AND** the response body SHALL be a ProblemDetails with the `Auth.InvalidCredentials` error code

#### Scenario: Nonexistent user login
- **WHEN** `POST /api/auth/login` is called with an email that does not exist
- **THEN** the response SHALL be HTTP 401

### Requirement: Auth refresh token integration test
The test suite SHALL verify token refresh through the API.

#### Scenario: Successful token refresh
- **WHEN** `POST /api/auth/refresh-token` is called with a valid refresh token
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain a new `accessToken` and `refreshToken`

#### Scenario: Invalid refresh token
- **WHEN** `POST /api/auth/refresh-token` is called with an invalid or expired token
- **THEN** the response SHALL be HTTP 401

### Requirement: Chat create conversation integration test
The test suite SHALL verify conversation creation through the API.

#### Scenario: Successful conversation creation
- **WHEN** an authenticated user calls `POST /api/conversations` with a valid title
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain the conversation `id` and `title`

#### Scenario: Unauthenticated conversation creation
- **WHEN** `POST /api/conversations` is called without a JWT token
- **THEN** the response SHALL be HTTP 401

#### Scenario: Invalid conversation title
- **WHEN** `POST /api/conversations` is called with an empty title or title exceeding 200 characters
- **THEN** the response SHALL be HTTP 400

### Requirement: Chat send message integration test
The test suite SHALL verify message sending through the API.

#### Scenario: Send message to existing conversation
- **WHEN** an authenticated user calls `POST /api/chat/message` with a valid `conversationId` and `content`
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain the AI assistant's response
- **AND** two messages (user and assistant) SHALL be persisted in the database

#### Scenario: Send message without conversation (auto-create)
- **WHEN** an authenticated user calls `POST /api/chat/message` with `content` but no `conversationId`
- **THEN** the response SHALL be HTTP 200
- **AND** a new conversation SHALL be created with the message content as title

#### Scenario: Send message to nonexistent conversation
- **WHEN** an authenticated user calls `POST /api/chat/message` with a non-existent `conversationId`
- **THEN** the response SHALL be HTTP 404

#### Scenario: Send message to another user's conversation
- **WHEN** an authenticated user calls `POST /api/chat/message` with a `conversationId` belonging to a different user
- **THEN** the response SHALL be HTTP 404

### Requirement: Chat get conversations integration test
The test suite SHALL verify listing conversations through the API.

#### Scenario: Returns user conversations ordered by recency
- **WHEN** an authenticated user calls `GET /api/conversations`
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain only the authenticated user's conversations
- **AND** conversations SHALL be ordered by most recently updated first

#### Scenario: Empty conversation list
- **WHEN** an authenticated user with no conversations calls `GET /api/conversations`
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL be an empty array

### Requirement: Chat get conversation messages integration test
The test suite SHALL verify retrieving messages for a conversation through the API.

#### Scenario: Returns messages ordered by timestamp
- **WHEN** an authenticated user calls `GET /api/conversations/{id}/messages` for their own conversation
- **THEN** the response SHALL be HTTP 200
- **AND** the response body SHALL contain messages ordered by timestamp ascending

#### Scenario: Conversation not found
- **WHEN** an authenticated user calls `GET /api/conversations/{id}/messages` with a non-existent or other user's conversation ID
- **THEN** the response SHALL be HTTP 404

### Requirement: Test categorization
All integration tests SHALL be categorized for filtered execution.

#### Scenario: Trait annotation
- **WHEN** an integration test class is created
- **THEN** it SHALL be annotated with `[Trait("Category", "Integration")]`

#### Scenario: Filtered execution
- **WHEN** `dotnet test --filter Category=Integration` is run
- **THEN** only integration tests SHALL execute
