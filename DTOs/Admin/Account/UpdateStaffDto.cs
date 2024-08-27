using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Admin.Account
{
    public class UpdateStaffDto
    {
        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Full Name must be at least {2} and maximum {1} characters")]
        public string FullName { get; set; }
        public string Phone { get; set; } = string.Empty;
        [Required]
        [RegularExpression("^((?!\\.)[\\w-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$", ErrorMessage = "Invalid email address !")]
        public string Email { get; set; }
    }
}
