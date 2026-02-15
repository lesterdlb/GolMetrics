# Architecture

## Purpose

Defines the foundational patterns and infrastructure shared across all features: Vertical Slice structure, MediatR dispatching, Result pattern, validation pipeline, exception handling, route constants, base entity, and dependency injection registration.

## Requirements

### Requirement: Vertical Slice Structure

The system SHALL organize features as vertical slices using `internal sealed class : ISlice` with nested types.

#### Scenario: Slice registration

- **WHEN** the application starts
- **THEN** `AddSlices()` SHALL scan the assembly for all `ISlice` implementations
- **AND** register their dependencies in the DI container

#### Scenario: Endpoint mapping

- **WHEN** the application configures the HTTP pipeline
- **THEN** `MapSliceEndpoints()` SHALL invoke `AddEndpoint(IEndpointRouteBuilder)` on every registered `ISlice`
- **AND** each slice SHALL define its route, HTTP method, and handler within `AddEndpoint()`

#### Scenario: Slice internal structure

- **WHEN** a new feature slice is created
- **THEN** it SHALL be an `internal sealed class` implementing `ISlice`
- **AND** Command/Query types SHALL be `internal sealed` classes nested inside the slice
- **AND** Validator and Handler types SHALL be `internal sealed` classes nested inside the slice
- **AND** request/response DTOs SHALL be `public sealed record` types

### Requirement: MediatR Dispatching

The system SHALL use the MediatR NuGet package for command/query dispatching via `ISender`.

#### Scenario: Command dispatching

- **WHEN** an endpoint receives a valid HTTP request
- **THEN** the endpoint SHALL create a Command or Query and call `ISender.Send()`
- **AND** MediatR SHALL route the request to the matching `IRequestHandler<TRequest, TResponse>`

#### Scenario: Pipeline behaviors

- **WHEN** `ISender.Send()` is called
- **THEN** registered `IPipelineBehavior<,>` implementations SHALL execute in order before the handler
- **AND** `ValidationBehavior<,>` SHALL be the first pipeline behavior

### Requirement: Result Pattern

The system SHALL use a custom `Result<T>` type for all handler return values instead of throwing exceptions for expected failures.

#### Scenario: Successful operation

- **WHEN** a handler completes successfully
- **THEN** it SHALL return the value via implicit conversion to `Result<T>`

#### Scenario: Business error

- **WHEN** a handler encounters a business rule violation
- **THEN** it SHALL return `Result.Failure(error)` with a feature-specific `Error`
- **AND** the `Error` SHALL contain `Code`, `Message`, and `ErrorCategory`

#### Scenario: HTTP response conversion

- **WHEN** an endpoint receives a `Result<T>` from `ISender.Send()`
- **AND** the result is a failure
- **THEN** it SHALL call `ToProblemDetails()` to convert the error to an HTTP ProblemDetails response
- **AND** `ErrorCategory.BadRequest` SHALL map to HTTP 400
- **AND** `ErrorCategory.Unauthorized` SHALL map to HTTP 401
- **AND** `ErrorCategory.Forbidden` SHALL map to HTTP 403
- **AND** `ErrorCategory.NotFound` SHALL map to HTTP 404
- **AND** `ErrorCategory.Conflict` SHALL map to HTTP 409

### Requirement: Validation Pipeline

The system SHALL validate all commands/queries via FluentValidation before handler execution.

#### Scenario: Valid request

- **WHEN** `ValidationBehavior<,>` receives a request with a registered validator
- **AND** validation passes
- **THEN** it SHALL call `next()` to proceed to the handler

#### Scenario: Invalid request

- **WHEN** `ValidationBehavior<,>` receives a request with a registered validator
- **AND** validation fails
- **THEN** it SHALL throw a `ValidationException` with all validation errors

#### Scenario: No validator registered

- **WHEN** `ValidationBehavior<,>` receives a request with no registered validator
- **THEN** it SHALL call `next()` to proceed to the handler without validation

