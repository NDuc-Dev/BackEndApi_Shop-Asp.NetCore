using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class OrderDetails
    {
        [Key]
        public int DetailsId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductColorSizeId { get; set; }
        public ProductColorSize ProductColorSize { get; set; }
        [Column(TypeName = "decimal(9,0)")]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
        [Column(TypeName = "decimal(9,0)")]
        public int TotalAmount { get; set; }

    }
}
