using FluentAssertions;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.Chat;
using GolMetrics.API.Tests.Common.Fakers;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Tests.Features.Chat;

[Trait("Category", "Unit")]
public class GetConversationMessagesHandlerTests : IDisposable
{
    private readonly GolMetricsDbContext _dbContext;
    private readonly GetConversationMessages.Handler _sut;
    private readonly ConversationFaker _conversationFaker = new();
    private readonly MessageFaker _messageFaker = new();

    public GetConversationMessagesHandlerTests()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new GolMetricsDbContext(options);
        _sut = new GetConversationMessages.Handler(_dbContext);
    }

    [Fact]
    public async Task Handle_ValidConversation_ReturnsMessagesOrderedByTimestamp()
    {
        var userId = Guid.NewGuid();
        var conversation = _conversationFaker.Generate();
        conversation.UserId = userId;
        conversation.CreatedBy = userId;

        var olderMessage = _messageFaker.Generate();
        olderMessage.ConversationId = conversation.Id;
        olderMessage.Timestamp = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        olderMessage.CreatedBy = userId;
        olderMessage.Role = MessageRole.User;

        var newerMessage = _messageFaker.Generate();
        newerMessage.ConversationId = conversation.Id;
        newerMessage.Timestamp = new DateTime(2025, 1, 1, 10, 1, 0, DateTimeKind.Utc);
        newerMessage.CreatedBy = userId;
        newerMessage.Role = MessageRole.Assistant;

        _dbContext.Conversations.Add(conversation);
        _dbContext.Messages.AddRange(olderMessage, newerMessage);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.Handle(
            new GetConversationMessages.Query(conversation.Id, userId),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(2);
        result.Value![0].Id.Should().Be(olderMessage.Id);
        result.Value[1].Id.Should().Be(newerMessage.Id);
    }

    [Fact]
    public async Task Handle_ConversationNotFound_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        var nonExistentConversationId = Guid.NewGuid();

        var result = await _sut.Handle(
            new GetConversationMessages.Query(nonExistentConversationId, userId),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ChatErrors.ConversationNotFound);
    }

    [Fact]
    public async Task Handle_ConversationBelongsToOtherUser_ReturnsFailure()
    {
        var ownerId = Guid.NewGuid();
        var requesterId = Guid.NewGuid();
        var conversation = _conversationFaker.Generate();
        conversation.UserId = ownerId;
        conversation.CreatedBy = ownerId;

        _dbContext.Conversations.Add(conversation);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.Handle(
            new GetConversationMessages.Query(conversation.Id, requesterId),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ChatErrors.ConversationNotFound);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}