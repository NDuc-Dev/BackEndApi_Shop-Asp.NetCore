using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

public class LogEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public LogEnrichmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User?.FindFirst("nameId")?.Value ?? "Anonymous";
        var requestId = context.TraceIdentifier; 

        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("RequestId", requestId))
        {
            await _next(context); 
        }
    }
}
