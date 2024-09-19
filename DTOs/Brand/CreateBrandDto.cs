using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Brand
{
    public class CreateBrandDto
    {
        [Required(ErrorMessage = "Brand name is required")]
        public string BrandName { get; set; }
        [Required(ErrorMessage = "Brand descriptions is required")]
        public string Descriptions { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }
    }
}
