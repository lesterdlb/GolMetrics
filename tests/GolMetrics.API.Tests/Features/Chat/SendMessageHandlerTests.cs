using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.Chat;
using GolMetrics.API.Tests.Common.Fakers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GolMetrics.API.Tests.Features.Chat;

[Trait("Category", "Unit")]
public class SendMessageHandlerTests : IDisposable
{
    private readonly GolMetricsDbContext _dbContext;
    private readonly Mock<ISemanticKernelService> _semanticKernelServiceMock;
    private readonly SendMessage.Handler _sut;
    private readonly ConversationFaker _conversationFaker = new();

    public SendMessageHandlerTests()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new GolMetricsDbContext(options);
        _semanticKernelServiceMock = new Mock<ISemanticKernelService>();
        _sut = new SendMessage.Handler(_dbContext, _semanticKernelServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingConversation_SavesMessagesAndReturnsAiResponse()
    {
        var userId = Guid.NewGuid();
        var conversation = _conversationFaker.Generate();
        conversation.UserId = userId;
        conversation.CreatedBy = userId;

        _dbContext.Conversations.Add(conversation);
        await _dbContext.SaveChangesAsync();

        _semanticKernelServiceMock
            .Setup(s => s.ProcessMessageAsync(
                "What are the standings?",
                It.IsAny<List<Message>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Here are the standings...");

        var command = new SendMessage.Command("What are the standings?", conversation.Id, userId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Content.Should().Be("Here are the standings...");
        result.Value.ConversationId.Should().Be(conversation.Id);

        var messages = await _dbContext.Messages.ToListAsync();
        messages.Should().HaveCount(2);
        messages.Should().Contain(m => m.Role == MessageRole.User);
        messages.Should().Contain(m => m.Role == MessageRole.Assistant);
    }

    [Fact]
    public async Task Handle_NoConversationId_AutoCreatesConversation()
    {
        var userId = Guid.NewGuid();

        _semanticKernelServiceMock
            .Setup(s => s.ProcessMessageAsync(
                It.IsAny<string>(),
                It.IsAny<List<Message>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("AI response");

        var command = new SendMessage.Command("Tell me about La Liga", null, userId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var conversation = await _dbContext.Conversations.FirstOrDefaultAsync();
        conversation.Should().NotBeNull();
        conversation!.Title.Should().Be("Tell me about La Liga");
        conversation.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task Handle_LongContent_TruncatesTitleAtWordBoundary()
    {
        var userId = Guid.NewGuid();
        var longContent =
            "This is a very long message that exceeds one hundred characters and should be truncated at the nearest word boundary for the title";

        _semanticKernelServiceMock
            .Setup(s => s.ProcessMessageAsync(
                It.IsAny<string>(),
                It.IsAny<List<Message>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("AI response");

        var command = new SendMessage.Command(longContent, null, userId);
        await _sut.Handle(command, CancellationToken.None);

        var conversation = await _dbContext.Conversations.FirstOrDefaultAsync();
        conversation!.Title.Length.Should().BeLessThanOrEqualTo(100);
    }

    [Fact]
    public async Task Handle_ConversationNotFound_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        var command = new SendMessage.Command("Hello", Guid.NewGuid(), userId);

        var result = await _sut.Handle(command, CancellationToken.None);

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

        var command = new SendMessage.Command("Hello", conversation.Id, requesterId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ChatErrors.ConversationNotFound);
    }

    [Fact]
    public async Task Handle_AiServiceThrows_ReturnsAiProcessingFailedError()
    {
        var userId = Guid.NewGuid();
        var conversation = _conversationFaker.Generate();
        conversation.UserId = userId;
        conversation.CreatedBy = userId;

        _dbContext.Conversations.Add(conversation);
        await _dbContext.SaveChangesAsync();

        _semanticKernelServiceMock
            .Setup(s => s.ProcessMessageAsync(
                It.IsAny<string>(),
                It.IsAny<List<Message>>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        var command = new SendMessage.Command("Hello", conversation.Id, userId);
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ChatErrors.AiProcessingFailed);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}