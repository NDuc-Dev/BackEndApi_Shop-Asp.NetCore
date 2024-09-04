using System.Collections.Generic;

namespace WebIdentityApi.Models
{
    public class Size
    {
        public int SizeId { get; set; }
        public string SizeValue { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
