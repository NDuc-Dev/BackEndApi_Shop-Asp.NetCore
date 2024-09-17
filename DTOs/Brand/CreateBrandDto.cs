using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace WebIdentityApi.DTOs.Brand
{
    public class CreateBrandDto
    {
        public string BrandName { get; set; }
        public string Descriptions { get; set; }
        public IFormFile Image { get; set; }
    }
}
