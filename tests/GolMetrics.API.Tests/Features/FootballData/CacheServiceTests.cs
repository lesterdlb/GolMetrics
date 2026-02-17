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

    private static readonly Dictionary<string, string> DefaultParams =
        new() { ["league"] = "39", ["season"] = "2024" };

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
    public async Task GetAsync_CacheMiss_ReturnsNull()
    {
        var result = await _sut.GetAsync("fixtures", DefaultParams);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_CacheHit_ReturnsStoredValue()
    {
        await _sut.SetAsync("fixtures", DefaultParams, "cached body", TimeSpan.FromHours(1));

        var result = await _sut.GetAsync("fixtures", DefaultParams);

        result.Should().Be("cached body");
    }

    [Fact]
    public async Task GetAsync_ExpiredEntry_ReturnsNull()
    {
        await _sut.SetAsync("fixtures", DefaultParams, "old body", TimeSpan.FromMinutes(5));

        _timeProvider.Advance(TimeSpan.FromMinutes(10));

        var result = await _sut.GetAsync("fixtures", DefaultParams);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_StoresEntryInDatabase()
    {
        await _sut.SetAsync("standings", DefaultParams, "body", TimeSpan.FromHours(1));

        var entry = await _dbContext.CachedQueries.FirstOrDefaultAsync();
        entry.Should().NotBeNull();
        entry!.Endpoint.Should().Be("standings");
        entry.ResponseData.Should().Be("body");
    }

    [Fact]
    public async Task SetAsync_ExistingEntry_UpdatesValueAndExpiry()
    {
        await _sut.SetAsync("fixtures", DefaultParams, "old body", TimeSpan.FromMinutes(5));

        _timeProvider.Advance(TimeSpan.FromMinutes(10));

        await _sut.SetAsync("fixtures", DefaultParams, "new body", TimeSpan.FromMinutes(5));

        var entries = await _dbContext.CachedQueries.ToListAsync();
        entries.Should().HaveCount(1);
        entries[0].ResponseData.Should().Be("new body");
    }

    [Fact]
    public async Task GetAsync_SameParametersDifferentOrder_ProducesSameCacheKey()
    {
        var paramsA = new Dictionary<string, string> { ["season"] = "2024", ["league"] = "39" };
        var paramsB = new Dictionary<string, string> { ["league"] = "39", ["season"] = "2024" };

        await _sut.SetAsync("fixtures", paramsA, "result A", TimeSpan.FromHours(1));

        var result = await _sut.GetAsync("fixtures", paramsB);

        result.Should().Be("result A");
    }

    [Fact]
    public async Task GetAsync_DifferentEndpoints_ProduceDifferentCacheKeys()
    {
        await _sut.SetAsync("fixtures", DefaultParams, "fixtures data", TimeSpan.FromHours(1));

        var result = await _sut.GetAsync("standings", DefaultParams);

        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_SaveChangesThrows_DoesNotPropagateException()
    {
        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var failingContext = new FailingDbContext(options);
        var sut = new CacheService(failingContext, _timeProvider, NullLogger<CacheService>.Instance);

        var act = async () => await sut.SetAsync("fixtures", DefaultParams, "body", TimeSpan.FromHours(1));

        await act.Should().NotThrowAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    private sealed class FailingDbContext(DbContextOptions<GolMetricsDbContext> options)
        : GolMetricsDbContext(options)
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => throw new DbUpdateException("Simulated failure");
    }
}