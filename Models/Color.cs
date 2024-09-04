using System.Collections.Generic;

namespace WebIdentityApi.Models
{
    public class Color
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public ICollection<ProductVariant> ProductVariants { get; set; }

    }
}
