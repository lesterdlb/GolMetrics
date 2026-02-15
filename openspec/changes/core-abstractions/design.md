## Context

The GolMetrics backend has only a bare `Program.cs` with a hardcoded `/api/chat` endpoint and basic CORS/OpenAPI configuration. The `project-setup` and `architecture` specs define a comprehensive set of core abstractions that every feature slice depends on. No feature work can begin until these foundations exist.

## Goals / Non-Goals

**Goals:**

- Implement all core abstractions defined in `project-setup/spec.md` and `architecture/spec.md`
- Establish the vertical slice infrastructure so feature slices can be added incrementally
- Set up the MediatR pipeline with validation behavior
- Configure EF Core with PostgreSQL, snake_case naming, and enum-as-string conversion
- Wire up ASP.NET Identity with JWT Bearer authentication
- Implement the 4-tier exception handling pipeline
- Refactor `Program.cs` to use DI extension methods and the correct middleware order

**Non-Goals:**

- Implementing any feature slices (Chat, Football, User Management)
- Creating database migrations (no domain entities yet beyond User)
- Frontend changes
- CI/CD pipeline setup
- Deploying to any environment

## Decisions

### D1: File organization under `Core/`

Place shared abstractions in `src/GolMetrics.API/Core/` with logical grouping:

- `Core/Abstractions/` -- `Entity.cs`, `ISlice.cs`, `ICurrentUserService.cs`
- `Core/Results/` -- `Result.cs`, `Error.cs`, `ErrorCategory.cs`, `ResultExtensions.cs`
- `Core/Behaviors/` -- `ValidationBehavior.cs`
- `Core/Exceptions/` -- `ValidationExceptionHandler.cs`, `DatabaseExceptionHandler.cs`, `JsonExceptionHandler.cs`, `GlobalExceptionHandler.cs`
- `Core/Persistence/` -- `ApplicationDbContext.cs`, `EntityConfiguration.cs`
- `Core/Identity/` -- `CurrentUserService.cs`
- `Core/Authorization/` -- `Permissions.cs`, `PermissionAuthorizationHandler.cs`, `PermissionRequirement.cs`, `EndpointExtensions.cs`

**Rationale**: Flat `Core/` would become crowded. Subdirectories group by concern while keeping paths short.

### D2: `EndpointNames.cs` placement

Place `EndpointNames.cs` at `src/GolMetrics.API/EndpointNames.cs` (project root level, not under `Core/`).

**Rationale**: Feature slices reference it directly for route constants. It's a project-wide concern, not a core abstraction.

### D3: `DependencyInjection.cs` placement

Place at `src/GolMetrics.API/DependencyInjection.cs` (project root level).

**Rationale**: Same reasoning as EndpointNames -- it orchestrates the entire application, not just core types.

### D4: Service registration order

Follow the spec-defined order: MediatR -> FluentValidation -> Slices -> Swagger -> Serilog -> Database -> Identity -> JWT -> Error Handlers -> CORS.

### D5: NuGet packages

| Package                                             | Purpose                                        |
| --------------------------------------------------- | ---------------------------------------------- |
| `MediatR`                                           | Command/query dispatching + pipeline behaviors |
| `FluentValidation.DependencyInjectionExtensions`    | Validator auto-registration                    |
| `Serilog.AspNetCore`                                | Structured logging                             |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Identity with EF store                         |
| `Microsoft.AspNetCore.Authentication.JwtBearer`     | JWT Bearer auth                                |
| `Npgsql.EntityFrameworkCore.PostgreSQL`             | PostgreSQL EF provider                         |
| `EFCore.NamingConventions`                          | snake_case naming convention                   |
| `Scalar.AspNetCore`                                 | API reference UI                               |

## Risks / Trade-offs

- **[Identity schema coupling]** ASP.NET Identity creates its own tables. -> Mitigated by configuring Identity in `ApplicationDbContext` with `OnModelCreating` and snake_case naming applied globally.
- **[JWT secret management]** JWT signing key must not be hardcoded. -> Use `dotnet user-secrets` for local development; environment variables in production.
- **[No domain entities yet]** `ApplicationDbContext` will be created with only Identity entities initially. -> Feature slices will add their own entity configurations incrementally.
- **[Exception handler ordering]** Registering handlers in wrong order could mask specific errors. -> Follow spec priority: Validation -> Database -> Json -> Global. Enforced in `AddErrorHandling()`.
