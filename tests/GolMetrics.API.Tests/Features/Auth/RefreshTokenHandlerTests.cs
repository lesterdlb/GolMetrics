using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Core.Results;
using GolMetrics.API.Features.Auth;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace GolMetrics.API.Tests.Features.Auth;

[Trait("Category", "Unit")]
public class RefreshTokenHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly RefreshToken.Handler _sut;

    public RefreshTokenHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _tokenServiceMock = new Mock<ITokenService>();
        _sut = new RefreshToken.Handler(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidToken_ReturnsNewTokenPair()
    {
        var user = CreateUser();
        var command = new RefreshToken.Command("valid-refresh-token");

        _tokenServiceMock
            .Setup(x => x.ValidateRefreshTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user.Id);
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        _tokenServiceMock
            .Setup(x => x.RevokeRefreshTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _tokenServiceMock
            .Setup(x => x.GenerateAccessToken(user))
            .Returns("new-access-token");
        _tokenServiceMock
            .Setup(x => x.GenerateRefreshTokenAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync("new-refresh-token");

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.AccessToken.Should().Be("new-access-token");
        result.Value.RefreshToken.Should().Be("new-refresh-token");
        _tokenServiceMock.Verify(x => x.RevokeRefreshTokenAsync(command.Token, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidToken_ReturnsInvalidRefreshTokenError()
    {
        var command = new RefreshToken.Command("invalid-token");

        _tokenServiceMock
            .Setup(x => x.ValidateRefreshTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Failure(AuthErrors.InvalidRefreshToken));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidRefreshToken);
    }

    [Fact]
    public async Task Handle_ExpiredToken_ReturnsInvalidRefreshTokenError()
    {
        var command = new RefreshToken.Command("expired-token");

        _tokenServiceMock
            .Setup(x => x.ValidateRefreshTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Failure(AuthErrors.InvalidRefreshToken));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidRefreshToken);
    }

    [Fact]
    public async Task Handle_RevokedToken_ReturnsInvalidRefreshTokenError()
    {
        var command = new RefreshToken.Command("revoked-token");

        _tokenServiceMock
            .Setup(x => x.ValidateRefreshTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Failure(AuthErrors.InvalidRefreshToken));

        var result = await _sut.Handle(command, CancellationToken.None);

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
}