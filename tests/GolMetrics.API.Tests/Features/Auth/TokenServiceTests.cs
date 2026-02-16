using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentAssertions;
using GolMetrics.API.Core.Identity;
using GolMetrics.API.Core.Persistence;
using GolMetrics.API.Features.Auth;
using GolMetrics.API.Features.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GolMetrics.API.Tests.Features.Auth;

[Trait("Category", "Unit")]
public class TokenServiceTests : IDisposable
{
    private const string SecretKey = "super-secret-key-for-testing-that-is-long-enough-for-hmac-sha256";
    private const string Issuer = "test-issuer";
    private const string Audience = "test-audience";

    private readonly GolMetricsDbContext _dbContext;
    private readonly TokenService _sut;

    public TokenServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["TokenOptions:SecretKey"] = SecretKey,
                ["TokenOptions:Issuer"] = Issuer,
                ["TokenOptions:Audience"] = Audience
            })
            .Build();

        var options = new DbContextOptionsBuilder<GolMetricsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new GolMetricsDbContext(options);
        _sut = new TokenService(configuration, _dbContext);
    }

    [Fact]
    public void GenerateAccessToken_ValidUser_ReturnsJwtWithExpectedClaims()
    {
        var user = CreateUser();

        var token = _sut.GenerateAccessToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
        jwt.Claims.Where(c => c.Type == "permissions").Select(c => c.Value)
            .Should().BeEquivalentTo("conversations:read", "conversations:write", "user:read", "user:write");
    }

    [Fact]
    public void GenerateAccessToken_ValidUser_TokenExpiresIn7Days()
    {
        var user = CreateUser();

        var token = _sut.GenerateAccessToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void GenerateAccessToken_ValidUser_TokenIsSignedWithHmacSha256()
    {
        var user = CreateUser();

        var token = _sut.GenerateAccessToken(user);

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        };

        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

        validatedToken.Should().BeOfType<JwtSecurityToken>();
        ((JwtSecurityToken)validatedToken).Header.Alg.Should().Be(SecurityAlgorithms.HmacSha256);
        principal.Should().NotBeNull();
    }

    [Fact]
    public async Task GenerateRefreshTokenAsync_ValidUserId_PersistsTokenToDatabase()
    {
        var userId = Guid.NewGuid();

        var token = await _sut.GenerateRefreshTokenAsync(userId);

        token.Should().NotBeNullOrEmpty();

        var stored = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        stored.Should().NotBeNull();
        stored!.UserId.Should().Be(userId);
        stored.IsRevoked.Should().BeFalse();
        stored.ExpiresAtUtc.Should().BeCloseTo(DateTime.UtcNow.AddDays(30), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task ValidateRefreshTokenAsync_ValidToken_ReturnsUserId()
    {
        var userId = Guid.NewGuid();
        var token = await _sut.GenerateRefreshTokenAsync(userId);

        var result = await _sut.ValidateRefreshTokenAsync(token);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(userId);
    }

    [Fact]
    public async Task ValidateRefreshTokenAsync_RevokedToken_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        var token = await _sut.GenerateRefreshTokenAsync(userId);
        await _sut.RevokeRefreshTokenAsync(token);

        var result = await _sut.ValidateRefreshTokenAsync(token);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidRefreshToken);
    }

    [Fact]
    public async Task ValidateRefreshTokenAsync_NonExistentToken_ReturnsFailure()
    {
        var result = await _sut.ValidateRefreshTokenAsync("non-existent-token");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidRefreshToken);
    }

    [Fact]
    public async Task ValidateRefreshTokenAsync_ExpiredToken_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        var entity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            Token = "expired-token",
            UserId = userId,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(-1),
            IsRevoked = false,
            CreatedBy = userId,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-31)
        };
        _dbContext.RefreshTokens.Add(entity);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.ValidateRefreshTokenAsync("expired-token");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidRefreshToken);
    }

    private static User CreateUser() => new()
    {
        Id = Guid.NewGuid(),
        Email = "test@example.com",
        UserName = "test@example.com",
        CreatedBy = Guid.Empty,
        CreatedAtUtc = DateTime.UtcNow
    };

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}