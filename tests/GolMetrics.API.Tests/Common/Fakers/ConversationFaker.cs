using Bogus;
using GolMetrics.API.Features.Chat;

namespace GolMetrics.API.Tests.Common.Fakers;

public sealed class ConversationFaker : Faker<Conversation>
{
    public ConversationFaker()
    {
        RuleFor(c => c.Id, f => f.Random.Guid());
        RuleFor(c => c.Title, f => f.Lorem.Sentence(3));
        RuleFor(c => c.UserId, f => f.Random.Guid());
        RuleFor(c => c.CreatedBy, (_, c) => c.UserId);
        RuleFor(c => c.CreatedAtUtc, f => f.Date.Past().ToUniversalTime());
    }
}