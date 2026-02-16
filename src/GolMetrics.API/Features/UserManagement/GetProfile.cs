using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.UserManagement;

internal sealed class GetProfile : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet(EndpointNames.User.Routes.GetProfile,
                async (ICurrentUserService currentUser, ISender sender) =>
                {
                    var result = await sender.Send(new Query(currentUser.UserId));
                    return result.IsSuccess
                        ? Results.Ok(result.Value)
                        : result.ToProblemDetails();
                })
            .WithName(EndpointNames.User.GetProfile)
            .RequirePermissions(Permissions.Users.Read);
    }

    internal sealed record Query(Guid UserId) : IRequest<Result<Response>>;

    public sealed record Response(Guid Id, string Email, bool HasApiKey, DateTime CreatedAt);

    internal sealed class Handler(UserManager<User> userManager)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                return Result<Response>.Failure(UserErrors.UserNotFound);
            }

            return new Response(
                user.Id,
                user.Email!,
                !string.IsNullOrEmpty(user.EncryptedApiKey),
                user.CreatedAtUtc);
        }
    }
}