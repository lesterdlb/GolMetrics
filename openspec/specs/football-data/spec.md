# Football Data

## Purpose

Integrates Semantic Kernel with Google Gemini for natural language processing and API-Football v3 for football statistics retrieval.

## Requirements

### Requirement: Semantic Kernel Service

The system SHALL provide `SemanticKernelService` for processing user messages through AI.

#### Scenario: Kernel configuration

- **WHEN** `SemanticKernelService` is initialized
- **THEN** it SHALL build a `Kernel` with `AddGoogleAIGeminiChatCompletion()` using model name and API key from configuration
- **AND** it SHALL register `FootballPlugin` via `kernel.Plugins.Add(KernelPluginFactory.CreateFromType<FootballPlugin>())`
- **AND** it SHALL use `FunctionChoiceBehavior.Auto()` for automatic function calling

#### Scenario: Message processing

- **WHEN** a user message is received along with chat history
- **THEN** `SemanticKernelService` SHALL send the message and history to Gemini via `IChatCompletionService.GetChatMessageContentAsync()` with chat history
- **AND** it SHALL return the AI-generated response

#### Scenario: Function calling

- **WHEN** Google Gemini determines that football data is needed to answer the user's question
- **THEN** Semantic Kernel SHALL automatically invoke the appropriate `FootballPlugin` function
- **AND** the function result SHALL be fed back to Gemini for response generation

### Requirement: Football Plugin

The system SHALL provide `FootballPlugin` with 5 `[KernelFunction]`-decorated methods.

#### Scenario: GetTopScorers

- **WHEN** `GetTopScorers(int leagueId, int season)` is invoked
- **THEN** it SHALL call API-Football `GET /players/topscorers?league={leagueId}&season={season}` via `IFootballApiClient`
- **AND** it SHALL return formatted scorer data

#### Scenario: GetStandings

- **WHEN** `GetStandings(int leagueId, int season)` is invoked
- **THEN** it SHALL call API-Football `GET /standings?league={leagueId}&season={season}` via `IFootballApiClient`
- **AND** it SHALL return formatted standings data

#### Scenario: GetRecentResults

- **WHEN** `GetRecentResults(int teamId, int last)` is invoked
- **THEN** it SHALL call API-Football `GET /fixtures?team={teamId}&last={last}` via `IFootballApiClient`
- **AND** it SHALL return formatted match results

#### Scenario: GetUpcomingMatches

- **WHEN** `GetUpcomingMatches(int leagueId, int? teamId, string fromDate)` is invoked
- **THEN** it SHALL call API-Football `GET /fixtures?league={leagueId}&next=10` via `IFootballApiClient`
- **AND** if `teamId` is provided, it SHALL add `team={teamId}` to the query
- **AND** it SHALL return formatted upcoming match data

#### Scenario: GetTeamStatistics

- **WHEN** `GetTeamStatistics(int teamId, int leagueId, int season)` is invoked
- **THEN** it SHALL call API-Football `GET /teams/statistics?team={teamId}&league={leagueId}&season={season}` via `IFootballApiClient`
- **AND** it SHALL return formatted team statistics

### Requirement: Football API Client

The system SHALL provide `IFootballApiClient` as a typed HttpClient for API-Football v3.

#### Scenario: API key resolution

- **WHEN** a request to API-Football is made
- **THEN** `IFootballApiClient` SHALL resolve the API key by reading the current user's `EncryptedApiKey` via `ICurrentUserService` and decrypting it via `IEncryptionService`
- **AND** if the user has no stored key, it SHALL fall back to the system default key from configuration
- **AND** it SHALL set the `x-apisports-key` header

#### Scenario: Request execution

- **WHEN** a football data request is made
- **THEN** `IFootballApiClient` SHALL use `ICacheService` to check/store cached responses
- **AND** it SHALL set the base URL to `https://v3.football.api-sports.io`

### Requirement: Response Formatting

The system SHALL format football data as Markdown for chat display.

#### Scenario: Structured data

- **WHEN** football data contains tabular information (standings, scorers)
- **THEN** the response SHALL be formatted as Markdown tables

### Requirement: Error Handling

The system SHALL handle API-Football errors gracefully.

#### Scenario: Rate limit exceeded

- **WHEN** API-Football returns HTTP 429
- **THEN** the system SHALL return an error with `FootballErrors.RateLimitExceeded` (BadRequest)

#### Scenario: API unavailable

- **WHEN** API-Football is unreachable or returns HTTP 5xx
- **THEN** the system SHALL return an error with `FootballErrors.ApiUnavailable` (BadRequest)

#### Scenario: Invalid parameters

- **WHEN** an invalid league ID or team ID is provided
- **THEN** the system SHALL return an error with `FootballErrors.InvalidParameters` (BadRequest)

#### Scenario: Gemini API error

- **WHEN** the Gemini API returns an error or times out
- **THEN** the system SHALL return an error with `FootballErrors.AiServiceUnavailable` (BadRequest)

### Requirement: Feature Errors

The system SHALL define football data errors in `FootballErrors.cs`.

#### Scenario: Error definitions

- **WHEN** football data errors are needed
- **THEN** `FootballErrors` SHALL define static properties: `RateLimitExceeded` (BadRequest), `ApiUnavailable` (BadRequest), `InvalidParameters` (BadRequest), `AiServiceUnavailable` (BadRequest)
