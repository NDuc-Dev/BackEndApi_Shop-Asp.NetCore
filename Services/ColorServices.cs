using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Color;
using WebIdentityApi.Interfaces;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class ColorServices : IColorServices
    {
        private readonly ApplicationDbContext _context;
        public ColorServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Color> CreateColorAsync(ColorDto model)
        {
            var color = new Color
            {
                ColorName = model.ColorName
            };
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();
            return color;
        }

        public async Task<Color> GetColorById(int id)
        {
            return await _context.Colors.FirstOrDefaultAsync(c => c.ColorId == id);
        }

        public async Task<List<Color>> GetColors()
        {
            return await _context.Colors.ToListAsync();
        }
    }
}