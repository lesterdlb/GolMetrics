using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Features.Chat;

internal sealed class GetConversationMessages : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet(EndpointNames.Chat.Routes.GetConversationMessages,
                async (Guid id, ICurrentUserService currentUser, ISender sender) =>
                {
                    var result = await sender.Send(new Query(id, currentUser.UserId));
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : result.ToProblemDetails();
                })
            .WithName(EndpointNames.Chat.GetConversationMessages)
            .RequirePermissions(Permissions.Conversations.Read);
    }

    internal sealed record Query(Guid ConversationId, Guid UserId)
        : IRequest<Result<IReadOnlyList<Response>>>;

    public sealed record Response(Guid Id, string Content, string Role, DateTime Timestamp);

    internal sealed class Handler(GolMetricsDbContext db)
        : IRequestHandler<Query, Result<IReadOnlyList<Response>>>
    {
        public async Task<Result<IReadOnlyList<Response>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var conversationExists = await db.Conversations
                .AnyAsync(c => c.Id == request.ConversationId && c.UserId == request.UserId,
                    cancellationToken);

            if (!conversationExists)
                return Result<IReadOnlyList<Response>>.Failure(ChatErrors.ConversationNotFound);

            var messages = await db.Messages
                .Where(m => m.ConversationId == request.ConversationId)
                .OrderBy(m => m.Timestamp)
                .Select(m => new Response(m.Id, m.Content, m.Role.ToString(), m.Timestamp))
                .ToListAsync(cancellationToken);

            return messages;
        }
    }
}