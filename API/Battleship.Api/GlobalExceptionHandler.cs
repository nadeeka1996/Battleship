using Battleship.Application.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace Battleship.Api;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}",
            exception.InnerException?.Message ?? exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            Envelope.Error("An error occurred while processing your request."), 
            cancellationToken
        );

        return true;
    }
}
