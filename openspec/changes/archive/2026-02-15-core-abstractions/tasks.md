## 1. NuGet Packages

- [x] 1.1 Install core infrastructure packages: `dotnet add src/GolMetrics.API/ package MediatR`, `FluentValidation.DependencyInjectionExtensions`, `Microsoft.EntityFrameworkCore.Design`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `EFCore.NamingConventions`, `Serilog.AspNetCore`
- [x] 1.2 Install authentication packages: `dotnet add src/GolMetrics.API/ package Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.AspNetCore.Authentication.JwtBearer`
- [x] 1.3 Add `[assembly: InternalsVisibleTo("GolMetrics.API.Tests")]` to the API project (in `Properties/AssemblyInfo.cs` or a `GlobalUsings.cs`)
- [x] 1.4 Verify: `dotnet build src/GolMetrics.API/`

## 2. Result Pattern (`Core/Results/`)

- [x] 2.1 Create `src/GolMetrics.API/Core/Results/ErrorCategory.cs` -- enum with `BadRequest`, `Unauthorized`, `Forbidden`, `NotFound`, `Conflict`
- [x] 2.2 Create `src/GolMetrics.API/Core/Results/Error.cs` -- `public sealed record Error(string Code, string Message, ErrorCategory Category)`
- [x] 2.3 Create `src/GolMetrics.API/Core/Results/Result.cs` -- `Result` (non-generic) with `IsSuccess`, `Error`, static `Success()`, static `Failure(Error)`; `Result<T>` with `Value` and implicit conversion from `T`
- [x] 2.4 Create `src/GolMetrics.API/Core/Results/ResultExtensions.cs` -- C# 14 `extension(Result)` with `ToProblemDetails()` mapping `ErrorCategory` to HTTP status codes (400, 401, 403, 404, 409)
- [x] 2.5 Verify: `dotnet build src/GolMetrics.API/`

## 3. Entity Base and EF Configuration (`Core/Abstractions/`, `Core/Persistence/`)

- [x] 3.1 Create `src/GolMetrics.API/Core/Abstractions/Entity.cs` -- abstract class with `Id` (Guid), `CreatedBy` (Guid), `LastModifiedBy` (Guid?), `CreatedAtUtc` (DateTime), `UpdatedAtUtc` (DateTime?), `Version` (uint)
- [x] 3.2 Create `src/GolMetrics.API/Core/Persistence/EntityConfiguration.cs` -- `abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>` configuring PK on `Id`, audit fields, `IsRowVersion()` on `Version`
- [x] 3.3 Create `src/GolMetrics.API/Core/Persistence/GolMetricsDbContext.cs` -- inherits `IdentityDbContext<User, IdentityRole<Guid>, Guid>`, configures `UseSnakeCaseNamingConvention()`, applies configurations from assembly, overrides `SaveChangesAsync` to set audit fields
- [x] 3.4 Verify: `dotnet build src/GolMetrics.API/`

## 4. ISlice Infrastructure (`Core/Abstractions/`)

- [x] 4.1 Create `src/GolMetrics.API/Core/Abstractions/ISlice.cs` -- interface with `void RegisterEndpoints(IEndpointRouteBuilder routes)`
- [x] 4.2 Create `src/GolMetrics.API/Core/Extensions/ServiceCollectionExtensions.cs` -- C# 14 `extension(IServiceCollection)` with `AddSlices()` that scans assembly for `ISlice` implementations and registers as transient
- [x] 4.3 Create `src/GolMetrics.API/Core/Extensions/EndpointRouteBuilderExtensions.cs` -- C# 14 `extension(IEndpointRouteBuilder)` with `MapSliceEndpoints()` that resolves all `ISlice` and calls `RegisterEndpoints()`
- [x] 4.4 Verify: `dotnet build src/GolMetrics.API/`

## 5. ValidationBehavior (`Core/Behaviors/`)

- [x] 5.1 Create `src/GolMetrics.API/Core/Behaviors/ValidationBehavior.cs` -- `internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>` that resolves `IEnumerable<IValidator<TRequest>>`, validates, throws `ValidationException` on failure, passes through if no validators
- [x] 5.2 Verify: `dotnet build src/GolMetrics.API/`

## 6. Exception Handlers (`Core/Exceptions/`)

