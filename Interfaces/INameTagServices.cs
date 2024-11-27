using System.Collections.Generic;
using System.Threading.Tasks;
using WebIdentityApi.DTOs.NameTag;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface INameTagServices
    {
        Task<List<NameTag>> GetNameTags();
        Task<NameTag> GetNameTagById(int id);
        Task<NameTag> CreateNameTagAsync(NameTagDto model, User user);
    }
}