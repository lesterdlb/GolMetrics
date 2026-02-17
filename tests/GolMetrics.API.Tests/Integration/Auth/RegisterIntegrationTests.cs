using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Auth;

public sealed class RegisterIntegrationTests(PostgreSqlFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Register_ValidCredentials_ReturnsCreatedWithTokens()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/register",
            new { Email = "newuser@example.com", Password = "Password123!" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body.Should().NotBeNull();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_DuplicateEmail_ReturnsConflict()
    {
        await RegisterUserAsync("duplicate@example.com");

        var response = await Client.PostAsJsonAsync("/api/auth/register",
            new { Email = "duplicate@example.com", Password = "Password123!" });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Register_EmptyEmail_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/register",
            new { Email = "", Password = "Password123!" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShortPassword_ReturnsBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/register",
            new { Email = "user@example.com", Password = "12345" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}