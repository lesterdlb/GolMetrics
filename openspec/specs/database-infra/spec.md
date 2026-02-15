# Database Infrastructure

## Purpose

Specifies EF Core infrastructure configuration beyond entity definitions: interceptors for audit field automation, snake_case naming convention, and migration strategy. Entity definitions and per-entity Fluent API configurations are covered in `data-model/spec.md`.

## Requirements

### Requirement: SaveChanges Interceptor

The system SHALL automatically populate audit fields during persistence operations.

#### Scenario: New entity audit fields

- **WHEN** `SaveChangesAsync` is called and entities are in `Added` state
- **THEN** the interceptor SHALL set `CreatedAtUtc` to `DateTime.UtcNow`
- **AND** it SHALL set `CreatedBy` to the current user ID from `ICurrentUserService`

#### Scenario: Modified entity audit fields

- **WHEN** `SaveChangesAsync` is called and entities are in `Modified` state
- **THEN** the interceptor SHALL set `UpdatedAtUtc` to `DateTime.UtcNow`
- **AND** it SHALL set `LastModifiedBy` to the current user ID from `ICurrentUserService`

#### Scenario: System operations without authenticated user

- **WHEN** `ICurrentUserService.UserId` is null (e.g., seeding or system operations)
- **THEN** `CreatedBy` and `LastModifiedBy` SHALL be set to `"system"`

### Requirement: Snake Case Naming Convention

The system SHALL apply snake_case naming to all database objects.

#### Scenario: Naming convention application

- **WHEN** the EF Core model is built
- **THEN** `UseSnakeCaseNamingConvention()` SHALL be applied via the `EFCore.NamingConventions` NuGet package
- **AND** all table names, column names, and index names SHALL be in snake_case

### Requirement: Enum Storage

The system SHALL store enum properties as strings.

#### Scenario: Enum column type

- **WHEN** an entity contains an enum property
- **THEN** EF Core SHALL store it as a string via `HasConversion<string>()`

### Requirement: Optimistic Concurrency

The system SHALL use optimistic concurrency control via row versioning.

#### Scenario: Concurrency token configuration

- **WHEN** an entity inherits from `Entity`
- **THEN** the `Version` property (uint) SHALL be configured as a concurrency token

#### Scenario: Concurrency conflict handling

- **WHEN** a concurrency conflict occurs during `SaveChangesAsync`
- **THEN** the database exception handler SHALL return HTTP 409 Conflict

### Requirement: Migration Strategy

The system SHALL use EF Core migrations for schema management.

#### Scenario: Migration generation

- **WHEN** schema changes are made
- **THEN** migrations SHALL be generated via `dotnet ef migrations add <Name>`

#### Scenario: Development migration application

- **WHEN** the application starts in development
- **THEN** pending migrations SHALL be applied automatically via `Database.MigrateAsync()`

#### Scenario: Production migration application

- **WHEN** the application starts in production
- **THEN** migrations SHALL NOT be applied automatically
