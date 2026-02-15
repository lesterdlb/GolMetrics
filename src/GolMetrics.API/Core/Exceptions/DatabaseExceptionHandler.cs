using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GolMetrics.API.Core.Exceptions;

internal sealed class DatabaseExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DbUpdateException and not DbUpdateConcurrencyException)
            return false;

        var problemDetails = new ProblemDetails
        {
            Title = "Database Conflict",
            Detail = "A database conflict occurred. The resource may have been modified by another request.",
            Status = StatusCodes.Status409Conflict,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10"
        };

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}