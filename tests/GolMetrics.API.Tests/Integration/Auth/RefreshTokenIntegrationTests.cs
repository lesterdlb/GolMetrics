using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Auth;

public sealed class RefreshTokenIntegrationTests(PostgreSqlFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task RefreshToken_ValidToken_ReturnsOkWithNewTokens()
    {
        var auth = await RegisterUserAsync("refresh@example.com");

        var response = await Client.PostAsJsonAsync("/api/auth/refresh-token",
            new { Token = auth.RefreshToken });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body.Should().NotBeNull();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBe(auth.RefreshToken);
    }

    [Fact]
    public async Task RefreshToken_InvalidToken_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/refresh-token",
            new { Token = "invalid-refresh-token" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_RevokedToken_ReturnsUnauthorized()
    {
        var auth = await RegisterUserAsync("revoke@example.com");

        await Client.PostAsJsonAsync("/api/auth/refresh-token",
            new { Token = auth.RefreshToken });

        var response = await Client.PostAsJsonAsync("/api/auth/refresh-token",
            new { Token = auth.RefreshToken });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}