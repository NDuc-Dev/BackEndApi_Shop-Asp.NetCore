using System.Collections.Generic;
using WebIdentityApi.DTOs.ProductColor;

namespace WebIdentityApi.DTOs.Product
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        public List<CreateProductColorDto> Variants { get; set; }
    }
}
