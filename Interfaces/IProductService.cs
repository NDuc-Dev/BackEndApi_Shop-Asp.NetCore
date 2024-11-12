using System.Threading.Tasks;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.Models;

namespace WebIdentityApi.Interfaces
{
    public interface IProductServices
    {
        Task<Product> CreateProductAsync(CreateProductDto model, Brand brand, User user);
        Task<ProductNameTag> CreateProductNameTagAsync(Product product, NameTag nameTag);
        Task<ProductColor> CreateProductColorAsync(Product product, Color color, decimal price, string imagePath);
        Task<ProductColorSize> CreateProductColorSizeAsync(ProductColor productColor, Size size, int quantity);
    }
}