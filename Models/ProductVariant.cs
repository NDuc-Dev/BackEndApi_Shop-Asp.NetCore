using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class ProductVariant
    {
        [Key]
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        [Column(TypeName = "decimal(9,0)")]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
        public ICollection<OrderDetails> Details { get; set; }

    }
}
