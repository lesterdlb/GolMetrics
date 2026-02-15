using GolMetrics.API.Core.Abstractions;

namespace GolMetrics.API.Features.Chat;

public sealed class Message : Entity
{
    public string Content { get; set; } = string.Empty;
    public MessageRole Role { get; set; }
    public Guid ConversationId { get; set; }
    public DateTime Timestamp { get; set; }
    public Conversation Conversation { get; set; } = null!;
}