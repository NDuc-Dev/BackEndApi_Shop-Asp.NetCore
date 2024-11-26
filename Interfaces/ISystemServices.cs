using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface ISystemServices
    {
        void Log(string tableName, string action, object dataBefore, object dataAfter, User userHandle);
    }
}