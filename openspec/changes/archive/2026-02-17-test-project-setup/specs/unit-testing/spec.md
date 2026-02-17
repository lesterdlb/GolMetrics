## ADDED Requirements

### Requirement: Result and Result<T> unit tests
The test suite SHALL verify the `Result` and `Result<T>` core abstractions for success/failure creation, property access, and implicit conversion.

#### Scenario: Result.Success creates a successful result
- **WHEN** `Result.Success()` is called
- **THEN** `IsSuccess` SHALL be `true`
- **AND** `Error` SHALL be `null`

#### Scenario: Result.Failure creates a failed result
- **WHEN** `Result.Failure(error)` is called with a valid `Error`
- **THEN** `IsSuccess` SHALL be `false`
- **AND** `Error` SHALL be the provided error

#### Scenario: Result<T>.Failure creates a failed typed result
- **WHEN** `Result<T>.Failure(error)` is called
- **THEN** `IsSuccess` SHALL be `false`
- **AND** `Value` SHALL be `default`
- **AND** `Error` SHALL be the provided error

#### Scenario: Implicit conversion creates successful Result<T>
- **WHEN** a value of type `T` is implicitly converted to `Result<T>`
- **THEN** `IsSuccess` SHALL be `true`
- **AND** `Value` SHALL be the provided value
- **AND** `Error` SHALL be `null`

### Requirement: Error record unit tests
The test suite SHALL verify that `Error` record equality and properties work correctly.

#### Scenario: Error records with same values are equal
- **WHEN** two `Error` instances are created with identical `Code`, `Message`, and `ErrorCategory`
- **THEN** they SHALL be equal

#### Scenario: Error records with different values are not equal
- **WHEN** two `Error` instances differ in any property
- **THEN** they SHALL NOT be equal

### Requirement: ResultExtensions.ToProblemDetails unit tests
The test suite SHALL verify that `ToProblemDetails()` maps each `ErrorCategory` to the correct HTTP status code.

#### Scenario: Each ErrorCategory maps to correct status code
- **WHEN** `ToProblemDetails()` is called on a failed `Result` with a given `ErrorCategory`
- **THEN** `BadRequest` SHALL map to 400
- **AND** `Unauthorized` SHALL map to 401
- **AND** `Forbidden` SHALL map to 403
- **AND** `NotFound` SHALL map to 404
- **AND** `Conflict` SHALL map to 409
- **AND** `BadGateway` SHALL map to 502

#### Scenario: ToProblemDetails throws on successful result
- **WHEN** `ToProblemDetails()` is called on a successful `Result`
- **THEN** it SHALL throw `InvalidOperationException`

### Requirement: ValidationBehavior unit tests
The test suite SHALL verify the MediatR `ValidationBehavior<TRequest, TResponse>` pipeline behavior.

#### Scenario: No validators registered passes through
- **WHEN** a request is sent through the pipeline with no registered validators
- **THEN** the handler SHALL be called and return its response

#### Scenario: All validators pass allows handler execution
- **WHEN** all registered validators return no errors
- **THEN** the handler SHALL be called and return its response

#### Scenario: Validation failures throw ValidationException
- **WHEN** any registered validator returns validation errors
- **THEN** a `ValidationException` SHALL be thrown
- **AND** the handler SHALL NOT be called

#### Scenario: Multiple validators are all executed
- **WHEN** multiple validators are registered for a request
- **THEN** all validators SHALL be executed
- **AND** errors from all validators SHALL be aggregated

### Requirement: Login validator unit tests
The test suite SHALL verify `Login.Validator` rules: Email (NotEmpty, EmailAddress), Password (NotEmpty).

#### Scenario: Valid login command passes validation
- **WHEN** a `Login.Command` has a valid email and non-empty password
- **THEN** validation SHALL pass with no errors

#### Scenario: Empty email fails validation
- **WHEN** `Email` is empty
- **THEN** validation SHALL fail with an error on the `Email` property

#### Scenario: Invalid email format fails validation
- **WHEN** `Email` is not a valid email address
- **THEN** validation SHALL fail with an error on the `Email` property

#### Scenario: Empty password fails validation
- **WHEN** `Password` is empty
- **THEN** validation SHALL fail with an error on the `Password` property

### Requirement: Register validator unit tests
The test suite SHALL verify `Register.Validator` rules: Email (NotEmpty, EmailAddress), Password (NotEmpty, MinimumLength(6)).

#### Scenario: Valid register command passes validation
- **WHEN** a `Register.Command` has a valid email and password with 6+ characters
- **THEN** validation SHALL pass with no errors

#### Scenario: Password shorter than 6 characters fails validation
- **WHEN** `Password` has fewer than 6 characters
- **THEN** validation SHALL fail with an error on the `Password` property

#### Scenario: Empty email fails validation
- **WHEN** `Email` is empty
- **THEN** validation SHALL fail with an error on the `Email` property

### Requirement: RefreshToken validator unit tests
The test suite SHALL verify `RefreshToken.Validator` rules: Token (NotEmpty).

#### Scenario: Valid token passes validation
- **WHEN** `Token` is non-empty
- **THEN** validation SHALL pass with no errors

