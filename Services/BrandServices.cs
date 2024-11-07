using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Brand;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class BrandServices
    {
        private readonly ApplicationDbContext _context;
        public BrandServices(ApplicationDbContext context)
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
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand> GetBrandById(int id)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandId == id);
            return brand;
        }

        public async Task<List<Brand>> GetBrands()
        {
            var brands = await _context.Brands.ToListAsync();
            return brands;
        }
    }
}