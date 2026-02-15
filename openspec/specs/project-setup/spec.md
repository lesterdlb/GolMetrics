# Project Setup

## Purpose

Defines the solution bootstrapping patterns: project creation, NuGet packages, service configuration, shared infrastructure (Result pattern, ISlice, entity base classes, exception handlers), and dependency injection. All extension methods use C# 14 `extension(Type)` syntax.

## Requirements

### Requirement: Solution and Project Creation

The solution SHALL be created with an API project and a test project.

#### Scenario: API project creation

- **WHEN** the solution is initialized
- **THEN** it SHALL create a `GolMetrics.API` project targeting `net10.0` using `dotnet new webapi`
- **AND** it SHALL create a `GolMetrics.API.Tests` project using `dotnet new xunit`
- **AND** the API project SHALL include `[assembly: InternalsVisibleTo("GolMetrics.API.Tests")]` to allow testing of internal types

### Requirement: NuGet Packages

The solution SHALL install packages grouped by concern.

#### Scenario: Core infrastructure packages

- **WHEN** the API project is set up
- **THEN** it SHALL install: `MediatR`, `FluentValidation.DependencyInjectionExtensions`, `Microsoft.EntityFrameworkCore.Design`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `EFCore.NamingConventions`, `Serilog.AspNetCore`, `Scalar.AspNetCore`

#### Scenario: Authentication and security packages

- **WHEN** authentication is configured
- **THEN** it SHALL install: `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.AspNetCore.Authentication.JwtBearer`

#### Scenario: AI and Semantic Kernel packages

- **WHEN** AI chat processing is configured
- **THEN** it SHALL install: `Microsoft.SemanticKernel`, `Microsoft.SemanticKernel.Connectors.Google`

#### Scenario: Test packages

- **WHEN** the test project is set up
- **THEN** it SHALL install: `xunit`, `FluentAssertions`, `NSubstitute`, `Microsoft.AspNetCore.Mvc.Testing`, `Testcontainers.PostgreSql`

### Requirement: Program.cs Configuration

The application SHALL configure services and middleware in a specific order.

#### Scenario: Service registration order

- **WHEN** `Program.cs` registers services
- **THEN** it SHALL call builder extension methods in this order: `AddApiServices()` (MediatR, FluentValidation, Swagger, Serilog) -> `AddDatabase()` (DbContext, EF Core) -> `AddAuthenticationServices()` (Identity, JWT) -> `AddErrorHandling()` (exception handlers) -> `AddCors()`

#### Scenario: Middleware pipeline order

- **WHEN** the middleware pipeline is built
- **THEN** it SHALL apply middleware in this order: `UseExceptionHandler()` -> `UseCors()` -> `UseAuthentication()` -> `UseAuthorization()` -> `MapSliceEndpoints()` -> `MapScalarApiReference()`

### Requirement: appsettings.json Structure

The application SHALL define configuration sections in `appsettings.json`.

#### Scenario: Configuration sections

- **WHEN** `appsettings.json` is created
- **THEN** it SHALL include the following sections: `ConnectionStrings` (with `DefaultConnection` for PostgreSQL), `TokenOptions` (with `SecretKey`, `Issuer`, `Audience`, `ExpirationMinutes`), `Encryption` (with `Key` for AES-256), `ApiFootball` (with `BaseUrl`, `ApiKey`), `Gemini` (with `ApiKey`, `ModelId`)
- **AND** sensitive values SHALL use placeholder strings in committed config and real values in `appsettings.Development.json` or user secrets

### Requirement: ISlice Infrastructure

The system SHALL define an `ISlice` interface and auto-discovery extensions.

#### Scenario: ISlice interface

- **WHEN** a feature slice is created
- **THEN** it SHALL implement `ISlice` which defines `void RegisterEndpoints(IEndpointRouteBuilder routes)`

#### Scenario: Slice auto-discovery

