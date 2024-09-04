using System;
using System.Collections.Generic;

namespace WebIdentityApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
        public string ImagePath { get; set; }
    }
}
