## Why

TICK-001: The project has only a basic `Program.cs` scaffold. All core abstractions defined in the `project-setup` and `architecture` specs (Result pattern, Entity base, ISlice interface, exception handlers, ValidationBehavior, CurrentUserService, EndpointNames, DependencyInjection) must be implemented before any feature work can begin.

## What Changes

- Add `Result`, `Result<T>`, `Error`, and `ErrorCategory` types with `ToProblemDetails()` extension
- Add `Entity` base class with audit fields and optimistic concurrency
- Add `EntityConfiguration<TEntity>` abstract EF Core configuration base
- Add `ISlice` interface with assembly scanning and endpoint registration
- Add `ValidationBehavior<TRequest, TResponse>` MediatR pipeline behavior
- Add 4 exception handlers: Validation (400), Database (409), Json (400), Global (500)
- Add `ICurrentUserService` / `CurrentUserService` for JWT claim resolution
- Add `EndpointNames` static class with nested route constants structure
- Add `DependencyInjection.cs` with extension methods: `AddApiServices()`, `AddDatabase()`, `AddAuthenticationServices()`, `AddErrorHandling()`, `AddCors()`
- Add `Permissions` static class with nested resource classes and `RequirePermissions()` extension
- Refactor `Program.cs` to use the new DI extensions and middleware pipeline

## Capabilities

### New Capabilities

_None_ -- this change implements existing capability specs, it does not introduce new capabilities.

### Modified Capabilities

_None_ -- the `project-setup` and `architecture` specs already define these abstractions. This change is pure implementation.

## Impact

- **Code**: New files under `src/GolMetrics.API/Core/`, updated `Program.cs`
- **Dependencies**: NuGet packages required: MediatR, FluentValidation.DependencyInjectionExtensions, Serilog.AspNetCore, Microsoft.AspNetCore.Identity.EntityFrameworkCore, Microsoft.AspNetCore.Authentication.JwtBearer, Npgsql.EntityFrameworkCore.PostgreSQL, EFCore.NamingConventions, Scalar.AspNetCore
- **APIs**: No public API changes (foundation only)
- **Systems**: PostgreSQL connection string required in configuration