- **WHEN** services are registered
- **THEN** `ServiceCollectionExtensions.AddSlices()` (C# 14 `extension(IServiceCollection)`) SHALL scan the assembly for all types implementing `ISlice` and register them as transient services

#### Scenario: Endpoint mapping

- **WHEN** the middleware pipeline is built
- **THEN** `EndpointRouteBuilderExtensions.MapSliceEndpoints()` (C# 14 `extension(IEndpointRouteBuilder)`) SHALL resolve all `ISlice` implementations and call `RegisterEndpoints()` on each

### Requirement: Result Pattern

The system SHALL define a Result pattern for operation outcomes.

#### Scenario: ErrorCategory enum

- **WHEN** errors are categorized
- **THEN** `ErrorCategory` SHALL define: `BadRequest`, `Unauthorized`, `Forbidden`, `NotFound`, `Conflict`

#### Scenario: Error record

- **WHEN** an error is represented
- **THEN** `Error` SHALL be a `public sealed record` with properties `Code` (string), `Message` (string), `Category` (ErrorCategory)

#### Scenario: Result class

- **WHEN** an operation outcome is represented
- **THEN** `Result` SHALL expose `IsSuccess` (bool), `Error` (Error), static `Success()`, static `Failure(Error)`, and SHALL be non-generic for void operations
- **AND** `Result<T>` SHALL additionally expose `Value` (T) and support implicit conversion from `T` to `Result<T>` for success cases

#### Scenario: ToProblemDetails extension

- **WHEN** a failed Result is converted to an HTTP response
- **THEN** `ResultExtensions.ToProblemDetails()` (C# 14 `extension(Result)`) SHALL map `ErrorCategory` to HTTP status codes: BadRequest->400, Unauthorized->401, Forbidden->403, NotFound->404, Conflict->409

### Requirement: Entity Base Class

The system SHALL define an abstract `Entity` base class for all domain entities.

#### Scenario: Entity properties

- **WHEN** a domain entity is created
- **THEN** it SHALL inherit from `Entity` which defines: `Id` (Guid), `CreatedBy` (Guid), `LastModifiedBy` (Guid?), `CreatedAtUtc` (DateTime), `UpdatedAtUtc` (DateTime?), `Version` (uint, for optimistic concurrency)

### Requirement: EntityConfiguration Base

The system SHALL define an abstract `EntityConfiguration<TEntity>` for EF Core configurations.

#### Scenario: Base configuration

- **WHEN** an entity configuration is created
- **THEN** it SHALL inherit from `EntityConfiguration<TEntity>` which configures: primary key on `Id`, audit fields (`CreatedBy`, `LastModifiedBy`, `CreatedAtUtc`, `UpdatedAtUtc`), row version on `Version` using `IsRowVersion()`

### Requirement: Exception Handlers

The system SHALL define 4 exception handlers in priority order, each returning `ProblemDetails`.

#### Scenario: Validation exception handler

- **WHEN** a `ValidationException` (FluentValidation) is thrown
- **THEN** `ValidationExceptionHandler` SHALL return HTTP 400 with validation errors as `ProblemDetails` extensions

#### Scenario: Database exception handler

- **WHEN** a `DbUpdateException` or `DbUpdateConcurrencyException` is thrown
- **THEN** `DatabaseExceptionHandler` SHALL return HTTP 409 with conflict details as `ProblemDetails`

#### Scenario: JSON exception handler

- **WHEN** a `JsonException` is thrown
- **THEN** `JsonExceptionHandler` SHALL return HTTP 400 with parsing error details as `ProblemDetails`

#### Scenario: Global exception handler

- **WHEN** any unhandled exception is thrown
- **THEN** `GlobalExceptionHandler` SHALL return HTTP 500 with a generic error `ProblemDetails` and log the exception via Serilog

### Requirement: ValidationBehavior Pipeline

The system SHALL define a MediatR pipeline behavior for request validation.

#### Scenario: Validation pipeline

- **WHEN** a MediatR request is sent
- **THEN** `ValidationBehavior<TRequest, TResponse>` (implementing `IPipelineBehavior<TRequest, TResponse>`) SHALL resolve all `IValidator<TRequest>` instances, run validation, and throw `ValidationException` if any rules fail
- **AND** if no validators exist for the request, it SHALL pass through to the next behavior

### Requirement: CurrentUserService

The system SHALL provide `ICurrentUserService` for accessing the authenticated user's identity.

#### Scenario: User identity resolution

- **WHEN** `ICurrentUserService` is accessed
- **THEN** it SHALL expose `UserId` (Guid), `Email` (string), `IsAuthenticated` (bool), resolved from JWT claims in `IHttpContextAccessor`

### Requirement: EndpointNames Structure

The system SHALL define route constants in `EndpointNames.cs`.

#### Scenario: Route constant organization

- **WHEN** endpoint routes are defined
- **THEN** `EndpointNames` SHALL be a static class containing nested static classes per feature (e.g., `EndpointNames.Auth`, `EndpointNames.Chat`, `EndpointNames.Football`)
- **AND** each nested class SHALL define `Names` (string constants for endpoint naming) and `Routes` (string constants for URL patterns)

### Requirement: DependencyInjection

The system SHALL register all dependencies via C# 14 extension methods on `WebApplicationBuilder`.

#### Scenario: Extension method structure

- **WHEN** dependencies are registered
- **THEN** `DependencyInjection.cs` SHALL define C# 14 `extension(WebApplicationBuilder)` methods: `AddApiServices()` (registers MediatR, FluentValidation, slices, Swagger, Serilog), `AddDatabase()` (registers DbContext with PostgreSQL and snake_case naming), `AddAuthenticationServices()` (registers Identity, JWT Bearer), `AddErrorHandling()` (registers exception handlers in priority order), `AddCors()` (registers CORS policy)
