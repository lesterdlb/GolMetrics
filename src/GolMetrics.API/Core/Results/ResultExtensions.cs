namespace GolMetrics.API.Core.Results;

public static class ResultExtensions
{
    extension(Result result)
    {
        public IResult ToProblemDetails()
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Cannot convert a successful result to ProblemDetails.");

            var error = result.Error!;
            var statusCode = error.Category switch
            {
                ErrorCategory.BadRequest => StatusCodes.Status400BadRequest,
                ErrorCategory.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorCategory.Forbidden => StatusCodes.Status403Forbidden,
                ErrorCategory.NotFound => StatusCodes.Status404NotFound,
                ErrorCategory.Conflict => StatusCodes.Status409Conflict,
                ErrorCategory.BadGateway => StatusCodes.Status502BadGateway,
                _ => StatusCodes.Status500InternalServerError
            };

            return Microsoft.AspNetCore.Http.Results.Problem(
                title: error.Code,
                detail: error.Message,
                statusCode: statusCode);
        }
    }
}