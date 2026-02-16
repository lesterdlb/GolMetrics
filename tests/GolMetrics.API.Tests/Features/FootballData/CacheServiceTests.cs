using FluentAssertions;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.FootballData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace GolMetrics.API.Tests.Features.FootballData;

[Trait("Category", "Unit")]
public class CacheServiceTests : IDisposable
{
    private readonly GolMetricsDbContext _dbContext;
    private readonly FakeTimeProvider _timeProvider;
    private readonly CacheService _sut;

    public CacheServiceTests()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new GolMetricsDbContext(options);
        _timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.Zero));
        _sut = new CacheService(_dbContext, _timeProvider, NullLogger<CacheService>.Instance);
    }

    [Fact]
    public async Task GetOrSetAsync_CacheHit_ReturnsStoredDataWithoutCallingFetchFactory()
    {
        var endpoint = "fixtures";
        var parameters = new Dictionary<string, string> { ["league"] = "39", ["season"] = "2024" };
        var storedData = new TestResponse("cached result");

        await _sut.GetOrSetAsync(endpoint, parameters, () => Task.FromResult(storedData), TimeSpan.FromHours(1));

        var fetchFactoryCalled = false;
        var result = await _sut.GetOrSetAsync(endpoint, parameters, () =>
        {
            fetchFactoryCalled = true;
            return Task.FromResult(new TestResponse("fresh result"));
        }, TimeSpan.FromHours(1));

        result.Value.Should().Be("cached result");
        fetchFactoryCalled.Should().BeFalse();
    }

    [Fact]
    public async Task GetOrSetAsync_CacheMiss_InvokesFetchFactoryAndStoresResult()
    {
        var endpoint = "standings";
        var parameters = new Dictionary<string, string> { ["league"] = "39" };
        var fetchFactoryCalled = false;

        var result = await _sut.GetOrSetAsync(endpoint, parameters, () =>
        {
            fetchFactoryCalled = true;
            return Task.FromResult(new TestResponse("fetched data"));
        }, TimeSpan.FromHours(1));

        result.Value.Should().Be("fetched data");
        fetchFactoryCalled.Should().BeTrue();

        var cached = await _dbContext.CachedQueries.FirstOrDefaultAsync();
        cached.Should().NotBeNull();
        cached!.Endpoint.Should().Be("standings");
    }

    [Fact]
    public async Task GetOrSetAsync_ExpiredEntry_RefreshesAndUpdatesStoredData()
    {
        var endpoint = "fixtures";
        var parameters = new Dictionary<string, string> { ["league"] = "39" };

        await _sut.GetOrSetAsync(endpoint, parameters,
            () => Task.FromResult(new TestResponse("old data")), TimeSpan.FromMinutes(5));

        _timeProvider.Advance(TimeSpan.FromMinutes(10));

        var result = await _sut.GetOrSetAsync(endpoint, parameters,
            () => Task.FromResult(new TestResponse("new data")), TimeSpan.FromMinutes(5));

        result.Value.Should().Be("new data");

        var entries = await _dbContext.CachedQueries.ToListAsync();
        entries.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetOrSetAsync_SameParametersDifferentOrder_ProducesSameCacheKey()
    {
        var endpoint = "fixtures";
        var paramsA = new Dictionary<string, string> { ["season"] = "2024", ["league"] = "39" };
        var paramsB = new Dictionary<string, string> { ["league"] = "39", ["season"] = "2024" };

        await _sut.GetOrSetAsync(endpoint, paramsA,
            () => Task.FromResult(new TestResponse("result A")), TimeSpan.FromHours(1));

        var fetchCalled = false;
        var result = await _sut.GetOrSetAsync(endpoint, paramsB, () =>
        {
            fetchCalled = true;
            return Task.FromResult(new TestResponse("result B"));
        }, TimeSpan.FromHours(1));

        fetchCalled.Should().BeFalse();
        result.Value.Should().Be("result A");
    }

    [Fact]
    public async Task GetOrSetAsync_DifferentEndpoints_ProduceDifferentCacheKeys()
    {
        var parameters = new Dictionary<string, string> { ["league"] = "39" };

        await _sut.GetOrSetAsync("fixtures", parameters,
            () => Task.FromResult(new TestResponse("fixtures data")), TimeSpan.FromHours(1));

        var fetchCalled = false;
        var result = await _sut.GetOrSetAsync("standings", parameters, () =>
        {
            fetchCalled = true;
            return Task.FromResult(new TestResponse("standings data"));
        }, TimeSpan.FromHours(1));

        fetchCalled.Should().BeTrue();
        result.Value.Should().Be("standings data");
    }

    [Fact]
    public async Task GetOrSetAsync_SaveChangesThrows_ReturnsFetchedDataAnyway()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var failingContext = new FailingDbContext(options);
        var sut = new CacheService(failingContext, _timeProvider, NullLogger<CacheService>.Instance);

        var result = await sut.GetOrSetAsync("fixtures",
            new Dictionary<string, string> { ["league"] = "39" },
            () => Task.FromResult(new TestResponse("api data")),
            TimeSpan.FromHours(1));

        result.Value.Should().Be("api data");
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    private sealed record TestResponse(string Value);

    private sealed class FailingDbContext(DbContextOptions<GolMetricsDbContext> options)
        : GolMetricsDbContext(options)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => throw new DbUpdateException("Simulated failure");
    }
}