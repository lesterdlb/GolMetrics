using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Features.Chat;

public static class ChatErrors
{
    public static Error ConversationNotFound => new(
        "Chat.ConversationNotFound",
        "The conversation was not found.",
        ErrorCategory.NotFound);

    public static Error EmptyContent => new(
        "Chat.EmptyContent",
        "Message content cannot be empty.",
        ErrorCategory.BadRequest);

    public static Error ContentTooLong => new(
        "Chat.ContentTooLong",
        "Message content cannot exceed 4000 characters.",
        ErrorCategory.BadRequest);

    public static Error AiProcessingFailed => new(
        "Chat.AiProcessingFailed",
        "The AI service failed to process the message.",
        ErrorCategory.BadGateway);
}
