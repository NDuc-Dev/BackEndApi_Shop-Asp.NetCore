using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebIdentityApi.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required(ErrorMessage = "Size value is required")]
        public int SizeValue { get; set; }
        public string CreateByUserId { get; set; }
        [JsonIgnore]
        public User CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public ICollection<ProductColorSize> ProductColorSize { get; set; }
    }
}
