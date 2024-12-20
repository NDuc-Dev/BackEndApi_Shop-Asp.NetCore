﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebIdentityApi.Models
{
    public class NameTag
    {
        [Key]
        public int NameTagId { get; set; }
        [Required(ErrorMessage = "Tag name is required")]
        public string Tag { get; set; }
        public string CreateByUserId { get; set; }
        [JsonIgnore]
        public User CreateBy { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public ICollection<ProductNameTag> ProductTags { get; set; }
    }
}
