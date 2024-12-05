using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Brand;
using WebIdentityApi.Interfaces.Advance;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services.Advance
{
    public class AdvanceBrandServices : IAdvanceBrandServices
    {
        private readonly ApplicationDbContext _context;
        public AdvanceBrandServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Brand> CreateBrandAsync(CreateBrandDto model, User user, string filePath)
        {
            var brand = new Brand
            {
                BrandName = model.BrandName,
                Descriptions = model.Descriptions,
                CreatedByUser = user,
                ImagePath = Path.GetFileName(filePath)
            };
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public Task<Brand> GetBrandById(int id)
        {
            return _context.Brands.FirstOrDefaultAsync(b => b.BrandId == id);
        }

        public Task<List<Brand>> GetBrands()
        {
            return _context.Brands.ToListAsync(); ;
        }
    }
}