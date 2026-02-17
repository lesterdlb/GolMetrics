using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Chat;

public sealed class GetConversationMessagesIntegrationTests(PostgreSqlFixture fixture)
    : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetMessages_ValidConversation_ReturnsMessagesOrderedByTimestamp()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var createResponse = await client.PostAsJsonAsync("/api/conversations",
            new { Title = "Messages Test" });
        var conversation = await createResponse.Content.ReadFromJsonAsync<ConversationResponse>();

        await client.PostAsJsonAsync("/api/chat/message",
            new { Content = "First message", ConversationId = conversation!.Id });
        await client.PostAsJsonAsync("/api/chat/message",
            new { Content = "Second message", ConversationId = conversation.Id });

        var response = await client.GetAsync($"/api/conversations/{conversation.Id}/messages");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var messages = await response.Content.ReadFromJsonAsync<List<MessageResponse>>();
        messages.Should().HaveCount(4);
        messages![0].Role.Should().Be("User");
        messages[1].Role.Should().Be("Assistant");
        messages[2].Role.Should().Be("User");
        messages[3].Role.Should().Be("Assistant");
    }

    [Fact]
    public async Task GetMessages_NonexistentConversation_ReturnsNotFound()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.GetAsync($"/api/conversations/{Guid.NewGuid()}/messages");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetMessages_OtherUsersConversation_ReturnsNotFound()
    {
        using var client1 = await CreateAuthenticatedClientAsync("msgowner@example.com");

        var createResponse = await client1.PostAsJsonAsync("/api/conversations",
            new { Title = "Private Chat" });
        var conversation = await createResponse.Content.ReadFromJsonAsync<ConversationResponse>();

        using var client2 = await CreateAuthenticatedClientAsync("intruder@example.com");

        var response = await client2.GetAsync($"/api/conversations/{conversation!.Id}/messages");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record ConversationResponse(Guid Id, string Title);

    private sealed record MessageResponse(Guid Id, string Content, string Role, DateTime Timestamp);
}