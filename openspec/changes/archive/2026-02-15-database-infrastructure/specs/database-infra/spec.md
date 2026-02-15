## MODIFIED Requirements

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

- **WHEN** `ICurrentUserService.IsAuthenticated` is false (e.g., seeding or system operations)
- **THEN** `CreatedBy` and `LastModifiedBy` SHALL be set to `Guid.Empty`

#### Scenario: User entity audit fields

- **WHEN** `SaveChangesAsync` is called and `User` entities (which extend `IdentityUser<Guid>`, not `Entity`) are in `Added` or `Modified` state
- **THEN** the same audit field rules SHALL apply via a separate `ChangeTracker.Entries<User>()` iteration

### Requirement: Migration Strategy

The system SHALL use EF Core migrations for schema management.

#### Scenario: Migration generation

- **WHEN** schema changes are made
- **THEN** migrations SHALL be generated via `dotnet ef migrations add <Name>`

#### Scenario: Development migration application

- **WHEN** the application starts in development
- **THEN** pending migrations SHALL be applied automatically via `Database.MigrateAsync()` in `Program.cs`
- **AND** auto-migration SHALL be gated by `app.Environment.IsDevelopment()`

#### Scenario: Production migration application

- **WHEN** the application starts in production
- **THEN** migrations SHALL NOT be applied automatically
