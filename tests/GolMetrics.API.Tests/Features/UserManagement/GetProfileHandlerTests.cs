using FluentAssertions;
using GolMetrics.API.Features.UserManagement;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace GolMetrics.API.Tests.Features.UserManagement;

[Trait("Category", "Unit")]
public class GetProfileHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly GetProfile.Handler _sut;

    public GetProfileHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _sut = new GetProfile.Handler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingUser_ReturnsProfile()
    {
        var user = CreateUser();
        user.EncryptedApiKey = "encrypted-key";

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var result = await _sut.Handle(new GetProfile.Query(user.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(user.Id);
        result.Value.Email.Should().Be(user.Email);
        result.Value.HasApiKey.Should().BeTrue();
        result.Value.CreatedAt.Should().Be(user.CreatedAtUtc);
    }

    [Fact]
    public async Task Handle_ExistingUserWithoutApiKey_ReturnsHasApiKeyFalse()
    {
        var user = CreateUser();

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var result = await _sut.Handle(new GetProfile.Query(user.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.HasApiKey.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsUserNotFoundError()
    {
        var userId = Guid.NewGuid();

        _userManagerMock
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((User?)null);

        var result = await _sut.Handle(new GetProfile.Query(userId), CancellationToken.None);

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