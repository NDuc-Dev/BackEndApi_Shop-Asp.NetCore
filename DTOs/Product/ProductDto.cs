using System.Collections.Generic;
using WebIdentityApi.DTOs.NameTag;
using WebIdentityApi.DTOs.ProductColor;

namespace WebIdentityApi.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string BrandName { get; set; }
        public bool Status { get; set; }
        public List<string> Tag { get; set; }
        public List<ProductColorDto> Variant { get; set; }
    }
}
