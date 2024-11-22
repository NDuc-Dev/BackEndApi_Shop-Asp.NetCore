using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Size
{
    public class CreateSizeDto
    {
        [Required(ErrorMessage = "Size value is required !")]
        public int SizeValue { get; set; }
    }
}