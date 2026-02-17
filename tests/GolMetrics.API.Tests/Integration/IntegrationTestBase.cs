using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GolMetrics.API.Tests.Integration;

[Collection(IntegrationTestCollection.Name)]
[Trait("Category", "Integration")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected readonly PostgreSqlFixture Fixture;
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    protected IntegrationTestBase(PostgreSqlFixture fixture)
    {
        Fixture = fixture;
        Factory = new CustomWebApplicationFactory(fixture.ConnectionString);
        Client = Factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await Fixture.ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
    }

    protected async Task<AuthResponse> RegisterUserAsync(string email = "test@example.com",
        string password = "Password123!")
    {
        var response = await Client.PostAsJsonAsync("/api/auth/register",
            new { Email = email, Password = password });

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
        return result!;
    }

    protected async Task<HttpClient> CreateAuthenticatedClientAsync(string email = "test@example.com",
        string password = "Password123!")
    {
        var authResponse = await RegisterUserAsync(email, password);

        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResponse.AccessToken);

        return client;
    }

    protected sealed record AuthResponse(string AccessToken, string RefreshToken, DateTime ExpiresAtUtc);
}