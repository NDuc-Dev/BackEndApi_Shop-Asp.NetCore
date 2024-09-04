using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class ProductVariant
    {
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
    }
}
