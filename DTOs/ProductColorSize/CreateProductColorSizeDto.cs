using System;

namespace WebIdentityApi.DTOs.ProductColorSize
{
    public class CreateProductColorSizeDto
    {
        public int ProductColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
    }
}