using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Account
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [RegularExpression("^((?!\\.)[\\w-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$", ErrorMessage = "Invalid email address !")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password Name must be at least {2} and maximum {1} characters")]
        public string NewPassword { get; set; }
    }
}
