using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebIdentityApi.DTOs.Staff;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class StaffServices
    {
        private readonly UserServices _userServices;
        private readonly UserManager<User> _userManager;
        public StaffServices(UserServices userServices, UserManager<User> userManager)
        {
            _userServices = userServices;
            _userManager = userManager;
        }
        public async Task<User> CreateStaff(CreateStaffDto model, string password)
        {
            var staff = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
            };
            await _userManager.CreateAsync(staff, password);
            await _userManager.AddToRoleAsync(staff, "Staff");
            return staff;
        }
    }
}