# Frontend Features

## Purpose

Specifies the user-facing screens: Authentication (login/register), Chat (messaging and conversation history), and Settings (BYOK API key management).

## Requirements

### Requirement: Login Screen

The application SHALL provide a login screen at `/login`.

#### Scenario: Login form rendering

- **WHEN** the login page loads
- **THEN** it SHALL render email and password fields with a submit button
- **AND** it SHALL display the application branding ("GOL METRICS")
- **AND** it SHALL use the shared AuthLayout with Background component and glass-panel card

#### Scenario: Successful login

- **WHEN** valid credentials are submitted
- **THEN** the application SHALL call `POST /api/auth/login`
- **AND** it SHALL decode the JWT to extract user ID and email
- **AND** it SHALL call `useAuthStore().login(accessToken, { id, email })`
- **AND** it SHALL redirect to `/chat`

#### Scenario: Invalid credentials

- **WHEN** the API returns HTTP 401
- **THEN** the application SHALL display an inline error message below the form

#### Scenario: Registration link

- **WHEN** the login page is displayed
- **THEN** it SHALL include a link to `/register`

#### Scenario: Form submission state

- **WHEN** the form is being submitted
- **THEN** the submit button SHALL be disabled and display a loading indicator
- **AND** all form fields SHALL be disabled

#### Scenario: Client-side validation

- **WHEN** the user submits the form with empty fields
- **THEN** the application SHALL display validation errors without making an API call

### Requirement: Register Screen

The application SHALL provide a registration screen at `/register`.

#### Scenario: Registration form rendering

- **WHEN** the register page loads
- **THEN** it SHALL render email, password, and confirm password fields with a submit button
- **AND** it SHALL display the application branding ("GOL METRICS")
- **AND** it SHALL use the shared AuthLayout with Background component and glass-panel card

#### Scenario: Successful registration

- **WHEN** valid credentials are submitted
- **THEN** the application SHALL call `POST /api/auth/register`
- **AND** it SHALL decode the JWT to extract user ID and email
- **AND** it SHALL call `useAuthStore().login(accessToken, { id, email })`
- **AND** it SHALL redirect to `/chat`

#### Scenario: Duplicate email

- **WHEN** the API returns HTTP 409
- **THEN** the application SHALL display an inline error message below the form

#### Scenario: Invalid password

- **WHEN** the API returns HTTP 400
- **THEN** the application SHALL display validation errors inline below the form

#### Scenario: Login link

- **WHEN** the register page is displayed
- **THEN** it SHALL include a link to `/login`

#### Scenario: Password confirmation mismatch

- **WHEN** the password and confirm password fields do not match
- **THEN** the application SHALL display a validation error without making an API call

#### Scenario: Form submission state

- **WHEN** the form is being submitted
- **THEN** the submit button SHALL be disabled and display a loading indicator
- **AND** all form fields SHALL be disabled

#### Scenario: Client-side validation

- **WHEN** the user submits the form with empty fields
- **THEN** the application SHALL display validation errors without making an API call

### Requirement: Chat Screen

The application SHALL provide a chat screen at `/chat` with a conversation sidebar and real-time API integration.

#### Scenario: Conversation sidebar

- **WHEN** the chat page loads
- **THEN** it SHALL fetch conversations via `GET /api/conversations`
- **AND** it SHALL display a sidebar with the user's conversation list ordered by most recent (`updatedAt` descending)
- **AND** each conversation item SHALL display the title and a relative timestamp

#### Scenario: New conversation button

- **WHEN** the chat page is displayed
- **THEN** the sidebar SHALL include a "New Conversation" button at the top
- **AND** clicking it SHALL clear the active conversation and message list, allowing the user to start a fresh chat

#### Scenario: Active conversation messages

- **WHEN** a conversation is selected from the sidebar
- **THEN** it SHALL fetch messages via `GET /api/conversations/{id}/messages`
- **AND** it SHALL display them in a scrollable area ordered by timestamp ascending
- **AND** user messages SHALL appear right-aligned and assistant messages left-aligned

#### Scenario: Sending a message with active conversation

- **WHEN** the user sends a message while a conversation is active
- **THEN** the application SHALL call `POST /api/chat/message` with `content` and `conversationId`
- **AND** it SHALL immediately display the user's message in the chat area
- **AND** it SHALL show a typing indicator while waiting for the AI response
- **AND** it SHALL display the assistant's response when received

#### Scenario: Sending a message without active conversation

- **WHEN** the user sends a message without an active conversation
- **THEN** the application SHALL call `POST /api/chat/message` with `content` only (no `conversationId`)
- **AND** the backend SHALL auto-create a conversation
- **AND** the new conversation SHALL appear in the sidebar
- **AND** it SHALL become the active conversation

#### Scenario: Markdown rendering in assistant messages

- **WHEN** an assistant message contains Markdown content (including tables, bold, lists, code blocks)
- **THEN** the application SHALL render it as formatted HTML using `react-markdown` with `remark-gfm`
- **AND** Markdown tables SHALL render as styled HTML tables

#### Scenario: Auto-scroll on new messages

- **WHEN** a new message is added to the conversation (user or assistant)
- **THEN** the view SHALL auto-scroll to the latest message

