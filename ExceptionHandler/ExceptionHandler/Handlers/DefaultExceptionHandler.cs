using ExceptionHandler.Definitions;
using ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TsylExceptionHandler;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DefaultExceptionHandler> _logger;
    readonly JsonSerializerOptions options = new() { WriteIndented = true };

    public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        string guid = Guid.NewGuid().ToString();

        Dictionary<string, object?> extensions = new()
        {
            { "support", $"If you need further information, please contact support with the following id number: {guid}" }
        };

        ProblemDetails problemDetails = new()
        {
            Status = httpContext.Response.StatusCode,
            Type = exception.GetType().Name,
            Title = "An unexpected error occurred",
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} at {httpContext.Request.Host.Host}{httpContext.Request.Path}",
            Extensions = extensions
        };

        ApiError err = new
        (
            guid,
            problemDetails.Instance,
            DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            problemDetails.Detail,
            problemDetails.Status,
            exception.DescendantsAndSelf().StringifyMessages().Split("\r\n")            
        );

        _logger.LogError(JsonSerializer.Serialize(err, options), "An unexpected error occurred");
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}