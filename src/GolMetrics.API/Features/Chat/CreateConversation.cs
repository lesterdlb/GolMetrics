using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Core.Results;
using MediatR;

namespace GolMetrics.API.Features.Chat;

internal sealed class CreateConversation : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost(EndpointNames.Chat.Routes.CreateConversation,
                async (Command command, ICurrentUserService currentUser, ISender sender) =>
                {
                    var result = await sender.Send(command with { UserId = currentUser.UserId });
                    return result.IsSuccess
                        ? Results.Created($"/api/conversations/{result.Value!.Id}", result.Value)
                        : result.ToProblemDetails();
                })
            .WithName(EndpointNames.Chat.CreateConversation)
            .RequirePermissions(Permissions.Conversations.Write);
    }

    public sealed record Command(string Title, Guid UserId = default) : IRequest<Result<Response>>;

    public sealed record Response(Guid Id, string Title);

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        }
    }

    internal sealed class Handler(GolMetricsDbContext db) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var conversation = new Conversation
            {
                Title = request.Title,
                UserId = request.UserId,
                CreatedBy = request.UserId,
                CreatedAtUtc = DateTime.UtcNow
            };

            db.Conversations.Add(conversation);
            await db.SaveChangesAsync(cancellationToken);

            return new Response(conversation.Id, conversation.Title);
        }
    }
}