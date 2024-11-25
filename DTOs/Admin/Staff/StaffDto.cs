using System;

namespace WebIdentityApi.DTOs.Admin.Staff
{
    public class StaffDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
    }
}