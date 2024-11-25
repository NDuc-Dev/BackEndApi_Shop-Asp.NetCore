using System;
using System.Collections.Generic;
using WebIdentityApi.DTOs.Admin.NameTag;
using WebIdentityApi.DTOs.Admin.ProductColor;
using WebIdentityApi.DTOs.Admin.Staff;

namespace WebIdentityApi.DTOs.Admin.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public bool Status { get; set; }
        public List<NameTagDto> Tag { get; set; }
        public List<ProductColorDto> Variant { get; set; }
        public StaffDto CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}