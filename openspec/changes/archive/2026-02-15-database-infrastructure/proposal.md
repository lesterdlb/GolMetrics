## Why

The API has core abstractions (Entity, EntityConfiguration, GolMetricsDbContext) and the DI registration for PostgreSQL, but lacks entity configurations for each domain entity, a SaveChanges interceptor (currently inlined in DbContext), entity definitions for Conversation/Message/CachedQuery, and an initial EF Core migration. Without these, no feature can persist data.

## What Changes

- Define `Conversation`, `Message`, `CachedQuery` entities and `MessageRole` enum
- Create per-entity Fluent API configurations (`UserConfiguration`, `ConversationConfiguration`, `MessageConfiguration`, `CachedQueryConfiguration`)
- Extract audit field logic from `GolMetricsDbContext.SaveChangesAsync` into an `AuditableEntityInterceptor` (SaveChanges interceptor)
- Add `DbSet<>` properties to `GolMetricsDbContext` for all entities
- Configure enum-as-string storage globally in `OnModelCreating`
- Generate the initial EF Core migration
- Apply auto-migration in development via `Database.MigrateAsync()`

## Capabilities

### New Capabilities

_(none -- all capabilities already have specs)_

### Modified Capabilities

- `database-infra`: Implementation of the SaveChanges interceptor requirement (moving from inline override to interceptor class)
- `data-model`: Implementation of entity definitions, entity configurations, and DbSet registration

## Impact

- **Code**: `src/GolMetrics.API/Core/Persistence/` (interceptor, DbContext changes), `src/GolMetrics.API/Features/` (entity + configuration files per feature)
- **Dependencies**: May require `Microsoft.EntityFrameworkCore.Design` for migration tooling
- **Database**: Creates initial schema with all tables, indexes, and constraints
- **Startup**: Development mode will auto-apply migrations on startup
