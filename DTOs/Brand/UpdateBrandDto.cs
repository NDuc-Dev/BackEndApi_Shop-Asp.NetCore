using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Brand
{
    public class UpdateBrandDto
    {
        [Required(ErrorMessage = "Brand name is required")]
        public string BrandName { get; set; }
        [Required(ErrorMessage = "Brand descriptions is required")]
        public string Descriptions { get; set; }
        public bool ImageChanged {get; set;}
        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }
        public bool DataChanged { get; set; }
    }
}
