using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebIdentityApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderBy { get; set; }
        public User OrderByUser { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int OrderStatus { get; set; } = 0;
        [Column(TypeName = "decimal(9,0)")]
        public decimal Amount { get; set; } = 0;
        public int OrderType { get; set; }
        public ICollection<OrderDetails> Details { get; set; }
    }
}
