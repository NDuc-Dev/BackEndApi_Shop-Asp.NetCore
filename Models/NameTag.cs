using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.Models
{
    public class NameTag
    {
        [Key]
        public int NameTagId { get; set; }
        [Required(ErrorMessage = "Tag name is required")]
        public string Tag {  get; set; }
        public ICollection<ProductNameTag> ProductTags { get; set; }
    }
}
