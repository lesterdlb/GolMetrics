# Global Configuration

**Current date**: 2026-01-01

## Language Standards

**Language:** English only - all code, comments, docs, examples, commits, configs, errors, tests

## Tone and Behavior

- Criticism is welcome.
    - Please tell me when I am wrong or mistaken, or even when you think I might be wrong or mistaken.
    - Please tell me if there is a better approach than the one I am taking.
    - Please tell me if there is a relevant standard or convention that I appear to be unaware of.

- Point out potential issues with error handling, edge cases, and performance

- Be concise.
    - Short summaries are OK, but don't give an extended breakdown unless we are working through the details of a plan.
    - Do not flatter, and do not give compliments unless I am specifically asking for your judgement.
    - Occasional pleasantries are fine.

Flag any security concerns or data validation gaps

## Code Style

- Do not use comments to explain what the code is doing; instead, write self-explanatory code. Use comments only if the
  code itself is confusing or non-obvious.
- Use clear and descriptive names for variables, functions, classes, and modules
- Do not use emojis in code, comments, or documentation

## Project Structure

- **Backend API**: `src/GolMetrics.API/` (.NET 10 Minimal API)
- **Frontend**: `src/GolMetrics.Web/` (React 19 + Vite + TypeScript)
- **Tests**: `tests/GolMetrics.API.Tests/`
- **OpenSpec specs**: `openspec/specs/` (14 capability specs + config.yaml)
- **Features**: `src/GolMetrics.API/Features/<FeatureName>/` (vertical slices)
- **Core abstractions**: `src/GolMetrics.API/Core/` (Result, Entity, ISlice, exceptions)

## Architecture Rules

- **Vertical Slice Architecture**: Each feature is an `internal sealed class : ISlice` with nested `internal sealed` Command/Query/Validator/Handler classes
- **MediatR**: Dispatch all commands/queries via `ISender.Send()`; never call handlers directly
- **Result pattern**: Return `Result.Failure()` for business errors; convert to HTTP via `ToProblemDetails()` extension
- **ISlice**: All slices implement `ISlice.RegisterEndpoints()`; discovered automatically via `AddSlices()`
- **EntityConfiguration**: All EF entity configs inherit `EntityConfiguration<TEntity>` abstract base
- **EndpointNames**: Route constants live in `EndpointNames.cs` as nested static classes per feature
- **DependencyInjection**: All service registration via C# 14 extension methods on `WebApplicationBuilder` in `DependencyInjection.cs`
- **Feature errors**: Define as static properties on `<Feature>Errors.cs` (e.g., `FootballErrors.RateLimitExceeded`)
- **Exception handlers**: 4 handlers in priority order: Validation (400) -> Database (409) -> Json (400) -> Global (500)
- **Permissions**: Apply `RequirePermissions()` on all protected endpoints
- **DTOs**: All request/response DTOs are `public sealed record`

## Tech Stack

- .NET 10, C# 14, PostgreSQL 16, EF Core with snake_case naming
- MediatR + FluentValidation pipeline
- Semantic Kernel + Google Gemini (`gemini-2.0-flash`)
- ASP.NET Identity + JWT Bearer authentication
- AES-256 encryption for user API keys
- Serilog structured logging
- React 19, Vite, TypeScript (ES2022, strict)
- Tailwind CSS 4 + shadcn/ui
- Zustand for state, React Router for routing, Axios for HTTP

## OpenSpec Workflow

- Read specs before implementing any feature: `openspec/specs/<feature>/spec.md`
- Recommended reading order: `project-setup` -> `architecture` -> `data-model` -> then feature-specific specs
- Use `/opsx:apply` to implement tasks from an OpenSpec change

## Testing Conventions

- **Frameworks**: xUnit, FluentAssertions, Moq, Testcontainers (PostgreSQL)
- **Trait categories**: `[Trait("Category", "Unit")]`, `[Trait("Category", "Integration")]`
- **Run tests**: `dotnet test tests/GolMetrics.API.Tests/`
- **Naming**: `MethodName_StateUnderTest_ExpectedBehavior`

## Do NOT

- Use controllers (use Minimal API slices only)
- Throw exceptions for business logic failures (use `Result.Failure()`)
- Hardcode secrets in committed config files (use user secrets or environment variables)
- Make slices `public` (always `internal sealed`)
- Skip FluentValidation on MediatR requests
- Call handlers directly (always dispatch via `ISender.Send()`)
