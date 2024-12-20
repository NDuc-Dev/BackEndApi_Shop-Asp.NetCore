using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Size;
using WebIdentityApi.Interfaces;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class SizeServices :ISizeServices
    {
        private readonly ApplicationDbContext _context;
        public SizeServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Size> CreateSizeAsync(CreateSizeDto model)
        {
            var size = new Size
            {
                SizeValue = model.SizeValue
            };
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();
            return size;
        }
        public Task<List<Size>> GetSizes()
        {
            return _context.Sizes.ToListAsync();
        }
        public Task<Size> GetSizeById(int id)
        {
            return _context.Sizes.FirstOrDefaultAsync(s => s.SizeId == id);
        }
    }
}