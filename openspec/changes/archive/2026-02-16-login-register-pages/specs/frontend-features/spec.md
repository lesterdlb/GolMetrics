## MODIFIED Requirements

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
