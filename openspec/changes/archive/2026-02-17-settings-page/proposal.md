## Why

TICK-016: The Settings page (`/settings`) is currently a stub placeholder. Users need a functional screen to view their profile information and manage their API-Football API key (BYOK). Without this, there is no way for users to provide their own API key through the UI, which limits the BYOK feature to API-only usage.

## What Changes

- Implement a Settings page frontend at `/settings` with two sections: profile display and API key management
- Create a settings Zustand store for managing profile state and API key submission
- Create a settings service module for `GET /api/user/profile` and `PUT /api/user/api-key` calls
- Add navigation from the chat sidebar to the settings page
- Display user email, account creation date, and API key status (has key / no key)
- Provide an input form for submitting an API-Football API key with validation feedback
- Show success/error states using toast notifications consistent with existing patterns

## Capabilities

### New Capabilities

(none - all capabilities are covered by existing specs)

### Modified Capabilities

- `frontend-features`: Adding detailed requirements for the Settings screen implementation (store, service, navigation, form states)

## Impact

- **Frontend only**: All changes are in `src/GolMetrics.Web/`
- **New files**: settings service, settings store, SettingsPage components
- **Modified files**: SettingsPage.tsx (replace stub), sidebar/header navigation
- **Backend**: No changes needed - `GET /api/user/profile` and `PUT /api/user/api-key` already exist
- **Dependencies**: No new packages required - uses existing shadcn/ui components, Zustand, Axios, sonner
