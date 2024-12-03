using System;
using System.Threading.Tasks;
using Serilog;
using WebIdentityApi.Interfaces;
using WebIdentityApi.Models;

public class AuditLogService : IAuditLogServices
{
#nullable enable
    public async Task LogActionAsync(User user, string actionName, object? beforeData = null, object? afterData = null, string? keywords = null)
    {
        var logEntry = new AuditLog
        {
            UserId = user.Id,
            UserName = user.FullName,
            Action = actionName,
            DataBefore = beforeData,
            DataAfter = afterData,
            TimeStamp = DateTime.Now,
            SearchKeyword = keywords
        };

        Log.ForContext("AuditLog", true)
           .Information("{@logEntry}", logEntry);

        await Task.CompletedTask;
    }
}