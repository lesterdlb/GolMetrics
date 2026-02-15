using GolMetrics.API.Features.Chat;
using GolMetrics.API.Features.FootballData;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Core.Persistence;

public class GolMetricsDbContext(DbContextOptions<GolMetricsDbContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<CachedQuery> CachedQueries => Set<CachedQuery>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(GolMetricsDbContext).Assembly);
    }
}