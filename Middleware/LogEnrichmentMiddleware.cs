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
        // Thêm thông tin UserId vào LogContext nếu có
        var userId = context.User?.FindFirst("nameid")?.Value ?? "Anonymous";
        var requestId = context.TraceIdentifier; // ID của request hiện tại

        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("RequestId", requestId))
        {
            await _next(context); // Tiếp tục pipeline
        }
    }
}
