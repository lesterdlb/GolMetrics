## 1. Error Definition

- [x] 1.1 Add `AiServiceUnavailable` static property to `FootballErrors.cs`

## 2. FootballPlugin

- [x] 2.1 Create `FootballPlugin.cs` in `Features/FootballData/` with constructor injecting `IFootballApiClient`
- [x] 2.2 Implement `GetTopScorers(int leagueId, int season)` with `[KernelFunction]` and `[Description]` attributes
- [x] 2.3 Implement `GetStandings(int leagueId, int season)` with `[KernelFunction]` and `[Description]` attributes
- [x] 2.4 Implement `GetRecentResults(int teamId, int last)` with `[KernelFunction]` and `[Description]` attributes
- [x] 2.5 Implement `GetUpcomingMatches(int leagueId, int? teamId, string fromDate)` with `[KernelFunction]` and `[Description]` attributes
- [x] 2.6 Implement `GetTeamStatistics(int teamId, int leagueId, int season)` with `[KernelFunction]` and `[Description]` attributes

## 3. SemanticKernelService

- [x] 3.1 Create `SemanticKernelService.cs` in `Features/FootballData/` implementing `ISemanticKernelService`
- [x] 3.2 Define system prompt constant instructing Gemini to act as a football statistics assistant with Markdown table formatting
- [x] 3.3 Implement `ProcessMessageAsync` — build `ChatHistory` from persisted messages, set system prompt, call `IChatCompletionService.GetChatMessageContentAsync` with `GeminiPromptExecutionSettings` and `FunctionChoiceBehavior.Auto()`
- [x] 3.4 Add `#pragma warning disable SKEXP0070` for experimental Google connector usage

## 4. Dependency Injection

- [x] 4.1 Update `AddSemanticKernel()` to register `FootballPlugin` in the kernel via `KernelPluginFactory.CreateFromType<FootballPlugin>(serviceProvider)`
- [x] 4.2 Register `ISemanticKernelService` as scoped `SemanticKernelService` in `AddSemanticKernel()`

## 5. Testing

- [x] 5.1 Write unit tests for `FootballPlugin` — verify each method delegates to `IFootballApiClient` and handles `Result.Failure` by returning error description
- [x] 5.2 Write unit tests for `SemanticKernelService` — verify chat history construction and Gemini invocation
- [x] 5.3 Verify the application builds without errors
