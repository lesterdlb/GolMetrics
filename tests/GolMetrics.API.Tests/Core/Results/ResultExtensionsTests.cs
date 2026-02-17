using FluentAssertions;
using GolMetrics.API.Core.Results;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GolMetrics.API.Tests.Core.Results;

[Trait("Category", "Unit")]
public class ResultExtensionsTests
{
    [Theory]
    [InlineData(ErrorCategory.BadRequest, 400)]
    [InlineData(ErrorCategory.Unauthorized, 401)]
    [InlineData(ErrorCategory.Forbidden, 403)]
    [InlineData(ErrorCategory.NotFound, 404)]
    [InlineData(ErrorCategory.Conflict, 409)]
    [InlineData(ErrorCategory.BadGateway, 502)]
    public void ToProblemDetails_MapsErrorCategory_ToCorrectStatusCode(
        ErrorCategory category, int expectedStatusCode)
    {
        var error = new Error("Test.Error", "Test message", category);
        var result = Result.Failure(error);

        var problemDetails = result.ToProblemDetails() as ProblemHttpResult;

        problemDetails.Should().NotBeNull();
        problemDetails!.StatusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void ToProblemDetails_OnSuccessfulResult_ThrowsInvalidOperationException()
    {
        var result = Result.Success();

        var act = () => result.ToProblemDetails();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ToProblemDetails_SetsErrorCodeAsTitle()
    {
        var error = new Error("Auth.InvalidCredentials", "Bad password", ErrorCategory.Unauthorized);
        var result = Result.Failure(error);

        var problemDetails = result.ToProblemDetails() as ProblemHttpResult;

        problemDetails!.ProblemDetails.Title.Should().Be("Auth.InvalidCredentials");
    }

    [Fact]
    public void ToProblemDetails_SetsErrorMessageAsDetail()
    {
        var error = new Error("Auth.InvalidCredentials", "Bad password", ErrorCategory.Unauthorized);
        var result = Result.Failure(error);

        var problemDetails = result.ToProblemDetails() as ProblemHttpResult;

        problemDetails!.ProblemDetails.Detail.Should().Be("Bad password");
    }
}