## Context

The data layer (`IFootballApiClient`, `ICacheService`, `ApiKeyDelegatingHandler`) and the `ISemanticKernelService` interface are already in place. The `Kernel` singleton is registered but lacks plugin registration and the service implementation. This change bridges the gap so the Chat feature can invoke AI-powered football queries.

## Goals / Non-Goals

**Goals:**
- Implement `SemanticKernelService` that processes user messages through Gemini with chat history
- Implement `FootballPlugin` with 5 kernel functions delegating to `IFootballApiClient`
- Register the plugin in the kernel and the service in DI
- Add the missing `AiServiceUnavailable` error

**Non-Goals:**
- Chat endpoints (separate change — consumes `ISemanticKernelService`)
- Streaming responses or token-by-token output
- Custom prompt tuning or prompt management UI
- Rate limiting at the AI layer (handled at the API-Football layer)

## Decisions

### 1. SemanticKernelService as scoped service

Register `ISemanticKernelService` → `SemanticKernelService` as **scoped** (not singleton). The service needs per-request access to `IFootballApiClient` (which is scoped via its `HttpClient`). The `Kernel` itself remains singleton since it holds the Gemini connector config and plugin definitions.

**Alternative**: Singleton service with `IServiceScopeFactory` — adds complexity for no benefit since chat handlers are already scoped.

### 2. FootballPlugin uses constructor DI

`FootballPlugin` takes `IFootballApiClient` via constructor injection. Semantic Kernel resolves it from the service provider passed to `KernelPluginFactory.CreateFromType<FootballPlugin>(serviceProvider)`. This means the plugin is instantiated per-kernel-call, getting a fresh scoped `IFootballApiClient`.

**Alternative**: Inject `IServiceProvider` and resolve manually — violates DI best practices and makes testing harder.

### 3. Kernel registration change

The current `AddSemanticKernel()` registers `Kernel` as a singleton with no plugins. The updated registration must:
1. Keep `Kernel` as singleton for the Gemini connector
2. Add `FootballPlugin` via `KernelPluginFactory.CreateFromType<FootballPlugin>(serviceProvider)` using the root service provider
3. Register `ISemanticKernelService` as scoped

Since `FootballPlugin` resolves scoped services (`IFootballApiClient`), and the plugin is added to a singleton kernel, the plugin factory must receive the service provider so Semantic Kernel can create new instances per invocation.

### 4. Error handling strategy

`FootballPlugin` methods return `string`. On `Result.Failure` from `IFootballApiClient`, the plugin returns a descriptive error string (not throwing) so Gemini can relay the error naturally to the user. `SemanticKernelService.ProcessMessageAsync` wraps the entire Gemini call in try/catch — on failure, it throws so the caller (Chat handler) can map to `ChatErrors.AiProcessingFailed`.

### 5. System prompt

A static system prompt instructs Gemini to act as a football statistics assistant, use plugin functions for data retrieval, and format tabular data as Markdown tables. Defined as a `const string` in `SemanticKernelService`.

## Risks / Trade-offs

- **Singleton kernel with scoped plugin dependencies** → Semantic Kernel handles this via `KernelPluginFactory.CreateFromType` with a service provider, which creates plugin instances per invocation. If this breaks, fallback is to create a kernel-per-request (at cost of memory).
- **Gemini experimental connector** → `SKEXP0070` suppression required. If the API changes between SK versions, the connector calls may break. Mitigated by pinning `Microsoft.SemanticKernel` version.
- **No retry on Gemini failure** → If Gemini times out or errors, a single attempt fails. Acceptable for MVP; retry with exponential backoff can be added later.
