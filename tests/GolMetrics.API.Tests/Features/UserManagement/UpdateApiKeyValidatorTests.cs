using FluentAssertions;
using GolMetrics.API.Features.UserManagement;

namespace GolMetrics.API.Tests.Features.UserManagement;

[Trait("Category", "Unit")]
public class UpdateApiKeyValidatorTests
{
    private readonly UpdateApiKey.Validator _sut = new();

    [Fact]
    public async Task Validate_ValidApiKey_PassesValidation()
    {
        var command = new UpdateApiKey.Command("valid-api-key-123");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyApiKey_FailsValidation()
    {
        var command = new UpdateApiKey.Command("");

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ApiKey");
    }
}