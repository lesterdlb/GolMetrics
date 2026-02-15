## Context

The API has a working `GolMetricsDbContext` with inline audit field logic in `SaveChangesAsync`, a base `Entity` class, an abstract `EntityConfiguration<TEntity>`, and PostgreSQL registration with snake_case naming. However, the data model is incomplete: only `User` exists as an entity; `Conversation`, `Message`, and `CachedQuery` are defined in specs but not yet implemented. No entity configurations exist beyond the base class, and no migrations have been generated.

The audit logic in `SaveChangesAsync` duplicates iteration for `Entity` and `User` (since `User` extends `IdentityUser`, not `Entity`). The spec calls for a SaveChanges interceptor pattern.

## Goals / Non-Goals

**Goals:**
- Define all domain entities (`Conversation`, `Message`, `CachedQuery`, `MessageRole`)
- Create per-entity Fluent API configurations matching the data-model spec
- Extract audit field logic into an `AuditableEntityInterceptor`
- Add `DbSet<>` properties to `GolMetricsDbContext`
- Configure global enum-as-string storage
- Generate and apply the initial EF Core migration
- Auto-migrate in development mode

**Non-Goals:**
- Seeding data
- Repository or Unit of Work abstractions (DbContext is used directly)
- Production migration tooling (CI/CD pipeline handles that separately)

## Decisions

### 1. SaveChanges interceptor vs. DbContext override

**Decision**: Extract audit logic into a separate `AuditableEntityInterceptor` class.

**Rationale**: The `database-infra/spec.md` explicitly calls for a "SaveChanges interceptor pattern". While inlining logic in `SaveChangesAsync` is functional, a separate interceptor is cleaner, easier to test in isolation, and keeps the `GolMetricsDbContext` focused on database orchestration rather than metadata management.

**Alternative considered**: Keeping logic in the `SaveChangesAsync` override. Rejected to maintain strict alignment with the specification and improve separation of concerns.

### 2. User entity audit field handling

**Decision**: Handle `User` separately in the interceptor since it extends `IdentityUser<Guid>` instead of `Entity`.

**Rationale**: `User` cannot inherit from both `IdentityUser<Guid>` and `Entity`. The interceptor will iterate both `ChangeTracker.Entries<Entity>()` and `ChangeTracker.Entries<User>()` to ensure all auditable types are covered.

### 3. Entity placement

**Decision**: Place entities in their feature folders (`Features/<FeatureName>/`) rather than a shared `Entities/` folder.

**Rationale**: Follows the vertical slice architecture. `Conversation` and `Message` belong to the Chat feature; `CachedQuery` belongs to FootballData (caching external API responses). `User` already lives in `Features/UserManagement/`.

### 4. Entity configuration placement

**Decision**: Place each `EntityConfiguration` in the same feature folder as its entity.

**Rationale**: Keeps related code together in the vertical slice. EF Core discovers configurations via `ApplyConfigurationsFromAssembly` regardless of folder location.

### 5. Enum-as-string configuration

**Decision**: Configure `MessageRole` enum conversion in `MessageConfiguration` rather than globally in `OnModelCreating`.

**Rationale**: The spec says "enum properties SHALL be stored as a string via `HasConversion<string>()`". Per-property configuration in the entity configuration is more explicit and discoverable. A global convention would be fine too, but with only one enum currently, per-property is simpler.

### 6. Migration auto-apply in development

**Decision**: Call `Database.MigrateAsync()` in `Program.cs` gated by `app.Environment.IsDevelopment()`.

**Rationale**: Matches the spec requirement. Simple and standard for development workflows.

## Risks / Trade-offs

- **[Risk] Initial migration on existing database** -> The migration assumes a fresh database. If anyone has an existing schema, they need to drop and recreate. Acceptable for early development.
- **[Risk] User audit field duplication** -> The interceptor iterates both `Entity` and `User` entries separately. If more non-Entity auditable types appear, this won't scale. -> Mitigation: Introduce `IAuditable` interface later if needed.
- **[Trade-off] Multi-loop iteration** -> The interceptor iterates both `Entity` and `User` entries separately to handle the lack of a shared base class. This is more explicit than a marker interface but requires updating if more non-Entity auditable types are added.
