using System.Collections.Generic;
using WebIdentityApi.DTOs.ProductColorSize;

namespace WebIdentityApi.DTOs.ProductColor
{
    public class CreateProductColorDto
    {
        public int ColorId { get; set; }
        public decimal Price { get; set; }
        public List<string> images { get; set; }
        public List<CreateProductColorSizeDto> ProductColorSize { get; set; }
    }
}