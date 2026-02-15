using GolMetrics.API.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GolMetrics.API.Features.FootballData;

internal sealed class CachedQueryConfiguration : EntityConfiguration<CachedQuery>
{
    protected override void ConfigureEntity(EntityTypeBuilder<CachedQuery> builder)
    {
        builder.Property(c => c.QueryHash).IsRequired();
        builder.Property(c => c.Endpoint).IsRequired();
        builder.Property(c => c.Params).HasColumnType("jsonb").IsRequired();
        builder.Property(c => c.ResponseData).HasColumnType("jsonb").IsRequired();
        builder.Property(c => c.ExpiresAt).IsRequired();

        builder.HasIndex(c => c.QueryHash).IsUnique();
        builder.HasIndex(c => c.ExpiresAt);
    }
}