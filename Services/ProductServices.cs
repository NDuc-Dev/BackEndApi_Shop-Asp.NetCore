using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.DTOs.ProductColor;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class ProductServices
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public ProductServices(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Product> CreateProduct(CreateProductDto model, Brand brand)
        {
            var productMap = _mapper.Map<Product>(model);
            productMap.Brand = brand;
            _context.Products.Add(productMap);
            await _context.SaveChangesAsync();
            return productMap;
        }

        public async Task<ProductNameTag> CreateProductNameTag(Product product, NameTag nameTag)
        {
            var productNameTag = new ProductNameTag
            {
                Product = product,
                NameTag = nameTag,
            };
            _context.ProductNameTags.Add(productNameTag);
            await _context.SaveChangesAsync();
            return productNameTag;
        }
    }
}