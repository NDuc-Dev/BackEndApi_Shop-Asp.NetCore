using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public bool Status { get; set; } = false;
        public string CreateByUserId { get; set; }
        [JsonIgnore]
        public User CreatedByUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductNameTag> NameTags { get; set; } = null;
        public ICollection<ProductColor> ProductColor { get; set; }
    }
}
