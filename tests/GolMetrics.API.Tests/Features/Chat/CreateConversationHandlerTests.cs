using FluentAssertions;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.Chat;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Tests.Features.Chat;

[Trait("Category", "Unit")]
public class CreateConversationHandlerTests : IDisposable
{
    private readonly GolMetricsDbContext _dbContext;
    private readonly CreateConversation.Handler _sut;

    public CreateConversationHandlerTests()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new GolMetricsDbContext(options);
        _sut = new CreateConversation.Handler(_dbContext);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesConversationAndReturnsResponse()
    {
        var userId = Guid.NewGuid();
        var command = new CreateConversation.Command("Premier League Stats", userId);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Title.Should().Be("Premier League Stats");
        result.Value.Id.Should().NotBeEmpty();

        var saved = await _dbContext.Conversations.FirstOrDefaultAsync();
        saved.Should().NotBeNull();
        saved!.Title.Should().Be("Premier League Stats");
        saved.UserId.Should().Be(userId);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}