using System.Threading.Tasks;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface IAuditLogServices
    {
        #nullable enable
        Task LogActionAsync(User user, string actionName, object? beforeData = null, object? afterData = null, string? keywords = null);
    }
}