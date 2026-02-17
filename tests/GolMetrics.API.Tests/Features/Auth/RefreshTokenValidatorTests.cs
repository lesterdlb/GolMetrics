using FluentAssertions;
using GolMetrics.API.Features.Auth;

namespace GolMetrics.API.Tests.Features.Auth;

[Trait("Category", "Unit")]
public class RefreshTokenValidatorTests
{
    private readonly RefreshToken.Validator _sut = new();

    [Fact]
    public async Task Validate_ValidToken_PassesValidation()
    {
        var command = new RefreshToken.Command("valid-refresh-token");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyToken_FailsValidation()
    {
        var command = new RefreshToken.Command("");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Token");
    }
}