### Requirement: Exception Handling

The system SHALL handle exceptions through 4 exception handlers in priority order.

#### Scenario: Validation exception

- **WHEN** a `ValidationException` is thrown
- **THEN** the Validation exception handler SHALL return HTTP 400 with ProblemDetails containing field-level errors

#### Scenario: Database exception

- **WHEN** a database-related exception is thrown (e.g., unique constraint violation)
- **THEN** the Database exception handler SHALL return HTTP 409 or 500 with ProblemDetails

#### Scenario: JSON exception

- **WHEN** a JSON deserialization exception is thrown
- **THEN** the Json exception handler SHALL return HTTP 400 with ProblemDetails

#### Scenario: Unhandled exception

- **WHEN** any other unhandled exception is thrown
- **THEN** the Global exception handler SHALL return HTTP 500 with ProblemDetails
- **AND** it SHALL log the exception details via Serilog

### Requirement: Route Constants

The system SHALL define all route names as constants in `EndpointNames.cs`.

#### Scenario: Route naming

- **WHEN** a slice defines an endpoint
- **THEN** it SHALL reference a constant from `EndpointNames` for the route name
- **AND** the constant name SHALL match the pattern `<Feature><Action>` (e.g., `AuthRegister`, `ChatSendMessage`)

### Requirement: Base Entity

The system SHALL provide a base `Entity` class with common audit fields.

#### Scenario: Entity fields

- **WHEN** a domain entity inherits from `Entity`
- **THEN** it SHALL have `Id` (Guid), `CreatedBy` (string), `LastModifiedBy` (string), `CreatedAtUtc` (DateTime), `UpdatedAtUtc` (DateTime), and `Version` (uint) properties

### Requirement: ISlice Interface

The system SHALL define `ISlice` as the contract for vertical slice endpoint registration.

#### Scenario: Interface definition

- **WHEN** a vertical slice implements `ISlice`
- **THEN** it SHALL implement a single method `AddEndpoint(IEndpointRouteBuilder endpoints)`

#### Scenario: Slice discovery

- **WHEN** `AddSlices()` scans the assembly
- **THEN** it SHALL discover all non-public, non-abstract types implementing `ISlice`
- **AND** it SHALL register each discovered type in the DI container

### Requirement: Permission Model

The system SHALL enforce a single-role permission model where all registered users receive the same permissions.

#### Scenario: Permission constants

- **WHEN** permissions are referenced
- **THEN** the `Permissions` static class SHALL define nested resource classes (`Conversations`, `Users`)
- **AND** each class SHALL contain `Read` and `Write` string constants in `resource:action` format (e.g., `conversations:read`, `conversations:write`, `user:read`, `user:write`)

#### Scenario: Permission enforcement

- **WHEN** an endpoint requires authorization
- **THEN** the slice SHALL call `RequirePermissions()` extension method on `RouteHandlerBuilder`
- **AND** `RequirePermissions()` SHALL call `RequireAuthorization` with a `PermissionAuthorizationHandler`

#### Scenario: Permission check passes

- **WHEN** the authenticated user's JWT contains the required `permissions` claim
- **THEN** the request SHALL proceed to the endpoint handler

#### Scenario: Permission check fails

- **WHEN** the authenticated user's JWT does not contain the required `permissions` claim
- **THEN** the system SHALL return HTTP 403 Forbidden

#### Scenario: Unauthenticated access to protected endpoint

- **WHEN** an unauthenticated request is made to an endpoint that requires permissions
- **THEN** the system SHALL return HTTP 401 Unauthorized

### Requirement: Dependency Injection Registration

The system SHALL register all services via `DependencyInjection.cs` extension methods.

#### Scenario: Service registration

- **WHEN** the application starts
- **THEN** `DependencyInjection.cs` SHALL provide extension methods on `WebApplicationBuilder`
- **AND** these methods SHALL register MediatR, FluentValidation, EF Core, Identity, JWT, and application services
