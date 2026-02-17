using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Features.Chat;

internal sealed class SendMessage : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost(EndpointNames.Chat.Routes.SendMessage,
                async (Command command, ICurrentUserService currentUser, ISender sender) =>
                {
                    var result = await sender.Send(command with { UserId = currentUser.UserId });
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : result.ToProblemDetails();
                })
            .WithName(EndpointNames.Chat.SendMessage)
            .RequirePermissions(Permissions.Conversations.Write);
    }

    public sealed record Command(string Content, Guid? ConversationId = null, Guid UserId = default)
        : IRequest<Result<Response>>;

    public sealed record Response(string Content, Guid ConversationId);

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(4000);
        }
    }

    internal sealed class Handler(
        GolMetricsDbContext db,
        ISemanticKernelService semanticKernelService) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            Conversation conversation;

            if (request.ConversationId.HasValue)
            {
                var existing = await db.Conversations
                    .FirstOrDefaultAsync(
                        c => c.Id == request.ConversationId.Value && c.UserId == request.UserId,
                        cancellationToken);

                if (existing is null)
                    return Result<Response>.Failure(ChatErrors.ConversationNotFound);

                conversation = existing;
            }
            else
            {
                conversation = new Conversation
                {
                    Title = TruncateAtWordBoundary(request.Content, 100),
                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedAtUtc = DateTime.UtcNow
                };

                db.Conversations.Add(conversation);
                await db.SaveChangesAsync(cancellationToken);
            }

            var chatHistory = await db.Messages
                .Where(m => m.ConversationId == conversation.Id)
                .OrderBy(m => m.Timestamp)
                .ToListAsync(cancellationToken);

            var userMessage = new Message
            {
                Content = request.Content,
                Role = MessageRole.User,
                ConversationId = conversation.Id,
                Timestamp = DateTime.UtcNow,
                CreatedBy = request.UserId,
                CreatedAtUtc = DateTime.UtcNow
            };

            db.Messages.Add(userMessage);
            await db.SaveChangesAsync(cancellationToken);

            string aiResponse;
            try
            {
                aiResponse = await semanticKernelService.ProcessMessageAsync(
                    request.Content, chatHistory, cancellationToken);
            }
            catch
            {
                return Result<Response>.Failure(ChatErrors.AiProcessingFailed);
            }

            var assistantMessage = new Message
            {
                Content = aiResponse,
                Role = MessageRole.Assistant,
                ConversationId = conversation.Id,
                Timestamp = DateTime.UtcNow,
                CreatedBy = request.UserId,
                CreatedAtUtc = DateTime.UtcNow
            };

            db.Messages.Add(assistantMessage);
            conversation.UpdatedAtUtc = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);

            return new Response(aiResponse, conversation.Id);
        }

        private static string TruncateAtWordBoundary(string text, int maxLength)
        {
            if (text.Length <= maxLength)
                return text;

            var lastSpace = text.LastIndexOf(' ', maxLength);
            return lastSpace > 0 ? text[..lastSpace] : text[..maxLength];
        }
    }
}