using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebIdentityApi.DTOs.Account;
using WebIdentityApi.DTOs.Admin.Account;
using WebIdentityApi.Models;
using WebIdentityApi.Services;

namespace WebIdentityApi.Controllers
{
    [Authorize(Policy = "OnlyAdminRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private IConfiguration _config;
        public AdminController(UserManager<User> userManager, JwtService jwtService, EmailService emailService, IConfiguration config)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _config = config;
        }
        [HttpPost("create-staff-account")]
        public async Task<ActionResult> createStaff(CreateStaffDto model)
        {
            if (await CheckEmailExistAsync(model.Email)) return BadRequest("Email is already exist, please try with another email !");

            var staff = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
            };
            var password = await GenerateDefaultPassword();
            var result = await _userManager.CreateAsync(staff, password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(staff, "Staff");
            try
            {
                if (await SendConfirmEmailAsync(staff, password))
                {
                    return Ok(new JsonResult(new { title = "Account Created", message = "Create staff account successfully!" }));
                }
                return BadRequest("Failed to create account, please try again !");
            }
            catch (Exception)
            {
                return BadRequest("Failed to create account, please try again !");
            }
        }

        [HttpGet("get-all-staff")]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetStaffs()
        {
            var staffs = await _userManager.GetUsersInRoleAsync("Staff");
            if (staffs.Count == 0) return BadRequest("There are no staff on the list");
            var staffDto = staffs.Select(staff => new StaffDto
            {
                Id = staff.Id,
                FullName = staff.FullName,
                Email = staff.Email
            });
            return Ok(staffDto);
        }

        [HttpGet("get-staff/{id}")]
        public async Task<ActionResult<StaffDto>> GetStaff(string id)
        {
            var staff = await _userManager.FindByIdAsync(id);
            if (staff == null) return BadRequest("Staff not found !");
            var staffDto = new StaffDto
            {
                Email = staff.Email,
                Id = staff.Id,
                FullName = staff.FullName
            };
            return Ok(staffDto);
        }

        //[HttpPut("update-staff/{id}")]
        //public async Task<IActionResult> UpdateStaff (string id)
        //{

        //}

        #region Private Helper Method

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        private async Task<bool> SendConfirmEmailAsync(User userToAdd, string password)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userToAdd);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ConfirmEmailPath"]}?token={token}&email={userToAdd.Email}";

            var body = $"<p>Hi: {userToAdd.FullName}</p> " +
                $"<p>Your account has been successfully created, please click <a href =\"{url}\">here</a> to verify your email and log in to your account to change your password.</p>" +
                $"<p>Your default password :{password} </p>" +
                "<p>Thank you, Welcome to My shop</p>" +
                $"<br>{_config["Email:ApplicationName"]}";

            var emaiSend = new EmailSendDto(userToAdd.Email, "CONFIRM YOUR EMAIL", body);

            return await _emailService.SendEmail(emaiSend);
        }

        public async Task<string> GenerateDefaultPassword()
        {
            Random random = new Random();

            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string specialChars = "!@#$%^&*()";
            const string digits = "0123456789";

            char upperChar = upperChars[random.Next(upperChars.Length)];
            char lowerChar = lowerChars[random.Next(lowerChars.Length)];
            char specialChar = specialChars[random.Next(specialChars.Length)];

            string numberString = new string(Enumerable.Repeat(digits, 3)
                                                .Select(s => s[random.Next(s.Length)])
                                                .ToArray());

            // Combine all and shuffle
            string result = upperChar.ToString() + lowerChar + specialChar + numberString;
            return new string(result.ToCharArray().OrderBy(c => random.Next()).ToArray());
        }
        #endregion
    }
}
