using System.Collections.Generic;
using WebIdentityApi.DTOs.ProductColor;

namespace WebIdentityApi.DTOs.Product
{
    public class ProducGetListDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string BrandName { get; set; }
        public List<string> Tag { get; set; }
        public string ImagePath { get; set; }
    }
}
