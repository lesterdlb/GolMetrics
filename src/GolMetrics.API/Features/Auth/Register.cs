using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.Auth;

internal sealed class Register : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost(EndpointNames.Auth.Routes.Register, async (Command command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return result.IsSuccess
                    ? Results.Created(EndpointNames.Auth.Routes.Register, result.Value)
                    : result.ToProblemDetails();
            })
            .WithName(EndpointNames.Auth.Register)
            .AllowAnonymous();
    }

    public sealed record Command(string Email, string Password) : IRequest<Result<Response>>;

    public sealed record Response(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }

    internal sealed class Handler(
        UserManager<User> userManager,
        ITokenService tokenService) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                CreatedBy = Guid.Empty,
                CreatedAtUtc = DateTime.UtcNow
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var hasDuplicateEmail = identityResult.Errors
                    .Any(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName");

                return hasDuplicateEmail
                    ? Result<Response>.Failure(AuthErrors.DuplicateEmail)
                    : Result<Response>.Failure(AuthErrors.InvalidPassword);
            }

            var accessToken = tokenService.GenerateAccessToken(user);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);

            return new Response(accessToken, refreshToken, DateTime.UtcNow.AddDays(7));
        }
    }
}