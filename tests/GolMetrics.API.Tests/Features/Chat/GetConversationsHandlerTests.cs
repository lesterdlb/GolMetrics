using FluentAssertions;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.Chat;
using GolMetrics.API.Tests.Common.Fakers;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Tests.Features.Chat;

[Trait("Category", "Unit")]
public class GetConversationsHandlerTests : IDisposable
{
    private readonly GolMetricsDbContext _dbContext;
    private readonly GetConversations.Handler _sut;
    private readonly ConversationFaker _conversationFaker = new();

    public GetConversationsHandlerTests()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new GolMetricsDbContext(options);
        _sut = new GetConversations.Handler(_dbContext);
    }

    [Fact]
    public async Task Handle_UserHasConversations_ReturnsOrderedByMostRecent()
    {
        var userId = Guid.NewGuid();
        var older = _conversationFaker.Generate();
        older.UserId = userId;
        older.CreatedBy = userId;
        older.CreatedAtUtc = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        older.UpdatedAtUtc = null;

        var newer = _conversationFaker.Generate();
        newer.UserId = userId;
        newer.CreatedBy = userId;
        newer.CreatedAtUtc = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        newer.UpdatedAtUtc = null;

        _dbContext.Conversations.AddRange(older, newer);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.Handle(new GetConversations.Query(userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(2);
        result.Value![0].Id.Should().Be(newer.Id);
        result.Value[1].Id.Should().Be(older.Id);
    }

    [Fact]
    public async Task Handle_UserHasNoConversations_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid();

        var result = await _sut.Handle(new GetConversations.Query(userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_OtherUsersConversations_AreNotIncluded()
    {
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var ownConversation = _conversationFaker.Generate();
        ownConversation.UserId = userId;
        ownConversation.CreatedBy = userId;

        var otherConversation = _conversationFaker.Generate();
        otherConversation.UserId = otherUserId;
        otherConversation.CreatedBy = otherUserId;

        _dbContext.Conversations.AddRange(ownConversation, otherConversation);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.Handle(new GetConversations.Query(userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(1);
        result.Value![0].Id.Should().Be(ownConversation.Id);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}