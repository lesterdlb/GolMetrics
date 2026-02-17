using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Core.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Features.Chat;

internal sealed class GetConversations : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet(EndpointNames.Chat.Routes.GetConversations,
                async (ICurrentUserService currentUser, ISender sender) =>
                {
                    var result = await sender.Send(new Query(currentUser.UserId));
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : result.ToProblemDetails();
                })
            .WithName(EndpointNames.Chat.GetConversations)
            .RequirePermissions(Permissions.Conversations.Read);
    }

    internal sealed record Query(Guid UserId) : IRequest<Result<IReadOnlyList<Response>>>;

    public sealed record Response(Guid Id, string Title, DateTime CreatedAt, DateTime? UpdatedAt);

    internal sealed class Handler(GolMetricsDbContext db)
        : IRequestHandler<Query, Result<IReadOnlyList<Response>>>
    {
        public async Task<Result<IReadOnlyList<Response>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var conversations = await db.Conversations
                .Where(c => c.UserId == request.UserId)
                .OrderByDescending(c => c.UpdatedAtUtc ?? c.CreatedAtUtc)
                .Select(c => new Response(c.Id, c.Title, c.CreatedAtUtc, c.UpdatedAtUtc))
                .ToListAsync(cancellationToken);

            return conversations;
        }
    }
}