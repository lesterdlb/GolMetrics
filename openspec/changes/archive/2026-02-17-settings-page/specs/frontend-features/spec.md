## MODIFIED Requirements

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
