using System.Threading.Tasks;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface ISystemServices
    {
        Task Log(string tableName, string action, object dataBefore, object dataAfter, User userHandle);
    }
}