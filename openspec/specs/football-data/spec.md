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

#### Scenario: Required packages

- **WHEN** the Semantic Kernel service is set up
- **THEN** the project SHALL reference `Microsoft.SemanticKernel` and `Microsoft.SemanticKernel.Connectors.Google`
- **AND** it SHALL suppress experimental warning `SKEXP0070` via `#pragma warning disable SKEXP0070`

#### Scenario: Kernel build pattern

- **WHEN** the kernel is constructed
- **THEN** the build sequence SHALL be: create `Kernel.CreateBuilder()` -> call `AddGoogleAIGeminiChatCompletion(modelId, apiKey)` -> call `Build()` -> call `kernel.Plugins.Add(KernelPluginFactory.CreateFromType<FootballPlugin>(serviceProvider))` passing the service provider for dependency injection

#### Scenario: Chat history management

- **WHEN** a user message is processed with existing conversation history
- **THEN** `SemanticKernelService` SHALL build a `ChatHistory` from persisted `Message` entities: messages with role `User` SHALL be added via `AddUserMessage()`, messages with role `Assistant` SHALL be added via `AddAssistantMessage()`
- **AND** the system prompt SHALL be set as the first message in the history

#### Scenario: Execution settings

- **WHEN** a chat completion request is made
- **THEN** it SHALL use `GeminiPromptExecutionSettings` with `FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()`

#### Scenario: System prompt

- **WHEN** the system prompt is configured
- **THEN** it SHALL instruct Gemini to act as a football statistics assistant, use the plugin functions to retrieve data, and format responses using Markdown tables when presenting tabular data

#### Scenario: Gemini configuration

- **WHEN** Gemini settings are configured
- **THEN** `appsettings.json` SHALL include a `Gemini` section with `ApiKey` (string) and `ModelId` (string, default `gemini-2.0-flash`)

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

### Requirement: API-Football Configuration

The system SHALL use API-Football v3 with the following configuration constraints.

#### Scenario: API authentication and rate limits

- **WHEN** communicating with API-Football
- **THEN** the base URL SHALL be `https://v3.football.api-sports.io`
- **AND** the authentication header SHALL be `x-apisports-key`
- **AND** the system SHALL monitor rate limit headers: `x-ratelimit-requests-limit` and `x-ratelimit-requests-remaining`
- **AND** the free tier limit is 100 requests/day

#### Scenario: Response envelope structure

- **WHEN** a response is received from API-Football
- **THEN** it SHALL follow the envelope structure: `{ "get", "parameters", "errors", "results", "paging", "response" }`
- **AND** the actual data SHALL be in the `response` array

#### Scenario: League and season conventions

- **WHEN** league or season parameters are used
- **THEN** the season format SHALL be a 4-digit year (e.g., `2024`)
- **AND** common league IDs SHALL be: Premier League=39, La Liga=140, Serie A=135, Bundesliga=78, Ligue 1=61

### Requirement: API-Football Endpoint Details

The system SHALL call specific API-Football endpoints with defined response field paths.

#### Scenario: Top scorers endpoint

- **WHEN** `GetTopScorers(int leagueId, int season)` is invoked
- **THEN** it SHALL call `GET /players/topscorers?league={leagueId}&season={season}`
- **AND** each item in `response[]` SHALL contain `player.name`, `player.nationality`, `statistics[0].team.name`, `statistics[0].goals.total`, `statistics[0].games.appearences`

#### Scenario: Standings endpoint

- **WHEN** `GetStandings(int leagueId, int season)` is invoked
- **THEN** it SHALL call `GET /standings?league={leagueId}&season={season}`
- **AND** the standings array SHALL be at `response[0].league.standings[0][]`
- **AND** each entry SHALL contain `rank`, `team.name`, `points`, `all.played`, `all.win`, `all.draw`, `all.lose`, `all.goals.for`, `all.goals.against`, `goalsDiff`

#### Scenario: Recent results endpoint

- **WHEN** `GetRecentResults(int teamId, int last)` is invoked
- **THEN** it SHALL call `GET /fixtures?team={teamId}&last={last}`
- **AND** each item in `response[]` SHALL contain `fixture.date`, `teams.home.name`, `teams.away.name`, `goals.home`, `goals.away`, `fixture.status.short`

#### Scenario: Upcoming matches endpoint

- **WHEN** `GetUpcomingMatches(int leagueId, int? teamId, string fromDate)` is invoked
- **THEN** it SHALL call `GET /fixtures?league={leagueId}&next=10`
- **AND** if `teamId` is provided, it SHALL append `&team={teamId}`
- **AND** each item in `response[]` SHALL contain `fixture.date`, `fixture.venue.name`, `teams.home.name`, `teams.away.name`, `league.name`

#### Scenario: Team statistics endpoint

- **WHEN** `GetTeamStatistics(int teamId, int leagueId, int season)` is invoked
- **THEN** it SHALL call `GET /teams/statistics?team={teamId}&league={leagueId}&season={season}`
- **AND** `response` SHALL contain `team.name`, `fixtures.played.total`, `fixtures.wins.total`, `fixtures.draws.total`, `fixtures.loses.total`, `goals.for.total.total`, `goals.against.total.total`, `clean_sheet.total`, `form`

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
