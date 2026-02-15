# E2E Testing

## Purpose

Defines end-to-end testing strategy, tooling, and critical user flows validated against the full stack (frontend, backend, database).

## Requirements

### Requirement: Test Tooling

The system SHALL use Playwright for end-to-end testing.

#### Scenario: Framework setup

- **WHEN** E2E tests are configured
- **THEN** they SHALL use Playwright with TypeScript
- **AND** they SHALL target Chromium at minimum

#### Scenario: Test location

- **WHEN** E2E test files are created
- **THEN** they SHALL be placed under `src/GolMetrics.Web/e2e/`

### Requirement: Test Infrastructure

E2E tests SHALL run against the full Docker Compose stack.

#### Scenario: Stack startup

- **WHEN** the E2E test suite starts
- **THEN** the full stack (`api`, `web`, `db`) SHALL be running via Docker Compose

#### Scenario: Test isolation

- **WHEN** a test requires an authenticated user
- **THEN** it SHALL register a unique user via `POST /api/auth/register` per test run
- **AND** it SHALL not depend on pre-existing database state

#### Scenario: External service stubbing

- **WHEN** tests exercise AI chat or football data flows
- **THEN** external APIs (API-Football, Google Gemini) SHALL be stubbed at the backend level
- **AND** no real external API calls SHALL be made during E2E tests

### Requirement: Authentication Flow

E2E tests SHALL verify the complete authentication lifecycle.

#### Scenario: Registration

- **WHEN** a user fills in valid credentials on `/register` and submits
- **THEN** they SHALL be redirected to `/chat`

#### Scenario: Logout and login

- **WHEN** an authenticated user logs out
- **THEN** they SHALL be redirected to `/login`
- **AND** when they log in with valid credentials, they SHALL be redirected to `/chat`

#### Scenario: Protected route enforcement

- **WHEN** an unauthenticated user navigates to `/chat`
- **THEN** they SHALL be redirected to `/login`

### Requirement: Chat Flow

E2E tests SHALL verify the core chat interaction cycle.

#### Scenario: Send and receive message

- **WHEN** an authenticated user types a message and submits
- **THEN** the message SHALL appear in the conversation area
- **AND** an AI response SHALL appear after the loading indicator

#### Scenario: Conversation persistence

- **WHEN** a message is sent without an active conversation
- **THEN** a new conversation SHALL appear in the sidebar

#### Scenario: Conversation switching

- **WHEN** a user selects a different conversation from the sidebar
- **THEN** that conversation's messages SHALL load and display

### Requirement: Settings Flow

E2E tests SHALL verify API key management.

#### Scenario: Submit valid API key

- **WHEN** an authenticated user navigates to `/settings` and submits an API key
- **THEN** a success indicator SHALL be displayed
- **AND** the API key status SHALL update

#### Scenario: Submit invalid API key

- **WHEN** an invalid API key is submitted
- **THEN** an error message SHALL be displayed

### Requirement: Cross-Cutting Behavior

E2E tests SHALL verify behaviors that span multiple features.

#### Scenario: Automatic logout on 401

- **WHEN** the backend returns HTTP 401 during an authenticated session
- **THEN** the user SHALL be redirected to `/login`

#### Scenario: Empty state display

- **WHEN** a newly registered user navigates to `/chat`
- **THEN** the empty state illustration and prompt text SHALL be visible
