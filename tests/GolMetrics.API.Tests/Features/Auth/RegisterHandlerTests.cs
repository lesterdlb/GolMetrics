using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.Auth;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace GolMetrics.API.Tests.Features.Auth;

[Trait("Category", "Unit")]
public class RegisterHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Register.Handler _sut;

    public RegisterHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _tokenServiceMock = new Mock<ITokenService>();
        _sut = new Register.Handler(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessWithTokens()
    {
        var command = new Register.Command("test@example.com", "Password123!");
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);
        _tokenServiceMock
            .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("access-token");
        _tokenServiceMock
            .Setup(x => x.GenerateRefreshTokenAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("refresh-token");

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("refresh-token");
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ReturnsDuplicateEmailError()
    {
        var command = new Register.Command("test@example.com", "Password123!");
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail" }));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.DuplicateEmail);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ReturnsInvalidPasswordError()
    {
        var command = new Register.Command("test@example.com", "weak");
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), command.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "PasswordTooShort" }));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AuthErrors.InvalidPassword);
    }
}