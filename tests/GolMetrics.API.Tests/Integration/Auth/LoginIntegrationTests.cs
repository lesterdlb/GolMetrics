using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Auth;

public sealed class LoginIntegrationTests(PostgreSqlFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithTokens()
    {
        await RegisterUserAsync("login@example.com", "Password123!");

        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Email = "login@example.com", Password = "Password123!" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body.Should().NotBeNull();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsUnauthorized()
    {
        await RegisterUserAsync("login2@example.com", "Password123!");

        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Email = "login2@example.com", Password = "WrongPassword!" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_NonexistentUser_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Email = "nobody@example.com", Password = "Password123!" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}