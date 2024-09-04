using System.ComponentModel.DataAnnotations;

namespace WebIdentityApi.DTOs.Staff
{
    public class UpdateStaffDto
    {
        public string StaffId { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Full Name must be at least {2} and maximum {1} characters")]
        public string FullName { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
