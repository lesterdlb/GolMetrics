using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.Auth;

internal sealed class RefreshToken : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost(EndpointNames.Auth.Routes.RefreshToken, async (Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName(EndpointNames.Auth.RefreshToken)
            .AllowAnonymous();
    }

    public sealed record Command(string Token) : IRequest<Result<Response>>;

    public sealed record Response(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Token).NotEmpty();
        }
    }

    internal sealed class Handler(
        UserManager<User> userManager,
        ITokenService tokenService) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = await tokenService.ValidateRefreshTokenAsync(request.Token, cancellationToken);
            if (!validationResult.IsSuccess)
            {
                return Result<Response>.Failure(AuthErrors.InvalidRefreshToken);
            }

            var userId = validationResult.Value!;
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return Result<Response>.Failure(AuthErrors.InvalidRefreshToken);
            }

            await tokenService.RevokeRefreshTokenAsync(request.Token, cancellationToken);

            var accessToken = tokenService.GenerateAccessToken(user);
            var newRefreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

            return new Response(accessToken, newRefreshToken, DateTime.UtcNow.AddDays(7));
        }
    }
}