using FluentValidation;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Authorization;
using GolMetrics.API.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.UserManagement;

internal sealed class UpdateApiKey : ISlice
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPut(EndpointNames.User.Routes.UpdateApiKey,
                async (Command command, ICurrentUserService currentUser, ISender sender) =>
                {
                    var result = await sender.Send(command with { UserId = currentUser.UserId });
                    return result.IsSuccess
                        ? Results.Ok()
                        : result.ToProblemDetails();
                })
            .WithName(EndpointNames.User.UpdateApiKey)
            .RequirePermissions(Permissions.Users.Write);
    }

    public sealed record Command(string ApiKey) : IRequest<Result>
    {
        internal Guid UserId { get; init; }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ApiKey).NotEmpty();
        }
    }

    internal sealed class Handler(
        UserManager<User> userManager,
        IFootballApiClient footballApiClient,
        IEncryptionService encryptionService) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            bool isValid;
            try
            {
                isValid = await footballApiClient.ValidateApiKeyAsync(request.ApiKey, cancellationToken);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return Result.Failure(UserErrors.ApiValidationUnavailable);
            }

            if (!isValid)
            {
                return Result.Failure(UserErrors.InvalidApiKey);
            }

            user.EncryptedApiKey = encryptionService.Encrypt(request.ApiKey);
            await userManager.UpdateAsync(user);

            return Result.Success();
        }
    }
}