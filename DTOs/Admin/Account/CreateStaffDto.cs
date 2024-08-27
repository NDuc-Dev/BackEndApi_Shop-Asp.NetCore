using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Admin.Account
{
    public class CreateStaffDto
    {
        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Full Name must be at least {2} and maximum {1} characters")]
        public string FullName { get; set; }
        [Required]
        [RegularExpression("^((?!\\.)[\\w-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$", ErrorMessage = "Invalid email address !")]
        public string Email { get; set; }
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Invalid Phone number !")]
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