#### Scenario: AI processing indicator

- **WHEN** a message has been sent and the AI response is pending
- **THEN** the application SHALL display an animated typing indicator in the assistant message area
- **AND** the message input SHALL be disabled until the response is received

#### Scenario: API error on send message

- **WHEN** `POST /api/chat/message` returns an error
- **THEN** the application SHALL display an error toast notification with the error message
- **AND** the typing indicator SHALL be removed
- **AND** the message input SHALL be re-enabled

#### Scenario: API error on load conversations

- **WHEN** `GET /api/conversations` returns an error
- **THEN** the application SHALL display an error toast notification

#### Scenario: API error on load messages

- **WHEN** `GET /api/conversations/{id}/messages` returns an error
- **THEN** the application SHALL display an error toast notification
- **AND** the active conversation SHALL be deselected

#### Scenario: Empty state

- **WHEN** the user has no conversations
- **THEN** the application SHALL display a centered empty state with an illustration and prompt text encouraging the user to start a conversation

#### Scenario: Sidebar responsive behavior

- **WHEN** the viewport width is below the `md` breakpoint
- **THEN** the sidebar SHALL be hidden by default
- **AND** a toggle button SHALL be visible to show/hide the sidebar as an overlay

#### Scenario: Chat store state management

- **WHEN** the chat page mounts
- **THEN** the application SHALL use a Zustand `useChatStore` with state: `conversations`, `activeConversationId`, `messages`, `isLoadingConversations`, `isLoadingMessages`, `isSending`
- **AND** the store SHALL expose actions: `fetchConversations()`, `selectConversation(id)`, `sendMessage(content)`, `startNewConversation()`

### Requirement: Settings Screen

The application SHALL provide a settings screen at `/settings` with profile display and API key management.

#### Scenario: Page layout

- **WHEN** the settings page loads
- **THEN** it SHALL use the shared Background and Header layout components
- **AND** it SHALL display a centered card with two sections: Profile Information and API Key Management

#### Scenario: Profile info display

- **WHEN** the settings page loads
- **THEN** it SHALL fetch the user profile via `GET /api/user/profile`
- **AND** it SHALL display the user's email address
- **AND** it SHALL display the account creation date formatted as a readable date
- **AND** it SHALL display the API key status as a badge indicating whether a key is configured

#### Scenario: Profile loading state

- **WHEN** the profile data is being fetched
- **THEN** the profile section SHALL display skeleton placeholders

#### Scenario: Profile fetch error

- **WHEN** `GET /api/user/profile` returns an error
- **THEN** the application SHALL display an error toast notification

#### Scenario: API key input form

- **WHEN** the settings page is displayed
- **THEN** it SHALL provide a password-type input field for entering an API-Football API key
- **AND** it SHALL include a show/hide toggle button for the input
- **AND** it SHALL include a submit button labeled "Save API Key"

#### Scenario: API key client-side validation

- **WHEN** the user submits an empty API key field
- **THEN** the application SHALL display a validation error without making an API call

#### Scenario: Successful API key submission

- **WHEN** a key is submitted via `PUT /api/user/api-key` and the API returns HTTP 200
- **THEN** the application SHALL display a success toast notification
- **AND** it SHALL re-fetch the user profile to update the `hasApiKey` status
- **AND** it SHALL clear the API key input field

#### Scenario: Invalid API key submission

- **WHEN** the API returns HTTP 400 for an invalid API key
- **THEN** the application SHALL display an error toast notification with the error message

#### Scenario: API key validation unavailable

- **WHEN** the API returns HTTP 502 (API-Football validation service unavailable)
- **THEN** the application SHALL display an error toast notification indicating the validation service is unavailable

#### Scenario: API key form submission state

- **WHEN** the API key form is being submitted
- **THEN** the submit button SHALL be disabled and display a loading indicator
- **AND** the API key input field SHALL be disabled

#### Scenario: Settings store state management

- **WHEN** the settings page mounts
- **THEN** the application SHALL use a Zustand `useSettingsStore` with state: `profile` (nullable), `isLoadingProfile`, `isSubmittingApiKey`
- **AND** the store SHALL expose actions: `fetchProfile()`, `submitApiKey(key: string)`

#### Scenario: Settings navigation

- **WHEN** the user is on any authenticated page
- **THEN** the Header component SHALL include a settings icon button that navigates to `/settings`

### Requirement: UI States

The application SHALL provide consistent loading, error, and empty states.

#### Scenario: Loading state

- **WHEN** conversations are being fetched
- **THEN** the sidebar SHALL display skeleton placeholders

#### Scenario: Message loading state

- **WHEN** messages for a conversation are being fetched
- **THEN** the chat area SHALL display a centered loading spinner

#### Scenario: Typing indicator

- **WHEN** the AI is processing a response
- **THEN** the chat area SHALL display an animated "Typing..." indicator styled as an assistant message bubble

#### Scenario: Error state

- **WHEN** an API operation fails
- **THEN** the application SHALL display a toast notification with the error message using `sonner`

#### Scenario: Empty state

- **WHEN** no conversations exist
- **THEN** the application SHALL display an illustration with prompt text in the main chat area
