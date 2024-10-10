using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebIdentityApi.DTOs.ProductColorSize;

namespace WebIdentityApi.DTOs.ProductColor
{
    public class CreateProductColorDto
    {
        public int ColorId { get; set; }
        public decimal Price { get; set; }
        public IFormFileCollection images { get; set; }
        public List<CreateProductColorSizeDto> ProductColorSize { get; set; }
    }
}