## 1. Entity Definitions

- [x] 1.1 Create `MessageRole` enum in `Features/Chat/` with values `User` and `Assistant`
- [x] 1.2 Create `Conversation` entity in `Features/Chat/` with `Title`, `UserId`, navigation properties
- [x] 1.3 Create `Message` entity in `Features/Chat/` with `Content`, `Role`, `ConversationId`, `Timestamp`
- [x] 1.4 Create `CachedQuery` entity in `Features/FootballData/` with `QueryHash`, `Endpoint`, `Params`, `ResponseData`, `ExpiresAt`

## 2. Entity Configurations

- [x] 2.1 Create `UserConfiguration` in `Features/UserManagement/` implementing `IEntityTypeConfiguration<User>` (unique index on Email, EncryptedApiKey max length, Version as row version, audit fields)
- [x] 2.2 Create `ConversationConfiguration` in `Features/Chat/` extending `EntityConfiguration<Conversation>` (FK to User, Title max length)
- [x] 2.3 Create `MessageConfiguration` in `Features/Chat/` extending `EntityConfiguration<Message>` (FK to Conversation, Role as string, composite index)
- [x] 2.4 Create `CachedQueryConfiguration` in `Features/FootballData/` extending `EntityConfiguration<CachedQuery>` (unique index on QueryHash, JSONB columns, ExpiresAt index)

## 3. DbContext and Interceptor Updates

- [x] 3.1 Create `src/GolMetrics.API/Core/Persistence/AuditableEntityInterceptor.cs` -- `internal sealed class` inheriting from `SaveChangesInterceptor`, resolving `ICurrentUserService`, and implementing the audit logic for `Entity` and `User` types
- [x] 3.2 Update `src/GolMetrics.API/DependencyInjection.cs` to register `AuditableEntityInterceptor` as a scoped service and add it to the `DbContext` options via `AddInterceptors()`
- [x] 3.3 Remove `SaveChangesAsync` override and `ICurrentUserService` dependency from `src/GolMetrics.API/Core/Persistence/GolMetricsDbContext.cs`
- [x] 3.4 Add `DbSet<Conversation>`, `DbSet<Message>`, `DbSet<CachedQuery>` properties to `GolMetricsDbContext`

## 4. Migration and Startup

- [x] 4.1 Add `Microsoft.EntityFrameworkCore.Design` package if not already present
- [x] 4.2 Generate initial EF Core migration
- [x] 4.3 Add auto-migration in `Program.cs` gated by `IsDevelopment()`

## 5. Verification

- [x] 5.1 Verify the application builds successfully
- [x] 5.2 Verify migration applies cleanly against a fresh PostgreSQL database
