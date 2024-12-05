using System.Collections.Generic;
using System.Threading.Tasks;
using WebIdentityApi.DTOs.Brand;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces.Advance
{
    public interface IAdvanceBrandServices
    {
        Task<Brand> CreateBrandAsync(CreateBrandDto model, User user, string filePath);
        Task<Brand> GetBrandById(int id);
        Task<List<Brand>> GetBrands();
    }
}