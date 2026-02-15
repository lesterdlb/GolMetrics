# Data Model

## Purpose

Defines the EF Core data model, entity configurations, database context, and persistence conventions for all domain entities.

## Requirements

### Requirement: User Entity

The system SHALL define a `User` entity extending `IdentityUser<Guid>` with audit fields from `Entity`.

#### Scenario: User properties

- **WHEN** a User entity is created
- **THEN** it SHALL include `EncryptedApiKey` (string, nullable) for BYOK storage
- **AND** it SHALL include `CreatedBy`, `LastModifiedBy`, `CreatedAtUtc`, `UpdatedAtUtc`, and `Version` from the base Entity

### Requirement: Conversation Entity

The system SHALL define a `Conversation` entity inheriting from `Entity`.

#### Scenario: Conversation properties

- **WHEN** a Conversation entity is created
- **THEN** it SHALL include `Title` (string), `UserId` (Guid, FK to User)
- **AND** it SHALL have a navigation property to its `User` and a collection of `Message` entities

### Requirement: Message Entity

The system SHALL define a `Message` entity inheriting from `Entity`.

#### Scenario: Message properties

- **WHEN** a Message entity is created
- **THEN** it SHALL include `Content` (string), `Role` (MessageRole enum), `ConversationId` (Guid, FK to Conversation), `Timestamp` (DateTime)
- **AND** `MessageRole` SHALL be an enum with values `User` and `Assistant`

### Requirement: CachedQuery Entity

The system SHALL define a `CachedQuery` entity inheriting from `Entity`.

#### Scenario: CachedQuery properties

- **WHEN** a CachedQuery entity is created
- **THEN** it SHALL include `QueryHash` (string), `Endpoint` (string), `Params` (JSONB), `ResponseData` (JSONB), `ExpiresAt` (DateTime)

### Requirement: Abstract Entity Configuration Base

The system SHALL provide an abstract `EntityConfiguration<TEntity>` base class for EF Core configurations.

#### Scenario: Base configuration

- **WHEN** an entity configuration inherits from `EntityConfiguration<TEntity>`
- **THEN** it SHALL automatically configure `Id` as the primary key
- **AND** it SHALL configure `Version` as a concurrency token (row version)
- **AND** it SHALL configure `CreatedAtUtc`, `UpdatedAtUtc`, `CreatedBy`, `LastModifiedBy` columns

### Requirement: Per-Entity Fluent API Configurations

The system SHALL define Fluent API configurations for each entity.

#### Scenario: User configuration

- **WHEN** the User entity is configured
- **THEN** it SHALL have a unique index on `Email`
- **AND** `EncryptedApiKey` SHALL have a max length of 512

#### Scenario: Conversation configuration

- **WHEN** the Conversation entity is configured
- **THEN** it SHALL have a foreign key from `UserId` to `User` with cascade delete
- **AND** `Title` SHALL have a max length of 200

#### Scenario: Message configuration

- **WHEN** the Message entity is configured
- **THEN** it SHALL have a foreign key from `ConversationId` to `Conversation` with cascade delete
- **AND** `Role` SHALL be stored as a string (enum-as-string conversion)
- **AND** it SHALL have an index on `ConversationId` and `Timestamp`

#### Scenario: CachedQuery configuration

- **WHEN** the CachedQuery entity is configured
- **THEN** it SHALL have a unique index on `QueryHash`
- **AND** `Params` and `ResponseData` SHALL use JSONB column type
- **AND** it SHALL have an index on `ExpiresAt` for efficient cache eviction queries

### Requirement: Database Context

The system SHALL provide `GolMetricsDbContext` extending `IdentityDbContext<User, IdentityRole<Guid>, Guid>`.

#### Scenario: Audit field automation

- **WHEN** `SaveChangesAsync` is called
- **THEN** the context SHALL automatically set `CreatedAtUtc` and `CreatedBy` on added entities
- **AND** it SHALL set `UpdatedAtUtc` and `LastModifiedBy` on modified entities
- **AND** it SHALL resolve the current user via `ICurrentUserService`

#### Scenario: Naming convention

- **WHEN** the model is built
- **THEN** `UseSnakeCaseNamingConvention()` SHALL be applied to all table and column names

#### Scenario: Enum storage

- **WHEN** an entity contains an enum property
- **THEN** EF Core SHALL store it as a string, not an integer
