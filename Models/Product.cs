using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product descriptions is required")]
        public string Description { get; set; }
        public bool Status { get; set; }
        public string CreateByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductNameTag> NameTags { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
        public string ImagePath { get; set; }
    }
}
