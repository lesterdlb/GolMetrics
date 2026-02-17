using Bogus;
using GolMetrics.API.Features.Chat;

namespace GolMetrics.API.Tests.Common.Fakers;

public sealed class MessageFaker : Faker<Message>
{
    public MessageFaker()
    {
        RuleFor(m => m.Id, f => f.Random.Guid());
        RuleFor(m => m.Content, f => f.Lorem.Paragraph());
        RuleFor(m => m.Role, f => f.PickRandom<MessageRole>());
        RuleFor(m => m.ConversationId, f => f.Random.Guid());
        RuleFor(m => m.Timestamp, f => f.Date.Recent().ToUniversalTime());
        RuleFor(m => m.CreatedBy, f => f.Random.Guid());
        RuleFor(m => m.CreatedAtUtc, (_, m) => m.Timestamp);
    }
}