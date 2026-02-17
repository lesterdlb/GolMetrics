using FluentAssertions;
using GolMetrics.API.Core.Results;

namespace GolMetrics.API.Tests.Core.Results;

[Trait("Category", "Unit")]
public class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessfulResult()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_ReturnsFailedResult()
    {
        var error = new Error("Test.Error", "Something went wrong", ErrorCategory.BadRequest);

        var result = Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void GenericFailure_ReturnsFailedResultWithDefaultValue()
    {
        var error = new Error("Test.Error", "Not found", ErrorCategory.NotFound);

        var result = Result<string>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccessfulResult()
    {
        Result<string> result = "hello";

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("hello");
        result.Error.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromInt_CreatesSuccessfulResult()
    {
        Result<int> result = 42;

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void GenericFailure_ValueType_ReturnsDefault()
    {
        var error = new Error("Test.Error", "Bad", ErrorCategory.BadRequest);

        var result = Result<int>.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.Value.Should().Be(0);
    }
}