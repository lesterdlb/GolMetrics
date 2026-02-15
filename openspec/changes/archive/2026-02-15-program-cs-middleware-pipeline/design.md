## Context

The current `Program.cs` and `DependencyInjection.cs` wire core services (MediatR, FluentValidation, EF Core, Identity/JWT, exception handlers, CORS) and configure the middleware pipeline. However, three service groups are not yet registered:

1. **Semantic Kernel + Google Gemini** - needed for AI chat processing (packages not yet in csproj)
2. **Encryption services** - `IEncryptionService` for AES-256 BYOK key management
3. **Football API client** - typed `HttpClient` for API-Football v3

The middleware pipeline is also missing `UseSerilogRequestLogging()`.

## Goals / Non-Goals

**Goals:**

- Wire all remaining service registrations into `DependencyInjection.cs`
- Add missing NuGet packages (`Microsoft.SemanticKernel`, `Microsoft.SemanticKernel.Connectors.Google`)
- Add `UseSerilogRequestLogging()` to the middleware pipeline
- Ensure `Program.cs` matches the spec-defined service registration and middleware order exactly
- Verify `appsettings.json` has all required configuration sections

**Non-Goals:**

- Implementing the actual service classes (`SemanticKernelService`, `EncryptionService`, `FootballApiClient`) - those belong to their respective feature tickets
- Changing existing service registrations that already work correctly
- Frontend changes

## Decisions

### 1. New extension methods in DependencyInjection.cs

Add three new extension methods to the existing C# 14 `extension(WebApplicationBuilder)` block:

- `AddSemanticKernel()` - registers Semantic Kernel with Google Gemini connector, reads `Gemini:ModelId` and `Gemini:ApiKey` from configuration. Suppresses `SKEXP0070` warning at file level.
- `AddEncryptionServices()` - registers `IEncryptionService` as singleton (stateless, uses config key)
- `AddFootballServices()` - registers `IFootballApiClient` as a typed HttpClient with base address from `ApiFootball:BaseUrl`

**Rationale:** Keeps the pattern consistent with existing extension methods. Each concern gets its own method for clarity.

### 2. Service registration order in Program.cs

```
AddApiServices() -> AddDatabase() -> AddAuthenticationServices() ->
AddSemanticKernel() -> AddEncryptionServices() -> AddFootballServices() ->
AddErrorHandling() -> AddCors()
```

**Rationale:** New services are registered after auth (they depend on `ICurrentUserService`) and before error handling (which should be the last service registered). This maintains the spec pattern where infrastructure comes first, then features.

### 3. Middleware pipeline order

```
UseExceptionHandler() -> UseSerilogRequestLogging() -> UseCors("AllowAll") ->
UseAuthentication() -> UseAuthorization() -> MapSliceEndpoints() ->
MapOpenApi() + MapScalarApiReference() (dev only)
```

**Rationale:** `UseSerilogRequestLogging()` goes after exception handler so errors are still logged, but before CORS/auth so request timing includes the full pipeline. Matches Serilog best practices.

### 4. NuGet package additions

Add to `.csproj`:
- `Microsoft.SemanticKernel`
- `Microsoft.SemanticKernel.Connectors.Google`

**Rationale:** Required by the football-data spec for Gemini integration.

## Risks / Trade-offs

- [Semantic Kernel packages are marked experimental] -> Suppress `SKEXP0070` warning at file level in `DependencyInjection.cs`. Pin package versions.
- [Service implementations don't exist yet] -> Registration methods will reference interfaces/classes from upcoming feature tickets. These extension methods must be added when those types are implemented, not before. Only the `Program.cs` pipeline ordering and NuGet packages can be done now.
- [Configuration values are placeholders] -> `appsettings.json` already has all sections with `CHANGE_ME` placeholders. No changes needed to config files.
