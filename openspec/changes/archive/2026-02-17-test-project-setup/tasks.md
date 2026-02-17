## 1. Setup and Dependencies

- [x] 1.1 Add Bogus NuGet package to `tests/GolMetrics.API.Tests/GolMetrics.API.Tests.csproj`: `dotnet add tests/GolMetrics.API.Tests/ package Bogus`
- [x] 1.2 Verify `InternalsVisibleTo` is configured in `src/GolMetrics.API/GolMetrics.API.csproj` for the test project; add if missing
- [x] 1.3 Create Bogus Faker configurations at `tests/GolMetrics.API.Tests/Common/Fakers/ConversationFaker.cs` and `tests/GolMetrics.API.Tests/Common/Fakers/MessageFaker.cs`
- [x] 1.4 Verify build: `dotnet build tests/GolMetrics.API.Tests/`

## 2. Core Abstraction Tests

- [x] 2.1 Create `tests/GolMetrics.API.Tests/Core/Results/ResultTests.cs` with tests for `Result.Success()`, `Result.Failure()`, `Result<T>.Failure()`, and implicit conversion
- [x] 2.2 Create `tests/GolMetrics.API.Tests/Core/Results/ErrorTests.cs` with tests for record equality and property access
- [x] 2.3 Create `tests/GolMetrics.API.Tests/Core/Results/ResultExtensionsTests.cs` with tests for `ToProblemDetails()` mapping each `ErrorCategory` to correct HTTP status code, and throwing on success
- [x] 2.4 Create `tests/GolMetrics.API.Tests/Core/Behaviors/ValidationBehaviorTests.cs` with tests for no validators, passing validators, failing validators, and multiple validator aggregation
- [x] 2.5 Verify tests pass: `dotnet test tests/GolMetrics.API.Tests/ --filter "FullyQualifiedName~Core"`

## 3. Validator Tests

- [x] 3.1 Create `tests/GolMetrics.API.Tests/Features/Auth/LoginValidatorTests.cs` with tests for valid command, empty email, invalid email format, empty password
- [x] 3.2 Create `tests/GolMetrics.API.Tests/Features/Auth/RegisterValidatorTests.cs` with tests for valid command, empty email, invalid email, empty password, password shorter than 6 characters
- [x] 3.3 Create `tests/GolMetrics.API.Tests/Features/Auth/RefreshTokenValidatorTests.cs` with tests for valid token, empty token
- [x] 3.4 Create `tests/GolMetrics.API.Tests/Features/Chat/CreateConversationValidatorTests.cs` with tests for valid title, empty title, title exceeding 200 characters
- [x] 3.5 Create `tests/GolMetrics.API.Tests/Features/Chat/SendMessageValidatorTests.cs` with tests for valid content, empty content, content exceeding 4000 characters
- [x] 3.6 Create `tests/GolMetrics.API.Tests/Features/UserManagement/UpdateApiKeyValidatorTests.cs` with tests for valid API key, empty API key
- [x] 3.7 Verify tests pass: `dotnet test tests/GolMetrics.API.Tests/ --filter "FullyQualifiedName~Validator"`

## 4. Chat Handler Tests

- [x] 4.1 Create `tests/GolMetrics.API.Tests/Features/Chat/CreateConversationHandlerTests.cs` with test for successful conversation creation
- [x] 4.2 Create `tests/GolMetrics.API.Tests/Features/Chat/GetConversationsHandlerTests.cs` with tests for returning user conversations ordered by recent, empty list, and filtering out other users
- [x] 4.3 Create `tests/GolMetrics.API.Tests/Features/Chat/GetConversationMessagesHandlerTests.cs` with tests for returning ordered messages and conversation-not-found failure
- [x] 4.4 Create `tests/GolMetrics.API.Tests/Features/Chat/SendMessageHandlerTests.cs` with tests for sending to existing conversation, auto-creating conversation, conversation not found, and AI service failure
- [x] 4.5 Verify all tests pass: `dotnet test tests/GolMetrics.API.Tests/`
