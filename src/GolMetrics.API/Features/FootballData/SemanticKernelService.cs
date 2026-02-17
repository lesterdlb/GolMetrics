using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.Chat;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;

namespace GolMetrics.API.Features.FootballData;

internal sealed class SemanticKernelService(Kernel kernel, FootballPlugin footballPlugin) : ISemanticKernelService
{
    private const string SystemPrompt =
        """
        You are a football statistics assistant. Use the available plugin functions to retrieve
        real-time football data when answering questions. When presenting tabular data such as
        standings, top scorers, or match results, format your response using Markdown tables.
        Be concise and accurate. If you cannot retrieve the requested data, explain why.
        """;

    public async Task<string> ProcessMessageAsync(
        string userMessage,
        IReadOnlyList<Message> chatHistory,
        CancellationToken cancellationToken = default)
    {
        var history = new ChatHistory();
        history.AddSystemMessage(SystemPrompt);

        foreach (var message in chatHistory)
        {
            switch (message.Role)
            {
                case MessageRole.User:
                    history.AddUserMessage(message.Content);
                    break;
                case MessageRole.Assistant:
                    history.AddAssistantMessage(message.Content);
                    break;
            }
        }

        history.AddUserMessage(userMessage);

        var executionSettings = new GeminiPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var requestKernel = kernel.Clone();
        requestKernel.Plugins.Add(KernelPluginFactory.CreateFromObject(footballPlugin));

        var chatCompletionService = requestKernel.GetRequiredService<IChatCompletionService>();
        var response = await chatCompletionService.GetChatMessageContentAsync(
            history, executionSettings, requestKernel, cancellationToken);

        return response.Content ?? string.Empty;
    }
}