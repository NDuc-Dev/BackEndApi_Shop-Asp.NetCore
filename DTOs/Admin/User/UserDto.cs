using System;

namespace WebIdentityApi.DTOs.Admin.User
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreateDate { get; set; }
        public string Address { get; set; }
        public bool AccountStatus { get; set; }
    }
}