- [x] 6.1 Create `src/GolMetrics.API/Core/Exceptions/ValidationExceptionHandler.cs` -- `IExceptionHandler` returning HTTP 400 ProblemDetails with field-level validation errors
- [x] 6.2 Create `src/GolMetrics.API/Core/Exceptions/DatabaseExceptionHandler.cs` -- `IExceptionHandler` returning HTTP 409 ProblemDetails for `DbUpdateException`/`DbUpdateConcurrencyException`
- [x] 6.3 Create `src/GolMetrics.API/Core/Exceptions/JsonExceptionHandler.cs` -- `IExceptionHandler` returning HTTP 400 ProblemDetails for `JsonException`
- [x] 6.4 Create `src/GolMetrics.API/Core/Exceptions/GlobalExceptionHandler.cs` -- `IExceptionHandler` returning HTTP 500 ProblemDetails with Serilog logging
- [x] 6.5 Verify: `dotnet build src/GolMetrics.API/`

## 7. CurrentUserService (`Core/Identity/`)

- [x] 7.1 Create `src/GolMetrics.API/Core/Abstractions/ICurrentUserService.cs` -- interface with `UserId` (Guid), `Email` (string), `IsAuthenticated` (bool)
- [x] 7.2 Create `src/GolMetrics.API/Core/Identity/CurrentUserService.cs` -- `internal sealed class` implementing `ICurrentUserService`, resolving from JWT claims via `IHttpContextAccessor`
- [x] 7.3 Verify: `dotnet build src/GolMetrics.API/`

## 8. Authorization (`Core/Authorization/`)

- [x] 8.1 Create `src/GolMetrics.API/Core/Authorization/Permissions.cs` -- static class with nested `Conversations` and `Users` classes, constants in `resource:action` format
- [x] 8.2 Create `src/GolMetrics.API/Core/Authorization/PermissionRequirement.cs` -- `IAuthorizationRequirement` implementation
- [x] 8.3 Create `src/GolMetrics.API/Core/Authorization/PermissionAuthorizationHandler.cs` -- `AuthorizationHandler<PermissionRequirement>` checking JWT `permissions` claim
- [x] 8.4 Create `src/GolMetrics.API/Core/Authorization/EndpointExtensions.cs` -- C# 14 `extension(RouteHandlerBuilder)` with `RequirePermissions(params string[] permissions)` calling `RequireAuthorization`
- [x] 8.5 Verify: `dotnet build src/GolMetrics.API/`

## 9. EndpointNames and DependencyInjection

- [x] 9.1 Create `src/GolMetrics.API/EndpointNames.cs` -- static class with nested static classes (`Auth`, `Chat`, `Football`) each containing `Names` and `Routes` string constants
- [x] 9.2 Create `src/GolMetrics.API/DependencyInjection.cs` -- C# 14 `extension(WebApplicationBuilder)` with `AddApiServices()`, `AddDatabase()`, `AddAuthenticationServices()`, `AddErrorHandling()`, `AddCors()`
- [x] 9.3 Verify: `dotnet build src/GolMetrics.API/`

## 10. Configuration and Program.cs

- [x] 10.1 Update `src/GolMetrics.API/appsettings.json` with sections: `ConnectionStrings.DefaultConnection`, `TokenOptions` (SecretKey, Issuer, Audience, ExpirationMinutes), `Encryption.Key`, `ApiFootball` (BaseUrl, ApiKey), `Gemini` (ApiKey, ModelId) -- placeholder values only
- [x] 10.2 Refactor `src/GolMetrics.API/Program.cs` -- use DI extension methods (`AddApiServices()`, `AddDatabase()`, `AddAuthenticationServices()`, `AddErrorHandling()`, `AddCors()`) and middleware pipeline (`UseExceptionHandler()`, `UseCors()`, `UseAuthentication()`, `UseAuthorization()`, `MapSliceEndpoints()`, `MapScalarApiReference()`)
- [x] 10.3 Verify: `dotnet build src/GolMetrics.API/`

## 11. User Entity

- [x] 11.1 Create `src/GolMetrics.API/Features/UserManagement/User.cs` -- `public sealed class User : IdentityUser<Guid>` adding any extra fields required by the data-model spec (e.g., encrypted API key storage)
- [x] 11.2 Verify: `dotnet build src/GolMetrics.API/`

## 12. Final Verification

- [x] 12.1 Run full build: `dotnet build src/GolMetrics.API/`
- [x] 12.2 Verify no warnings related to core abstractions
