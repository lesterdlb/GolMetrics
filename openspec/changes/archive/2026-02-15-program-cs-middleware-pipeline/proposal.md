## Why

TICK-003: The current `Program.cs` has the basic structure but needs to be the definitive, production-ready entry point that wires all services (API, database, auth, error handling, CORS, Semantic Kernel, encryption) and configures the middleware pipeline in the correct order per the project-setup and architecture specs.

## What Changes

- Replace the current `Program.cs` with the complete service registration and middleware pipeline
- Add `AddSemanticKernel()` extension method in `DependencyInjection.cs` to register Semantic Kernel with Google Gemini connector
- Add `AddEncryptionServices()` extension method in `DependencyInjection.cs` to register AES-256 encryption for user API keys
- Add `AddFootballServices()` extension method in `DependencyInjection.cs` to register the API-Football HTTP client
- Wire Serilog as the logging provider with structured logging configuration
- Ensure middleware pipeline order matches spec: `UseExceptionHandler()` -> `UseSerilogRequestLogging()` -> `UseCors()` -> `UseAuthentication()` -> `UseAuthorization()` -> `MapSliceEndpoints()` -> `MapScalarApiReference()` (dev only)
- Auto-apply EF Core migrations in development only
- Configure `appsettings.json` sections per spec: `ConnectionStrings`, `TokenOptions`, `Encryption`, `ApiFootball`, `Gemini`

## Capabilities

### New Capabilities

(none - this change implements existing capabilities)

### Modified Capabilities

- `project-setup`: Adding Semantic Kernel, encryption, and football service registration to `DependencyInjection.cs`; adding `UseSerilogRequestLogging()` to middleware pipeline
- `architecture`: No requirement changes, implementation alignment only

## Impact

- `src/GolMetrics.API/Program.cs` - replaced with complete pipeline
- `src/GolMetrics.API/DependencyInjection.cs` - new extension methods added
- `src/GolMetrics.API/appsettings.json` - configuration sections added/verified
- `src/GolMetrics.API/appsettings.Development.json` - development-specific values
- NuGet packages: `Microsoft.SemanticKernel`, `Microsoft.SemanticKernel.Connectors.Google` (if not already installed)
