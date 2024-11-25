using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required(ErrorMessage = "Size value is required")]
        public int SizeValue { get; set; }
        public string CreateByUserId { get; set; }
        public User CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public ICollection<ProductColorSize> ProductColorSize { get; set; }
    }
}
