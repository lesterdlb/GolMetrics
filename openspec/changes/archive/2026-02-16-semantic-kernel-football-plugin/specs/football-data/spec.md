## MODIFIED Requirements

### Requirement: Feature Errors

The system SHALL define football data errors in `FootballErrors.cs`.

#### Scenario: Error definitions

- **WHEN** football data errors are needed
- **THEN** `FootballErrors` SHALL define static properties:
- **AND** `RateLimitExceeded` SHALL be `Error("Football.RateLimitExceeded", "API-Football rate limit exceeded. Please try again later.", ErrorCategory.BadRequest)`
- **AND** `ApiUnavailable` SHALL be `Error("Football.ApiUnavailable", "API-Football service is currently unavailable.", ErrorCategory.BadRequest)`
- **AND** `InvalidParameters` SHALL be `Error("Football.InvalidParameters", "The provided parameters returned an error from API-Football.", ErrorCategory.BadRequest)`
- **AND** `AiServiceUnavailable` SHALL be `Error("Football.AiServiceUnavailable", "The AI service is currently unavailable.", ErrorCategory.BadRequest)`
