using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace GolMetrics.API.Features.FootballData;

internal sealed class ApiKeyDelegatingHandler(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var apiKey = await ResolveApiKeyAsync(cancellationToken);
        request.Headers.Remove("x-apisports-key");
        request.Headers.Add("x-apisports-key", apiKey);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> ResolveApiKeyAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();

        if (!currentUserService.IsAuthenticated)
            return GetSystemDefaultKey();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByIdAsync(currentUserService.UserId.ToString());

        if (user?.EncryptedApiKey is not null)
        {
            var encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();
            return encryptionService.Decrypt(user.EncryptedApiKey);
        }

        return GetSystemDefaultKey();
    }

    private string GetSystemDefaultKey() =>
        configuration["ApiFootball:ApiKey"]
        ?? throw new InvalidOperationException("ApiFootball:ApiKey configuration is missing.");
}