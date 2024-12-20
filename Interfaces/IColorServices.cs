using System.Collections.Generic;
using System.Threading.Tasks;
using WebIdentityApi.DTOs.Color;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface IColorServices
    {
        Task<Color> CreateColorAsync(CreateColorDto model, User user);
        Task<Color> GetColorById(int id);
        Task<List<Color>> GetColors();
    }
}