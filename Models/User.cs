using Microsoft.AspNetCore.Identity;
using System;

namespace WebIdentityApi.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;

    }
}
