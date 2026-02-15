# User Management

## Purpose

Handles user profile retrieval and BYOK (Bring Your Own Key) API key management with AES-256 encryption.

## Requirements

### Requirement: Get User Profile

The system SHALL return the authenticated user's profile via `GET /api/user/profile`.

#### Scenario: Successful profile retrieval

- **WHEN** an authenticated user requests their profile
- **THEN** the system SHALL return HTTP 200 with `id`, `email`, `hasApiKey` (bool), `createdAt`
- **AND** the endpoint SHALL require authentication

#### Scenario: Unauthenticated request

- **WHEN** an unauthenticated request is made to `GET /api/user/profile`
- **THEN** the system SHALL return HTTP 401 Unauthorized

### Requirement: Set API Key

The system SHALL allow users to store their API-Football key via `PUT /api/user/api-key`.

#### Scenario: Successful key storage

- **WHEN** an authenticated user submits a valid API key
- **THEN** the system SHALL validate the key against API-Football `GET /status` endpoint
- **AND** it SHALL encrypt the key with AES-256 via `IEncryptionService`
- **AND** it SHALL store the encrypted key on the User entity
- **AND** it SHALL return HTTP 200

#### Scenario: Invalid API key

- **WHEN** the submitted API key fails validation against API-Football `/status`
- **THEN** the system SHALL return HTTP 400 Bad Request
- **AND** the error SHALL use `UserErrors.InvalidApiKey`

#### Scenario: API key validation timeout

- **WHEN** the API-Football `/status` call times out or returns HTTP 5xx
- **THEN** the system SHALL return HTTP 502
- **AND** the error SHALL use `UserErrors.ApiValidationUnavailable`

#### Scenario: Unauthenticated request

- **WHEN** an unauthenticated request is made to `PUT /api/user/api-key`
- **THEN** the system SHALL return HTTP 401 Unauthorized

### Requirement: API Key Priority

The system SHALL prioritize the user's BYOK key over the system default key.

#### Scenario: User has BYOK key

- **WHEN** a football data request is made and the user has a stored API key
- **THEN** the system SHALL decrypt and use the user's key for API-Football requests

#### Scenario: User has no BYOK key

- **WHEN** a football data request is made and the user has no stored API key
- **THEN** the system SHALL use the system default API key from configuration

### Requirement: Encryption Service

The system SHALL provide `IEncryptionService` for AES-256 encryption and decryption.

#### Scenario: Encrypt API key

- **WHEN** an API key needs to be stored
- **THEN** `IEncryptionService.Encrypt()` SHALL encrypt the key using AES-256 with a random IV per encryption
- **AND** it SHALL prepend the IV to the ciphertext for storage
- **AND** the encryption key SHALL be read from `IConfiguration["Encryption:Key"]`

#### Scenario: Decrypt API key

- **WHEN** an API key needs to be used
- **THEN** `IEncryptionService.Decrypt()` SHALL decrypt the stored encrypted key

### Requirement: Feature Errors

The system SHALL define user management errors in `UserErrors.cs`.

#### Scenario: Error definitions

- **WHEN** user management errors are needed
- **THEN** `UserErrors` SHALL define static properties: `InvalidApiKey` (BadRequest), `UserNotFound` (NotFound), `ApiValidationUnavailable` (BadRequest)
