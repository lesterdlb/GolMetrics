using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Chat;

public sealed class CreateConversationIntegrationTests(PostgreSqlFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task CreateConversation_Authenticated_ReturnsCreated()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/conversations",
            new { Title = "Test Conversation" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<ConversationResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().NotBeEmpty();
        body.Title.Should().Be("Test Conversation");
    }

    [Fact]
    public async Task CreateConversation_Unauthenticated_ReturnsUnauthorized()
    {
        var response = await Client.PostAsJsonAsync("/api/conversations",
            new { Title = "Test Conversation" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateConversation_EmptyTitle_ReturnsBadRequest()
    {
        using var client = await CreateAuthenticatedClientAsync("emptytitle@example.com");

        var json = JsonSerializer.Serialize(new { title = "" });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/conversations", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateConversation_TitleTooLong_ReturnsBadRequest()
    {
        using var client = await CreateAuthenticatedClientAsync("longtitle@example.com");

        var json = JsonSerializer.Serialize(new { title = new string('A', 201) });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/conversations", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private sealed record ConversationResponse(Guid Id, string Title);
}