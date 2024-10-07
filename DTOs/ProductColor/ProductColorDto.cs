using System;
using System.Collections.Generic;
using WebIdentityApi.DTOs.ProductColorSize;

namespace WebIdentityApi.DTOs.ProductColor
{
    public class ProductColorDto
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public List<ProductColorSizeDto> ProductColorSize {get; set;}
    }
}