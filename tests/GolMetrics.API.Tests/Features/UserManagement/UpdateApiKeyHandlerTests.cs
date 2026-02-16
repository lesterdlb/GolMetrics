using FluentAssertions;
using GolMetrics.API.Core.Abstractions;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace GolMetrics.API.Tests.Features.UserManagement;

[Trait("Category", "Unit")]
public class UpdateApiKeyHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IFootballApiClient> _footballApiClientMock;
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly UpdateApiKey.Handler _sut;

    public UpdateApiKeyHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _footballApiClientMock = new Mock<IFootballApiClient>();
        _encryptionServiceMock = new Mock<IEncryptionService>();

        _sut = new UpdateApiKey.Handler(
            _userManagerMock.Object,
            _footballApiClientMock.Object,
            _encryptionServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidApiKey_EncryptsAndStoresKey()
    {
        var user = CreateUser();
        var command = new UpdateApiKey.Command("valid-api-key") { UserId = user.Id };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        _footballApiClientMock
            .Setup(x => x.ValidateApiKeyAsync("valid-api-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _encryptionServiceMock
            .Setup(x => x.Encrypt("valid-api-key"))
            .Returns("encrypted-value");
        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        user.EncryptedApiKey.Should().Be("encrypted-value");
        _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidApiKey_ReturnsInvalidApiKeyError()
    {
        var user = CreateUser();
        var command = new UpdateApiKey.Command("invalid-key") { UserId = user.Id };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        _footballApiClientMock
            .Setup(x => x.ValidateApiKeyAsync("invalid-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.InvalidApiKey);
    }

    [Fact]
    public async Task Handle_ApiValidationThrowsHttpRequestException_ReturnsApiValidationUnavailable()
    {
        var user = CreateUser();
        var command = new UpdateApiKey.Command("some-key") { UserId = user.Id };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        _footballApiClientMock
            .Setup(x => x.ValidateApiKeyAsync("some-key", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.ApiValidationUnavailable);
    }

    [Fact]
    public async Task Handle_ApiValidationThrowsTaskCanceledException_ReturnsApiValidationUnavailable()
    {
        var user = CreateUser();
        var command = new UpdateApiKey.Command("some-key") { UserId = user.Id };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        _footballApiClientMock
            .Setup(x => x.ValidateApiKeyAsync("some-key", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException("Request timed out"));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.ApiValidationUnavailable);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsUserNotFoundError()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateApiKey.Command("some-key") { UserId = userId };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((User?)null);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.UserNotFound);
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