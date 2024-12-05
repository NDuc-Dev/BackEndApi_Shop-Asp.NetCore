using System.Threading.Tasks;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces.Advance
{
    public interface IAdvanceProductServices
    {
        Task<Product> CreateProductAsync(CreateProductDto model, int brandId, User user);
        Task<ProductNameTag> CreateProductNameTagAsync(Product product, int nameTagId);
        Task<ProductColor> CreateProductColorAsync(Product product, int colorId, decimal price, string imagePath);
        Task<ProductColorSize> CreateProductColorSizeAsync(ProductColor productColor, int sizeId, int quantity);
        Task<Product> GetProductById(int id);

    }
}