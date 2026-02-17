using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace GolMetrics.API.Tests.Integration.Chat;

public sealed class GetConversationsIntegrationTests(PostgreSqlFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetConversations_WithConversations_ReturnsOrderedList()
    {
        using var client = await CreateAuthenticatedClientAsync();

        await client.PostAsJsonAsync("/api/conversations", new { Title = "First" });
        await client.PostAsJsonAsync("/api/conversations", new { Title = "Second" });

        var response = await client.GetAsync("/api/conversations");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var conversations = await response.Content.ReadFromJsonAsync<List<ConversationResponse>>();
        conversations.Should().HaveCount(2);
        conversations![0].Title.Should().Be("Second");
        conversations[1].Title.Should().Be("First");
    }

    [Fact]
    public async Task GetConversations_NoConversations_ReturnsEmptyList()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.GetAsync("/api/conversations");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var conversations = await response.Content.ReadFromJsonAsync<List<ConversationResponse>>();
        conversations.Should().BeEmpty();
    }

    [Fact]
    public async Task GetConversations_DoesNotReturnOtherUsersConversations()
    {
        using var client1 = await CreateAuthenticatedClientAsync("owner@example.com");
        await client1.PostAsJsonAsync("/api/conversations", new { Title = "Owner Chat" });

        using var client2 = await CreateAuthenticatedClientAsync("other@example.com");

        var response = await client2.GetAsync("/api/conversations");

        var conversations = await response.Content.ReadFromJsonAsync<List<ConversationResponse>>();
        conversations.Should().BeEmpty();
    }

    private sealed record ConversationResponse(Guid Id, string Title, DateTime CreatedAt, DateTime? UpdatedAt);
}