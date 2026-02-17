using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Moq;

namespace GolMetrics.API.Tests.Integration;

public sealed class CustomWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    public const string TestSemanticKernelResponse = "This is a test AI response.";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = connectionString,
                ["TokenOptions:SecretKey"] = "SuperSecretTestKeyThatIsLongEnoughForHmacSha256!",
                ["TokenOptions:Issuer"] = "GolMetrics",
                ["TokenOptions:Audience"] = "GolMetrics",
                ["TokenOptions:ExpirationMinutes"] = "60",
                ["Gemini:ApiKey"] = "test-gemini-key",
                ["Gemini:ModelId"] = "gemini-2.0-flash",
                ["Encryption:Key"] = "0123456789abcdef0123456789abcdef",
                ["ApiFootball:BaseUrl"] = "https://v3.football.api-sports.io",
                ["ApiFootball:ApiKey"] = "test-football-key"
            });
        });

        builder.ConfigureServices(services =>
        {
            ReplaceDbContext(services);
            ReplaceKernel(services);
            ReplaceSemanticKernelService(services);
            ReplaceFootballApiClient(services);
        });
    }

    private void ReplaceDbContext(IServiceCollection services)
    {
        var dbDescriptor =
            services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GolMetricsDbContext>));

        if (dbDescriptor is not null)
            services.Remove(dbDescriptor);

        var interceptorDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AuditableEntityInterceptor));

        if (interceptorDescriptor is not null)
            services.Remove(interceptorDescriptor);

        services.AddScoped<AuditableEntityInterceptor>();
        services.AddDbContext<GolMetricsDbContext>((sp, options) =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>()));
    }

    private static void ReplaceKernel(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Kernel));

        if (descriptor is not null)
            services.Remove(descriptor);

        services.AddSingleton(new Kernel());
    }

    private static void ReplaceSemanticKernelService(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ISemanticKernelService));

        if (descriptor is not null)
            services.Remove(descriptor);

        var mock = new Mock<ISemanticKernelService>();
        mock.Setup(x => x.ProcessMessageAsync(
                It.IsAny<string>(),
                It.IsAny<IReadOnlyList<GolMetrics.API.Features.Chat.Message>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestSemanticKernelResponse);

        services.AddScoped(_ => mock.Object);
    }

    private static void ReplaceFootballApiClient(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IFootballApiClient));

        if (descriptor is not null)
            services.Remove(descriptor);

        var mock = new Mock<IFootballApiClient>();
        mock.Setup(x => x.ValidateApiKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        services.AddScoped(_ => mock.Object);
    }
}