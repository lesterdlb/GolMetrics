## Why

The football data infrastructure (`IFootballApiClient`, caching, API key delegation) is complete, but there is no AI layer to process natural language queries. Users cannot ask questions in plain English and get football statistics back. The `SemanticKernelService` implementation and `FootballPlugin` are the bridge between the chat interface and the data layer.

## What Changes

- Implement `SemanticKernelService` (the concrete `ISemanticKernelService`) that orchestrates Gemini chat completion with chat history and a system prompt
- Implement `FootballPlugin` with 5 `[KernelFunction]`-decorated methods that delegate to `IFootballApiClient`
- Update `DependencyInjection.AddSemanticKernel()` to register `FootballPlugin` in the kernel and `ISemanticKernelService` as a scoped service
- Add `FootballErrors.AiServiceUnavailable` error definition

## Capabilities

### New Capabilities

_None_ — this change implements within the existing `football-data` capability spec.

### Modified Capabilities

- `football-data`: Adding `SemanticKernelService` implementation, `FootballPlugin` class, updated DI registration, and `AiServiceUnavailable` error — all defined in the existing spec but not yet implemented.

## Impact

- **Code**: New files in `src/GolMetrics.API/Features/FootballData/` (`SemanticKernelService.cs`, `FootballPlugin.cs`); modifications to `DependencyInjection.cs` and `FootballErrors.cs`
- **Dependencies**: Uses already-referenced `Microsoft.SemanticKernel` and `Microsoft.SemanticKernel.Connectors.Google` packages
- **APIs**: No new HTTP endpoints — this change provides the internal service layer consumed by the Chat feature
- **Configuration**: Requires `Gemini:ApiKey` and `Gemini:ModelId` in configuration (already supported)
