using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.Auth;

internal sealed class Login : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost(EndpointNames.Auth.Routes.Login, async (Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : result.ToProblemDetails();
            })
            .WithName(EndpointNames.Auth.Login)
            .AllowAnonymous();
    }

    public sealed record Command(string Email, string Password) : IRequest<Result<Response>>;

    public sealed record Response(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    internal sealed class Handler(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Result<Response>.Failure(AuthErrors.InvalidCredentials);
            }

            var signInResult =
                await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Result<Response>.Failure(AuthErrors.InvalidCredentials);
            }

            var accessToken = tokenService.GenerateAccessToken(user);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

            return new Response(accessToken, refreshToken, DateTime.UtcNow.AddDays(7));
        }
    }
}