using FluentAssertions;
using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Tests.Core.Results;

[Trait("Category", "Unit")]
public class ErrorTests
{
    [Fact]
    public void Errors_WithSameValues_AreEqual()
    {
        var error1 = new Error("Test.Error", "message", ErrorCategory.BadRequest);
        var error2 = new Error("Test.Error", "message", ErrorCategory.BadRequest);

        error1.Should().Be(error2);
    }

    [Fact]
    public void Errors_WithDifferentCode_AreNotEqual()
    {
        var error1 = new Error("Error.One", "message", ErrorCategory.BadRequest);
        var error2 = new Error("Error.Two", "message", ErrorCategory.BadRequest);

        error1.Should().NotBe(error2);
    }

    [Fact]
    public void Errors_WithDifferentMessage_AreNotEqual()
    {
        var error1 = new Error("Test.Error", "message one", ErrorCategory.BadRequest);
        var error2 = new Error("Test.Error", "message two", ErrorCategory.BadRequest);

        error1.Should().NotBe(error2);
    }

    [Fact]
    public void Errors_WithDifferentCategory_AreNotEqual()
    {
        var error1 = new Error("Test.Error", "message", ErrorCategory.BadRequest);
        var error2 = new Error("Test.Error", "message", ErrorCategory.NotFound);

        error1.Should().NotBe(error2);
    }

    [Fact]
    public void Error_Properties_ReturnCorrectValues()
    {
        var error = new Error("Auth.Invalid", "Invalid credentials", ErrorCategory.Unauthorized);

        error.Code.Should().Be("Auth.Invalid");
        error.Message.Should().Be("Invalid credentials");
        error.Category.Should().Be(ErrorCategory.Unauthorized);
    }
}