using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.Chat;
using GolMetrics.API.Features.FootballData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Moq;

namespace GolMetrics.API.Tests.Features.FootballData;

[Trait("Category", "Unit")]
public class SemanticKernelServiceTests
{
    private readonly Mock<IChatCompletionService> _chatCompletionMock = new();
    private readonly SemanticKernelService _sut;

    public SemanticKernelServiceTests()
    {
        var footballApiClientMock = new Mock<IFootballApiClient>();
        var footballPlugin = new FootballPlugin(footballApiClientMock.Object);

        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.Services.AddSingleton(_chatCompletionMock.Object);
        var kernel = kernelBuilder.Build();
        _sut = new SemanticKernelService(kernel, footballPlugin);
    }

    [Fact]
    public async Task ProcessMessageAsync_EmptyHistory_SendsSystemPromptAndUserMessage()
    {
        ChatHistory? capturedHistory = null;
        _chatCompletionMock
            .Setup(c => c.GetChatMessageContentsAsync(
                It.IsAny<ChatHistory>(),
                It.IsAny<PromptExecutionSettings>(),
                It.IsAny<Kernel>(),
                It.IsAny<CancellationToken>()))
            .Callback<ChatHistory, PromptExecutionSettings?, Kernel?, CancellationToken>((history, _, _, _) =>
                capturedHistory = history)
            .ReturnsAsync([new ChatMessageContent(AuthorRole.Assistant, "AI response")]);

        var result = await _sut.ProcessMessageAsync("Hello", []);

        result.Should().Be("AI response");
        capturedHistory.Should().NotBeNull();
        capturedHistory!.Should().HaveCount(2);
        capturedHistory[0].Role.Should().Be(AuthorRole.System);
        capturedHistory[1].Role.Should().Be(AuthorRole.User);
        capturedHistory[1].Content.Should().Be("Hello");
    }

    [Fact]
    public async Task ProcessMessageAsync_WithHistory_BuildsChatHistoryInOrder()
    {
        ChatHistory? capturedHistory = null;
        _chatCompletionMock
            .Setup(c => c.GetChatMessageContentsAsync(
                It.IsAny<ChatHistory>(),
                It.IsAny<PromptExecutionSettings>(),
                It.IsAny<Kernel>(),
                It.IsAny<CancellationToken>()))
            .Callback<ChatHistory, PromptExecutionSettings?, Kernel?, CancellationToken>((history, _, _, _) =>
                capturedHistory = history)
            .ReturnsAsync([new ChatMessageContent(AuthorRole.Assistant, "Latest response")]);

        var chatHistory = new List<Message>
        {
            new() { Content = "First question", Role = MessageRole.User, Timestamp = DateTime.UtcNow.AddMinutes(-2) },
            new() { Content = "First answer", Role = MessageRole.Assistant, Timestamp = DateTime.UtcNow.AddMinutes(-1) }
        };

        var result = await _sut.ProcessMessageAsync("Second question", chatHistory);

        result.Should().Be("Latest response");
        capturedHistory.Should().NotBeNull();
        capturedHistory!.Should().HaveCount(4);
        capturedHistory[0].Role.Should().Be(AuthorRole.System);
        capturedHistory[1].Role.Should().Be(AuthorRole.User);
        capturedHistory[1].Content.Should().Be("First question");
        capturedHistory[2].Role.Should().Be(AuthorRole.Assistant);
        capturedHistory[2].Content.Should().Be("First answer");
        capturedHistory[3].Role.Should().Be(AuthorRole.User);
        capturedHistory[3].Content.Should().Be("Second question");
    }

    [Fact]
    public async Task ProcessMessageAsync_NullResponseContent_ReturnsEmptyString()
    {
        _chatCompletionMock
            .Setup(c => c.GetChatMessageContentsAsync(
                It.IsAny<ChatHistory>(),
                It.IsAny<PromptExecutionSettings>(),
                It.IsAny<Kernel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([new ChatMessageContent(AuthorRole.Assistant, (string?)null)]);

        var result = await _sut.ProcessMessageAsync("Test", []);

        result.Should().BeEmpty();
    }
}