using GolMetrics.API.Features.Chat;

namespace GolMetrics.API.Core.Abstractions;

public interface ISemanticKernelService
{
    Task<string> ProcessMessageAsync(string userMessage, IReadOnlyList<Message> chatHistory,
        CancellationToken cancellationToken = default);
}