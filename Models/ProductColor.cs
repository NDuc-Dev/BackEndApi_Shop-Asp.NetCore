using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class ProductColor
    {
        [Key]
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        [Column(TypeName = "decimal(9,0)")]
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public ICollection<ProductColorSize> ProductColorSizes { get; set; }
    }
}
