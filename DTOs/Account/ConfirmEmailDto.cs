using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Account
{
    public class ConfirmEmailDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [RegularExpression("^((?!\\.)[\\w-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$", ErrorMessage = "Invalid email address !")]
        public string Email { get; set; }
    }
}
