using System;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.Models
{
    public class ProductNameTag
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int NameTagId { get; set; }
        public NameTag NameTag { get; set; }
        public string CreateByUserId { get; set; }
        public User CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
