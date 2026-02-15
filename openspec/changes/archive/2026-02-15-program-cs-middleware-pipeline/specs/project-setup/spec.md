## MODIFIED Requirements

### Requirement: Program.cs Configuration

The application SHALL configure services and middleware in a specific order.

#### Scenario: Service registration order

- **WHEN** `Program.cs` registers services
- **THEN** it SHALL call builder extension methods in this order: `AddApiServices()` (MediatR, FluentValidation, Swagger, Serilog) -> `AddDatabase()` (DbContext, EF Core) -> `AddAuthenticationServices()` (Identity, JWT) -> `AddSemanticKernel()` (Kernel, Gemini) -> `AddEncryptionServices()` (AES-256) -> `AddFootballServices()` (HttpClient for API-Football) -> `AddErrorHandling()` (exception handlers) -> `AddCors()`

#### Scenario: Middleware pipeline order

- **WHEN** the middleware pipeline is built
- **THEN** it SHALL apply middleware in this order: `UseExceptionHandler()` -> `UseSerilogRequestLogging()` -> `UseCors()` -> `UseAuthentication()` -> `UseAuthorization()` -> `MapSliceEndpoints()` -> `MapScalarApiReference()` (development only)

### Requirement: DependencyInjection

The system SHALL register all dependencies via C# 14 extension methods on `WebApplicationBuilder`.

#### Scenario: Extension method structure

- **WHEN** dependencies are registered
- **THEN** `DependencyInjection.cs` SHALL define C# 14 `extension(WebApplicationBuilder)` methods: `AddApiServices()` (registers MediatR, FluentValidation, slices, Swagger, Serilog), `AddDatabase()` (registers DbContext with PostgreSQL and snake_case naming), `AddAuthenticationServices()` (registers Identity, JWT Bearer), `AddSemanticKernel()` (registers Semantic Kernel with Google Gemini connector using `Gemini:ModelId` and `Gemini:ApiKey` from configuration), `AddEncryptionServices()` (registers `IEncryptionService` as singleton), `AddFootballServices()` (registers `IFootballApiClient` as typed HttpClient with base address from `ApiFootball:BaseUrl`), `AddErrorHandling()` (registers exception handlers in priority order), `AddCors()` (registers CORS policy)

### Requirement: NuGet Packages

The solution SHALL install packages grouped by concern.

#### Scenario: AI and Semantic Kernel packages

- **WHEN** AI chat processing is configured
- **THEN** it SHALL install: `Microsoft.SemanticKernel`, `Microsoft.SemanticKernel.Connectors.Google`
- **AND** `DependencyInjection.cs` SHALL suppress experimental warning `SKEXP0070` via `#pragma warning disable SKEXP0070`
