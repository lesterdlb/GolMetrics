using GolMetrics.API.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GolMetrics.API.Features.Auth;

internal sealed class RefreshTokenConfiguration : EntityConfiguration<RefreshTokenEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.Property(rt => rt.Token).IsRequired().HasMaxLength(128);
        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.Property(rt => rt.ExpiresAtUtc).IsRequired();
        builder.Property(rt => rt.IsRevoked).IsRequired().HasDefaultValue(false);

        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}