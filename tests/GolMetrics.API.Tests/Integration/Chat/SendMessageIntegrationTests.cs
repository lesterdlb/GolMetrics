using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Chat;

public sealed class SendMessageIntegrationTests(PostgreSqlFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task SendMessage_ExistingConversation_ReturnsOkWithAiResponse()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var createResponse = await client.PostAsJsonAsync("/api/conversations",
            new { Title = "Test Chat" });
        var conversation = await createResponse.Content.ReadFromJsonAsync<ConversationResponse>();

        var response = await client.PostAsJsonAsync("/api/chat/message",
            new { Content = "Hello AI", ConversationId = conversation!.Id });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<MessageResponse>();
        body.Should().NotBeNull();
        body!.Content.Should().Be(CustomWebApplicationFactory.TestSemanticKernelResponse);
        body.ConversationId.Should().Be(conversation.Id);
    }

    [Fact]
    public async Task SendMessage_NoConversationId_AutoCreatesConversation()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/chat/message",
            new { Content = "Hello without conversation" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<MessageResponse>();
        body.Should().NotBeNull();
        body!.ConversationId.Should().NotBeEmpty();

        var conversationsResponse = await client.GetAsync("/api/conversations");
        var conversations = await conversationsResponse.Content
            .ReadFromJsonAsync<List<ConversationListItem>>();
        conversations.Should().HaveCount(1);
    }

    [Fact]
    public async Task SendMessage_NonexistentConversation_ReturnsNotFound()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/chat/message",
            new { Content = "Hello", ConversationId = Guid.NewGuid() });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SendMessage_OtherUsersConversation_ReturnsNotFound()
    {
        using var client1 = await CreateAuthenticatedClientAsync("user1@example.com");

        var createResponse = await client1.PostAsJsonAsync("/api/conversations",
            new { Title = "User1 Chat" });
        var conversation = await createResponse.Content.ReadFromJsonAsync<ConversationResponse>();

        using var client2 = await CreateAuthenticatedClientAsync("user2@example.com");

        var response = await client2.PostAsJsonAsync("/api/chat/message",
            new { Content = "Trying to access", ConversationId = conversation!.Id });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record ConversationResponse(Guid Id, string Title);

    private sealed record MessageResponse(string Content, Guid ConversationId);

    private sealed record ConversationListItem(Guid Id, string Title, DateTime CreatedAt, DateTime? UpdatedAt);
}