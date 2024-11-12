using System.Threading.Tasks;
using AutoMapper;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.Interfaces;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public ProductServices(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Product> CreateProductAsync(CreateProductDto model, Brand brand, User user)
        {
            var productMap = _mapper.Map<Product>(model);
            productMap.Brand = brand;
            productMap.CreatedByUser = user;
            _context.Products.Add(productMap);
            await _context.SaveChangesAsync();
            return productMap;
        }

        public async Task<ProductNameTag> CreateProductNameTagAsync(Product product, NameTag nameTag)
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

        public async Task<ProductColor> CreateProductColorAsync(Product product, Color color, decimal price, string imagePath)
        {
            var productColor = new ProductColor
            {
                Product = product,
                Color = color,
                Price = price,
                ImagePath = imagePath
            };
            _context.ProductColors.Add(productColor);
            await _context.SaveChangesAsync();
            return productColor;
        }

        public async Task<ProductColorSize> CreateProductColorSizeAsync(ProductColor productColor, Size size, int quantity)
        {
            var productColorSize = new ProductColorSize
            {
                ProductColor = productColor,
                Size = size,
                Quantity = quantity
            };
            _context.ProductColorSizes.Add(productColorSize);
            await _context.SaveChangesAsync();
            return productColorSize;
        }
    }
}