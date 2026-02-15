# Chat

## Purpose

Orchestrates the chat experience: sending messages through the AI pipeline, managing conversations, and persisting chat history.

## Requirements

### Requirement: Send Message

The system SHALL process user messages via `POST /api/chat/message`.

#### Scenario: Successful message with existing conversation

- **WHEN** an authenticated user sends a message with `content` and `conversationId`
- **THEN** the system SHALL persist the user message with role `User`
- **AND** it SHALL process the message through `SemanticKernelService` with conversation history
- **AND** it SHALL persist the AI response with role `Assistant`
- **AND** it SHALL return HTTP 200 with the assistant message content and `conversationId`

#### Scenario: Successful message without conversation

- **WHEN** an authenticated user sends a message with `content` but no `conversationId`
- **THEN** the system SHALL auto-create a new Conversation with a title set to the first 100 characters of the user's message content, truncated at a word boundary
- **AND** it SHALL process and persist messages as above
- **AND** it SHALL return HTTP 200 with the assistant message content and the new `conversationId`

#### Scenario: Empty content

- **WHEN** a message is sent with empty or whitespace-only content
- **THEN** the system SHALL return HTTP 400 Bad Request
- **AND** the validation error SHALL indicate content is required

#### Scenario: Content too long

- **WHEN** a message is sent with content exceeding 4000 characters
- **THEN** the system SHALL return HTTP 400 Bad Request
- **AND** the validation error SHALL indicate the maximum length

#### Scenario: Conversation not found

- **WHEN** a message is sent with a `conversationId` that does not exist
- **THEN** the system SHALL return HTTP 404 Not Found
- **AND** the error SHALL use `ChatErrors.ConversationNotFound`

#### Scenario: Conversation not owned

- **WHEN** a message is sent with a `conversationId` belonging to another user
- **THEN** the system SHALL return HTTP 404 Not Found
- **AND** the error SHALL use `ChatErrors.ConversationNotFound` (same as not found to prevent enumeration)

#### Scenario: AI processing failure

- **WHEN** `SemanticKernelService` fails to process the message
- **THEN** the system SHALL return HTTP 502 with `ChatErrors.AiProcessingFailed`
- **AND** it SHALL NOT persist the assistant message (the user message is already persisted)

### Requirement: List Conversations

The system SHALL return the authenticated user's conversations via `GET /api/conversations`.

#### Scenario: Successful listing

- **WHEN** an authenticated user requests their conversations
- **THEN** the system SHALL return HTTP 200 with conversations ordered by `UpdatedAtUtc` descending
- **AND** each conversation SHALL include `id`, `title`, `createdAt`, `updatedAt`

#### Scenario: No conversations

- **WHEN** an authenticated user has no conversations
- **THEN** the system SHALL return HTTP 200 with an empty array

### Requirement: Get Conversation Messages

The system SHALL return messages for a conversation via `GET /api/conversations/{id}/messages`.

#### Scenario: Successful retrieval

- **WHEN** an authenticated user requests messages for their own conversation
- **THEN** the system SHALL return HTTP 200 with messages ordered by `Timestamp` ascending
- **AND** each message SHALL include `id`, `content`, `role`, `timestamp`

#### Scenario: Conversation not found or not owned

- **WHEN** a user requests messages for a non-existent or another user's conversation
- **THEN** the system SHALL return HTTP 404 Not Found
- **AND** the error SHALL use `ChatErrors.ConversationNotFound`

### Requirement: Create Conversation

The system SHALL allow creating a conversation via `POST /api/conversations`.

#### Scenario: Successful creation

- **WHEN** an authenticated user creates a conversation with a `title`
- **THEN** the system SHALL persist the conversation and return HTTP 201 with `id` and `title`

#### Scenario: Empty title

- **WHEN** a conversation is created with an empty title
- **THEN** the system SHALL return HTTP 400 Bad Request

### Requirement: Feature Errors

The system SHALL define chat errors in `ChatErrors.cs`.

#### Scenario: Error definitions

- **WHEN** chat errors are needed
- **THEN** `ChatErrors` SHALL define static properties: `ConversationNotFound` (NotFound), `EmptyContent` (BadRequest), `ContentTooLong` (BadRequest), `AiProcessingFailed` (BadRequest)
