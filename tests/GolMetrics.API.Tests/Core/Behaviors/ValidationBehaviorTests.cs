using FluentAssertions;
using FluentValidation;
using GolMetrics.API.Core.Behaviors;
using MediatR;

namespace GolMetrics.API.Tests.Core.Behaviors;

[Trait("Category", "Unit")]
public class ValidationBehaviorTests
{
    public sealed record TestRequest(string Value) : IRequest<string>;

    private sealed class PassingValidator : AbstractValidator<TestRequest>;

    private sealed class FailingValidator : AbstractValidator<TestRequest>
    {
        public FailingValidator(string errorMessage)
        {
            RuleFor(x => x.Value).Must(_ => false).WithMessage(errorMessage);
        }
    }

    [Fact]
    public async Task Handle_NoValidators_CallsNextAndReturnsResponse()
    {
        var behavior = new ValidationBehavior<TestRequest, string>([]);
        var request = new TestRequest("test");

        var result = await behavior.Handle(
            request,
            (ct) => Task.FromResult("response"),
            CancellationToken.None);

        result.Should().Be("response");
    }

    [Fact]
    public async Task Handle_AllValidatorsPass_CallsNextAndReturnsResponse()
    {
        var behavior = new ValidationBehavior<TestRequest, string>([new PassingValidator()]);
        var request = new TestRequest("valid");

        var result = await behavior.Handle(
            request,
            (ct) => Task.FromResult("response"),
            CancellationToken.None);

        result.Should().Be("response");
    }

    [Fact]
    public async Task Handle_ValidationFails_ThrowsValidationException()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(
            [new FailingValidator("Value is required")]);
        var request = new TestRequest("");
        var nextCalled = false;

        var act = async () => await behavior.Handle(
            request,
            (ct) =>
            {
                nextCalled = true;
                return Task.FromResult("response");
            },
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
        nextCalled.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_MultipleValidators_AggregatesAllErrors()
    {
        var behavior = new ValidationBehavior<TestRequest, string>(
        [
            new FailingValidator("Error from validator 1"),
            new FailingValidator("Error from validator 2")
        ]);
        var request = new TestRequest("bad");

        var act = async () => await behavior.Handle(
            request,
            (ct) => Task.FromResult("response"),
            CancellationToken.None);

        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().Contain(e => e.ErrorMessage == "Error from validator 1");
        exception.Which.Errors.Should().Contain(e => e.ErrorMessage == "Error from validator 2");
    }
}