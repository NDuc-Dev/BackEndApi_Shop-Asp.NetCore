using System.Collections.Generic;
using System.Threading.Tasks;
using WebIdentityApi.DTOs.Size;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface ISizeServices
    {
        Task<Size> CreateSizeAsync(CreateSizeDto model);
        Task<List<Size>> GetSizes();
        Task<Size> GetSizeById(int id);
    }
}