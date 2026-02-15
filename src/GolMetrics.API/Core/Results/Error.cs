namespace GolMetrics.API.Core.Results;

public sealed record Error(string Code, string Message, ErrorCategory Category);