## Context

The Settings page (`/settings`) is currently a stub. The backend already provides `GET /api/user/profile` and `PUT /api/user/api-key` endpoints. The frontend needs a fully implemented page following the same patterns as ChatPage (Background + Header layout, Zustand store, service module, toast notifications).

## Goals / Non-Goals

**Goals:**
- Implement a functional Settings page displaying profile info and API key management
- Follow established frontend patterns (Zustand store, service module, layout components)
- Provide clear feedback for API key submission (success/error states)
- Add navigation link to settings from the chat header

**Non-Goals:**
- Editing profile fields (email, password) - read-only display only
- Deleting/revoking API keys - only setting/updating
- Backend changes - existing endpoints are sufficient
- Settings beyond API key management (preferences, themes, notifications)

## Decisions

### 1. Dedicated settings store vs extending auth store

**Decision**: Create a dedicated `settings-store.ts` Zustand store.

**Rationale**: The auth store handles authentication state (token, login/logout). Profile data (creation date, hasApiKey) and API key submission state (loading, errors) are distinct concerns. Keeping them separate follows the existing pattern where each page has its own store (chat-store for ChatPage).

**Alternative**: Extending auth-store with profile fields. Rejected because it mixes authentication lifecycle with settings page state.

### 2. Page layout structure

**Decision**: Use the same Background + Header layout as ChatPage, with a centered card containing two sections (Profile Info, API Key).

**Rationale**: Consistent visual language across the app. The glass-panel card style used in auth pages and chat provides visual continuity.

### 3. API key input as password field with toggle

**Decision**: Use a password-type input with a show/hide toggle for the API key field.

**Rationale**: API keys are sensitive. Masking by default prevents shoulder-surfing while still allowing users to verify their input.

### 4. Navigation to settings

**Decision**: Add a settings icon button in the Header component, next to the existing logout button.

**Rationale**: The Header is visible on all authenticated pages. A gear icon is universally recognized for settings.

## Risks / Trade-offs

- **[Stale hasApiKey status]** The profile data fetched on mount could become stale if the user has multiple tabs open. Mitigation: Re-fetch profile after successful API key submission.
- **[No API key deletion]** Users cannot remove a previously set API key, only replace it. Mitigation: Acceptable for MVP; can be added later if needed.
- **[No optimistic updates]** API key submission waits for server confirmation before updating UI. Mitigation: This is intentional since the backend validates the key against API-Football before storing.
