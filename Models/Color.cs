using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebIdentityApi.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required(ErrorMessage = "Color name is required")]
        public string ColorName { get; set; }
        public string CreateByUserId { get; set; }
        [JsonIgnore]
        public User CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public ICollection<ProductColor> ProductColor { get; set; }

    }
}
