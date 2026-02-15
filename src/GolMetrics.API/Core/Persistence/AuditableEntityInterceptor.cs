using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GolMetrics.API.Core.Persistence;

internal sealed class AuditableEntityInterceptor(ICurrentUserService currentUserService)
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var now = DateTime.UtcNow;
        var userId = currentUserService.IsAuthenticated ? currentUserService.UserId : Guid.Empty;

        ApplyAuditFields(eventData.Context.ChangeTracker.Entries<Entity>(), now, userId);
        ApplyUserAuditFields(eventData.Context.ChangeTracker.Entries<User>(), now, userId);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ApplyAuditFields(
        IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity>> entries,
        DateTime now,
        Guid userId)
    {
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAtUtc = now;
                    entry.Entity.LastModifiedBy = userId;
                    break;
            }
        }
    }

    private static void ApplyUserAuditFields(
        IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>> entries,
        DateTime now,
        Guid userId)
    {
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAtUtc = now;
                    entry.Entity.LastModifiedBy = userId;
                    break;
            }
        }
    }
}