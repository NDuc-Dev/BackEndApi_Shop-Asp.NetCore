using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required(ErrorMessage = "Color name is required")]
        public string ColorName { get; set; }
        public string CreateByUserId { get; set; }
        public User CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public ICollection<ProductColor> ProductColor { get; set; }

    }
}
