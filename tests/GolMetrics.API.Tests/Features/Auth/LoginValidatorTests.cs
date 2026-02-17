using FluentAssertions;
using GolMetrics.API.Features.Auth;

namespace GolMetrics.API.Tests.Features.Auth;

[Trait("Category", "Unit")]
public class LoginValidatorTests
{
    private readonly Login.Validator _sut = new();

    [Fact]
    public async Task Validate_ValidCommand_PassesValidation()
    {
        var command = new Login.Command("user@example.com", "Password123!");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyEmail_FailsValidation()
    {
        var command = new Login.Command("", "Password123!");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Validate_InvalidEmailFormat_FailsValidation()
    {
        var command = new Login.Command("not-an-email", "Password123!");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Validate_EmptyPassword_FailsValidation()
    {
        var command = new Login.Command("user@example.com", "");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}