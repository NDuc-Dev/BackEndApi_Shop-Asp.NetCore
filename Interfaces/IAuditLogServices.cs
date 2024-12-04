using System.Threading.Tasks;
using Serilog.Events;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface IAuditLogServices
    {
#nullable enable
        Task LogActionAsync(User user, string actionName, string? table = null, string? objId = null, string? exception = null, LogEventLevel? level = LogEventLevel.Information);
    }
}