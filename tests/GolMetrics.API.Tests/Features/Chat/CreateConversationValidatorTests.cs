using FluentAssertions;
using GolMetrics.API.Features.Chat;

namespace GolMetrics.API.Tests.Features.Chat;

[Trait("Category", "Unit")]
public class CreateConversationValidatorTests
{
    private readonly CreateConversation.Validator _sut = new();

    [Fact]
    public async Task Validate_ValidTitle_PassesValidation()
    {
        var command = new CreateConversation.Command("My Conversation");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyTitle_FailsValidation()
    {
        var command = new CreateConversation.Command("");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validate_TitleExceeding200Characters_FailsValidation()
    {
        var command = new CreateConversation.Command(new string('a', 201));

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }
}