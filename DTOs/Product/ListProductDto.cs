using System.Collections.Generic;
using WebIdentityApi.DTOs.NameTag;

namespace WebIdentityApi.DTOs.Product
{
    public class ListProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string BrandName { get; set; }
        public List<NameTagDto> Tag { get; set; }
        public string ImagePath { get; set; }
    }
}
