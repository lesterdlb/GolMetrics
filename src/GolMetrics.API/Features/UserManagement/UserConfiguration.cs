using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GolMetrics.API.Features.UserManagement;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.EncryptedApiKey).HasMaxLength(512);
        builder.Property(u => u.Version).IsRowVersion();
        builder.Property(u => u.CreatedBy).IsRequired();
        builder.Property(u => u.CreatedAtUtc).IsRequired();
    }
}