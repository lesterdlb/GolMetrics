using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.Auth;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace GolMetrics.API.Tests.Features.Auth;

[Trait("Category", "Unit")]
public class LoginHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Login.Handler _sut;

    public LoginHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);

        _tokenServiceMock = new Mock<ITokenService>();
        _sut = new Login.Handler(_userManagerMock.Object, _signInManagerMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSuccessWithTokens()
    {
        var user = CreateUser();
        var command = new Login.Command("test@example.com", "Password123!");

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);
        _signInManagerMock
            .Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
            .ReturnsAsync(SignInResult.Success);
        _tokenServiceMock
            .Setup(x => x.GenerateAccessToken(user))
            .Returns("access-token");
        _tokenServiceMock
            .Setup(x => x.GenerateRefreshTokenAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync("refresh-token");

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("refresh-token");
    }

    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsInvalidCredentialsError()
    {
        var user = CreateUser();
        var command = new Login.Command("test@example.com", "wrong-password");

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);
        _signInManagerMock
            .Setup(x => x.CheckPasswordSignInAsync(user, command.Password, false))
            .ReturnsAsync(SignInResult.Failed);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidCredentials);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsInvalidCredentialsError()
    {
        var command = new Login.Command("unknown@example.com", "Password123!");

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidCredentials);
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