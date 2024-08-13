using Genocs.HTTP;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace GenocsAspire.Identities.Application.Logging;

internal class LogContextMiddleware : IMiddleware
{
    private readonly ICorrelationIdFactory _correlationIdFactory;

    public LogContextMiddleware(ICorrelationIdFactory correlationIdFactory)
    {
        _correlationIdFactory = correlationIdFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = _correlationIdFactory.Create();
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await next(context);
        }
    }
}