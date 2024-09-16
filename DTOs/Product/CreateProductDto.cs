using System.Collections.Generic;
using WebIdentityApi.DTOs.ProductVariant;

namespace WebIdentityApi.DTOs.Product
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        public List<CreateProductVariantDto> Variants { get; set; }
    }
}
