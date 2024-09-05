using System;
using System.Collections.Generic;

namespace WebIdentityApi.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ImagePath { get; set; }
        public string Descriptions { get; set; }
        public string CreateByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ICollection<Product> Products { get; set; }
    }
}
