using FluentAssertions;
using GolMetrics.API.Features.Chat;

namespace GolMetrics.API.Tests.Features.Chat;

[Trait("Category", "Unit")]
public class SendMessageValidatorTests
{
    private readonly SendMessage.Validator _sut = new();

    [Fact]
    public async Task Validate_ValidContent_PassesValidation()
    {
        var command = new SendMessage.Command("What are the Premier League standings?");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyContent_FailsValidation()
    {
        var command = new SendMessage.Command("");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }

    [Fact]
    public async Task Validate_ContentExceeding4000Characters_FailsValidation()
    {
        var command = new SendMessage.Command(new string('a', 4001));

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }
}