#### Scenario: Empty token fails validation
- **WHEN** `Token` is empty
- **THEN** validation SHALL fail with an error on the `Token` property

### Requirement: CreateConversation validator unit tests
The test suite SHALL verify `CreateConversation.Validator` rules: Title (NotEmpty, MaximumLength(200)).

#### Scenario: Valid title passes validation
- **WHEN** `Title` is non-empty and 200 characters or fewer
- **THEN** validation SHALL pass with no errors

#### Scenario: Empty title fails validation
- **WHEN** `Title` is empty
- **THEN** validation SHALL fail with an error on the `Title` property

#### Scenario: Title exceeding 200 characters fails validation
- **WHEN** `Title` is longer than 200 characters
- **THEN** validation SHALL fail with an error on the `Title` property

### Requirement: SendMessage validator unit tests
The test suite SHALL verify `SendMessage.Validator` rules: Content (NotEmpty, MaximumLength(4000)).

#### Scenario: Valid content passes validation
- **WHEN** `Content` is non-empty and 4000 characters or fewer
- **THEN** validation SHALL pass with no errors

#### Scenario: Empty content fails validation
- **WHEN** `Content` is empty
- **THEN** validation SHALL fail with an error on the `Content` property

#### Scenario: Content exceeding 4000 characters fails validation
- **WHEN** `Content` is longer than 4000 characters
- **THEN** validation SHALL fail with an error on the `Content` property

### Requirement: UpdateApiKey validator unit tests
The test suite SHALL verify `UpdateApiKey.Validator` rules: ApiKey (NotEmpty).

#### Scenario: Valid API key passes validation
- **WHEN** `ApiKey` is non-empty
- **THEN** validation SHALL pass with no errors

#### Scenario: Empty API key fails validation
- **WHEN** `ApiKey` is empty
- **THEN** validation SHALL fail with an error on the `ApiKey` property

### Requirement: CreateConversation handler unit tests
The test suite SHALL verify the `CreateConversation.Handler` creates conversations and returns the correct response.

#### Scenario: Successfully creates a conversation
- **WHEN** a valid `CreateConversation.Command` is handled
- **THEN** a `Conversation` entity SHALL be added to the database
- **AND** the response SHALL contain the conversation `Id` and `Title`

### Requirement: GetConversations handler unit tests
The test suite SHALL verify the `GetConversations.Handler` returns user conversations ordered by most recent.

#### Scenario: Returns conversations for the user
- **WHEN** the user has conversations in the database
- **THEN** the handler SHALL return all conversations belonging to the user
- **AND** conversations SHALL be ordered by most recently updated first

#### Scenario: Returns empty list when user has no conversations
- **WHEN** the user has no conversations
- **THEN** the handler SHALL return an empty list

#### Scenario: Does not return other users' conversations
- **WHEN** other users have conversations in the database
- **THEN** the handler SHALL NOT include them in the result

### Requirement: GetConversationMessages handler unit tests
The test suite SHALL verify the `GetConversationMessages.Handler` returns messages for a conversation.

#### Scenario: Returns messages ordered by timestamp
- **WHEN** a valid conversation ID is queried by its owner
- **THEN** the handler SHALL return all messages ordered by `Timestamp` ascending

#### Scenario: Conversation not found returns failure
- **WHEN** the conversation ID does not exist or belongs to another user
- **THEN** the handler SHALL return `Result.Failure` with `ChatErrors.ConversationNotFound`

### Requirement: SendMessage handler unit tests
The test suite SHALL verify the `SendMessage.Handler` processes messages through the AI service.

#### Scenario: Sends message to existing conversation
- **WHEN** a message is sent with a valid `ConversationId`
- **THEN** a user `Message` SHALL be saved
- **AND** the AI service SHALL be called with the message and chat history
- **AND** an assistant `Message` SHALL be saved with the AI response
- **AND** the conversation's `UpdatedAtUtc` SHALL be set

#### Scenario: Auto-creates conversation when ConversationId is null
- **WHEN** a message is sent without a `ConversationId`
- **THEN** a new `Conversation` SHALL be created with the message content (truncated to 100 chars at word boundary) as the title
- **AND** the message flow SHALL proceed as normal

#### Scenario: Returns failure when conversation not found
- **WHEN** a message is sent with a `ConversationId` that does not exist or belongs to another user
- **THEN** the handler SHALL return `Result.Failure` with `ChatErrors.ConversationNotFound`

#### Scenario: Returns failure when AI service throws
- **WHEN** the `ISemanticKernelService.ProcessMessageAsync` throws any exception
- **THEN** the handler SHALL return `Result.Failure` with `ChatErrors.AiProcessingFailed`

### Requirement: Bogus test data generation
The test suite SHALL use Bogus `Faker` configurations for generating test entity data.

#### Scenario: Faker generates valid Conversation entities
- **WHEN** a `Conversation` is generated via Bogus
- **THEN** it SHALL have a non-empty `Title`, valid `UserId`, and populated audit fields

#### Scenario: Faker generates valid Message entities
- **WHEN** a `Message` is generated via Bogus
- **THEN** it SHALL have a non-empty `Content`, valid `ConversationId`, a `MessageRole`, and a `Timestamp`
