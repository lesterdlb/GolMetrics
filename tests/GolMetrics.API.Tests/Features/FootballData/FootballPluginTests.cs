using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.FootballData;
using Moq;

namespace GolMetrics.API.Tests.Features.FootballData;

[Trait("Category", "Unit")]
public class FootballPluginTests
{
    private readonly Mock<IFootballApiClient> _footballApiClientMock = new();
    private readonly FootballPlugin _sut;

    public FootballPluginTests()
    {
        _sut = new FootballPlugin(_footballApiClientMock.Object);
    }

    [Fact]
    public async Task GetTopScorersAsync_Success_ReturnsResponseData()
    {
        _footballApiClientMock
            .Setup(c => c.GetTopScorersAsync(39, 2024, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<string>)"""{"response":[]}""");

        var result = await _sut.GetTopScorersAsync(39, 2024);

        result.Should().Be("""{"response":[]}""");
        _footballApiClientMock.Verify(c => c.GetTopScorersAsync(39, 2024, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTopScorersAsync_Failure_ReturnsErrorMessage()
    {
        _footballApiClientMock
            .Setup(c => c.GetTopScorersAsync(39, 2024, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<string>.Failure(FootballErrors.RateLimitExceeded));

        var result = await _sut.GetTopScorersAsync(39, 2024);

        result.Should().Be(FootballErrors.RateLimitExceeded.Message);
    }

    [Fact]
    public async Task GetStandingsAsync_Success_ReturnsResponseData()
    {
        _footballApiClientMock
            .Setup(c => c.GetStandingsAsync(140, 2024, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<string>)"""{"response":[]}""");

        var result = await _sut.GetStandingsAsync(140, 2024);

        result.Should().Be("""{"response":[]}""");
        _footballApiClientMock.Verify(c => c.GetStandingsAsync(140, 2024, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetStandingsAsync_Failure_ReturnsErrorMessage()
    {
        _footballApiClientMock
            .Setup(c => c.GetStandingsAsync(140, 2024, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<string>.Failure(FootballErrors.ApiUnavailable));

        var result = await _sut.GetStandingsAsync(140, 2024);

        result.Should().Be(FootballErrors.ApiUnavailable.Message);
    }

    [Fact]
    public async Task GetRecentResultsAsync_Success_ReturnsResponseData()
    {
        _footballApiClientMock
            .Setup(c => c.GetRecentResultsAsync(33, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<string>)"""{"response":[]}""");

        var result = await _sut.GetRecentResultsAsync(33, 5);

        result.Should().Be("""{"response":[]}""");
        _footballApiClientMock.Verify(c => c.GetRecentResultsAsync(33, 5, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetRecentResultsAsync_Failure_ReturnsErrorMessage()
    {
        _footballApiClientMock
            .Setup(c => c.GetRecentResultsAsync(33, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<string>.Failure(FootballErrors.InvalidParameters));

        var result = await _sut.GetRecentResultsAsync(33, 5);

        result.Should().Be(FootballErrors.InvalidParameters.Message);
    }

    [Fact]
    public async Task GetUpcomingMatchesAsync_Success_ReturnsResponseData()
    {
        _footballApiClientMock
            .Setup(c => c.GetUpcomingMatchesAsync(39, null, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<string>)"""{"response":[]}""");

        var result = await _sut.GetUpcomingMatchesAsync(39);

        result.Should().Be("""{"response":[]}""");
    }

    [Fact]
    public async Task GetUpcomingMatchesAsync_WithTeamId_PassesTeamIdToClient()
    {
        _footballApiClientMock
            .Setup(c => c.GetUpcomingMatchesAsync(39, 33, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<string>)"""{"response":[]}""");

        var result = await _sut.GetUpcomingMatchesAsync(39, 33);

        result.Should().Be("""{"response":[]}""");
        _footballApiClientMock.Verify(
            c => c.GetUpcomingMatchesAsync(39, 33, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTeamStatisticsAsync_Success_ReturnsResponseData()
    {
        _footballApiClientMock
            .Setup(c => c.GetTeamStatisticsAsync(33, 39, 2024, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<string>)"""{"response":{}}""");

        var result = await _sut.GetTeamStatisticsAsync(33, 39, 2024);

        result.Should().Be("""{"response":{}}""");
        _footballApiClientMock.Verify(
            c => c.GetTeamStatisticsAsync(33, 39, 2024, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTeamStatisticsAsync_Failure_ReturnsErrorMessage()
    {
        _footballApiClientMock
            .Setup(c => c.GetTeamStatisticsAsync(33, 39, 2024, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<string>.Failure(FootballErrors.AiServiceUnavailable));

        var result = await _sut.GetTeamStatisticsAsync(33, 39, 2024);

        result.Should().Be(FootballErrors.AiServiceUnavailable.Message);
    }
}