# Frontend Features

## Purpose

Specifies the user-facing screens: Authentication (login/register), Chat (messaging and conversation history), and Settings (BYOK API key management).

## Requirements

### Requirement: Login Screen

The application SHALL provide a login screen at `/login`.

#### Scenario: Login form rendering

- **WHEN** the login page loads
- **THEN** it SHALL render email and password fields with a submit button

#### Scenario: Successful login

- **WHEN** valid credentials are submitted
- **THEN** the application SHALL call `POST /api/auth/login`, store the JWT, and redirect to `/chat`

#### Scenario: Invalid credentials

- **WHEN** the API returns HTTP 401
- **THEN** the application SHALL display an inline error message

#### Scenario: Registration link

- **WHEN** the login page is displayed
- **THEN** it SHALL include a link to `/register`

### Requirement: Register Screen

The application SHALL provide a registration screen at `/register`.

#### Scenario: Registration form rendering

- **WHEN** the register page loads
- **THEN** it SHALL render email and password fields with a submit button

#### Scenario: Successful registration

- **WHEN** valid credentials are submitted
- **THEN** the application SHALL call `POST /api/auth/register`, store the JWT, and redirect to `/chat`

#### Scenario: Duplicate email

- **WHEN** the API returns HTTP 409
- **THEN** the application SHALL display an inline error message

#### Scenario: Invalid password

- **WHEN** the API returns HTTP 400
- **THEN** the application SHALL display validation errors

#### Scenario: Login link

- **WHEN** the register page is displayed
- **THEN** it SHALL include a link to `/login`

### Requirement: Chat Screen

The application SHALL provide a chat screen at `/chat`.

#### Scenario: Conversation sidebar

- **WHEN** the chat page loads
- **THEN** it SHALL display a sidebar with the user's conversation list ordered by most recent

#### Scenario: Active conversation messages

- **WHEN** a conversation is selected from the sidebar
- **THEN** it SHALL load its messages via `GET /api/conversations/{id}/messages` and display them in a scrollable area

#### Scenario: Sending a message

- **WHEN** the user sends a message
- **THEN** the application SHALL call `POST /api/chat/message` with `content` and optional `conversationId`

#### Scenario: New conversation from message

- **WHEN** a message is sent without a conversation
- **THEN** the new conversation SHALL appear in the sidebar

#### Scenario: Auto-scroll

- **WHEN** a new message is added to the conversation
- **THEN** the view SHALL auto-scroll to the latest message

#### Scenario: AI processing indicator

- **WHEN** the AI is processing a response
- **THEN** the application SHALL display a typing/loading indicator

#### Scenario: API error handling

- **WHEN** the API returns an error
- **THEN** the application SHALL display an error toast

#### Scenario: Markdown table rendering

- **WHEN** an assistant message contains Markdown tables
- **THEN** they SHALL render as formatted HTML tables

#### Scenario: New conversation button

- **WHEN** the chat page is displayed
- **THEN** it SHALL provide a "New Conversation" button in the sidebar

### Requirement: Settings Screen

The application SHALL provide a settings screen at `/settings`.

#### Scenario: Profile info display

- **WHEN** the settings page loads
- **THEN** it SHALL display the user's email, account creation date, and API key status

#### Scenario: API key input

- **WHEN** the settings page is displayed
- **THEN** it SHALL provide an input field for entering an API-Football API key

#### Scenario: Successful API key submission

- **WHEN** a key is submitted and the API returns HTTP 200
- **THEN** the application SHALL display a success indicator and update the `hasApiKey` status

#### Scenario: Invalid API key

- **WHEN** the API returns HTTP 400
- **THEN** the application SHALL display an error message

### Requirement: UI States

The application SHALL provide consistent loading, error, and empty states.

#### Scenario: Loading state

- **WHEN** data is being fetched
- **THEN** the application SHALL display skeleton placeholders and a "Typing..." indicator for AI responses

#### Scenario: Error state

- **WHEN** an operation fails
- **THEN** the application SHALL display a toast notification with the error message

#### Scenario: Empty state

- **WHEN** no conversations exist
- **THEN** the application SHALL display an illustration with prompt text
