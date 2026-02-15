using GolMetrics.API.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GolMetrics.API.Features.Chat;

internal sealed class MessageConfiguration : EntityConfiguration<Message>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Message> builder)
    {
        builder.Property(m => m.Content).IsRequired();
        builder.Property(m => m.Role).HasConversion<string>().IsRequired();
        builder.Property(m => m.Timestamp).IsRequired();

        builder.HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.ConversationId, m.Timestamp });
    }
}