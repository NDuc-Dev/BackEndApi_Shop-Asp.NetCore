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
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
