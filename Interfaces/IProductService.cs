using System.Threading.Tasks;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.Models;
using WebIdentityApi.Services;

namespace WebIdentityApi.Interfaces
{
    public interface IProductServices
    {
        Task<Product> CreateProductAsync(CreateProductDto model, int brandId, User user);
        Task<ProductNameTag> CreateProductNameTagAsync(Product product, int nameTagId);
        Task<ProductColor> CreateProductColorAsync(Product product, int colorId, decimal price, string imagePath);
        Task<ProductColorSize> CreateProductColorSizeAsync(ProductColor productColor, int sizeId, int quantity);
    }
}