using System.Threading.Tasks;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface ISystemServices
    {
        Task<AuditLog> CreateLog(string action, object dataBefore, object dataAfter, User userHandle, string searchKeyword);
    }
}