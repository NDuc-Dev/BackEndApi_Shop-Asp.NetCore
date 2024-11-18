using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Account;
using WebIdentityApi.DTOs.Brand;
using WebIdentityApi.DTOs.Color;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.DTOs.Staff;
using WebIdentityApi.Models;
using WebIdentityApi.Services;
using SixLabors.ImageSharp;
using WebIdentityApi.DTOs.NameTag;
using AutoMapper;
using WebIdentityApi.Filters;
using WebIdentityApi.Interfaces;
using Microsoft.AspNetCore.Http;
using WebIdentityApi.Helpers;
using WebIdentityApi.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebIdentityApi.Controllers
{
    [Authorize(Policy = "OnlyAdminRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserServices _userServices;
        private readonly IProductServices _productServices;
        private readonly ImageServices _imageServices;
        private readonly BrandServices _brandServices;
        private readonly StaffServices _staffServices;
        public AdminController(
            UserManager<User> userManager,
            EmailService emailService,
            IConfiguration config,
            ApplicationDbContext context,
            IMapper mapper,
            UserServices userServices,
            IProductServices productServices,
            ImageServices imageServices,
            BrandServices brandServices,
            StaffServices staffServices)
        {
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            _context = context;
            _mapper = mapper;
            _userServices = userServices;
            _productServices = productServices;
            _imageServices = imageServices;
            _brandServices = brandServices;
            _staffServices = staffServices;
        }
        #region Staff Manage Function

        [HttpPost("create-staff-account")]
        public async Task<ActionResult> createStaff(CreateStaffDto model)
        {
            string message = "";
            if (await CheckEmailExistAsync(model.Email))
            {
                message = "Email has been exist, please try with another email";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<User>
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorView
                    {
                        Code = "EXIST_EMAIL",
                        Message = message
                    }

                });
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var password = await _userServices.GenerateDefaultPassword();
                    var staff = await _staffServices.CreateStaff(model, password);
                    await _userManager.AddToRoleAsync(staff, "Staff");
                    if (await SendConfirmEmailAsync(staff, password))
                    {
                        await transaction.CommitAsync();
                        var result = new ResponseView<User>()
                        {
                            Success = true,
                            Message = "Create staff successfully!",
                            Data = staff
                        };
                        return Ok(result);
                    }
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                    {
                        Success = false,
                        Message = "",
                        Error = new ErrorView()
                        {
                            Code = "SERVER_ERROR",
                            Message = "Failed to create account, please try again !"
                        }
                    });

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                    {
                        Success = false,
                        Message = "",
                        Error = new ErrorView()
                        {
                            Code = "SERVER_ERROR",
                            Message = "Failed to create account, please try again !"
                        }
                    });
                }
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
            if (staff == null) return NotFound("Staff not found !");
            try
            {
                var staffDto = new StaffDto
                {
                    Email = staff.Email,
                    Id = staff.Id,
                    FullName = staff.FullName
                };
                return Ok(staffDto);

            }
            catch (Exception)
            {
                return BadRequest("Error !");
            }

        }

        [HttpPut("update-staff/{id}")]
        public async Task<IActionResult> UpdateStaff(string id, UpdateStaffDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("Staff not found !");
            try
            {
                bool isStaff = await CheckStaffRoleAsync(user);
                if (isStaff)
                {
                    user.FullName = model.FullName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.Phone;
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded) return BadRequest(result.Errors);
                    return Ok(new { title = "Success", message = "Update staff successfully " });
                }
                return BadRequest("Access denied !");
            }
            catch (Exception)
            {
                return StatusCode(500, new { title = "Error", message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("reset-password/{id}")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest("Error, Staff not found !");
            if (!await CheckStaffRoleAsync(user)) return BadRequest("Access Denied");
            if (user.EmailConfirmed == false) return BadRequest("Please confirm your email first");
            try
            {
                if (await _userServices.SendForgotPassword(user))
                {
                    return Ok(new JsonResult(new { title = "Success", message = "Reset password successfully" }));
                }
                return BadRequest("An occured error, please try again later.");
            }
            catch (Exception)
            {
                return BadRequest("An occured error, please try again later.");

            }
        }
        #endregion

        #region Brand Manage Function
        [HttpGet("get-brands")]
        public async Task<IActionResult> GetAllBrand()
        {
            try
            {
                var brands = await _brandServices.GetBrands();
                if (brands == null) throw new Exception("Not have brand in list");
                var brandDto = _mapper.Map<List<BrandDto>>(brands);
                return Ok(brandDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("get-brand/{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                var brand = await _brandServices.GetBrandById(id);
                if (brand == null) throw new Exception("Brand not found");
                var listBrandDto = _mapper.Map<BrandDto>(brand);
                return Ok(listBrandDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("create-brand")]
        public async Task<IActionResult> CreateBrand([FromForm] CreateBrandDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    string filePath = await _imageServices.CreatePathForImg("brands", model.Image);
                    var authorizationHeader = Request.Headers.Authorization.FirstOrDefault();
                    var user = await _userServices.GetUserInfoFromJwtAsync(authorizationHeader);
                    if (user == null) throw new Exception("User not found!");
                    var exitsName = await _context.Brands.FirstOrDefaultAsync(b => b.BrandName == model.BrandName);
                    if (exitsName != null) throw new Exception($"Brand {model.BrandName} has been exist, please use another name");
                    var brand = await _brandServices.CreateBrandAsync(model, user, filePath);
                    await transaction.CommitAsync();
                    return CreatedAtAction(
                    nameof(GetBrandById),
                    new { id = brand.BrandId },
                    new { title = "Brand Created", message = $"Create {model.BrandName} brand successfully!" }
                    );
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.ToString());
                }
            }
        }

        [HttpPut("update-brand/{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] UpdateBrandDto model)
        {
            if (model.DataChanged == false) return Ok("No data field changed!");
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.BrandId == id);
            if (brand == null) return BadRequest("Brand does not exist, please try again");
            string fileImagePath = null;
            if (model.ImageChanged == true)
            {
                fileImagePath = await _imageServices.CreatePathForImg("brands", model.Image);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                var exitsName = await _context.Brands.FirstOrDefaultAsync(b => b.BrandName == model.BrandName);
                if (exitsName != null) throw new Exception($"Brand {model.BrandName} has been exist, please try with another name");
                try
                {
                    brand.BrandName = model.BrandName;
                    brand.Descriptions = model.Descriptions;
                    if (fileImagePath != null)
                    {
                        brand.ImagePath = fileImagePath;
                    }
                    _context.Update(brand);
                    await transaction.CommitAsync();
                    return Ok("Brand has been update !");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.ToString());
                }
            }
        }
        #endregion

        #region Color Manage Function
        [HttpGet("get-colors")]
        public async Task<IActionResult> GetColors()
        {
            var color = await _context.Colors.ToListAsync();
            return Ok(color);
        }

        [HttpGet("get-color/{id}")]
        public async Task<IActionResult> GetColorById(int id)
        {
            var color = await _context.Colors.FirstOrDefaultAsync(c => c.ColorId == id);
            if (color == null) return BadRequest("Color not found");
            return Ok(color);
        }

        [HttpPost("create-color")]
        public async Task<IActionResult> CreateColor(ColorDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var color = new Models.Color
                    {
                        ColorName = model.ColorName
                    };
                    _context.Colors.Add(color);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return CreatedAtAction(nameof(GetColorById),
                        new { id = color.ColorId },
                        new { title = "Color Created", message = $"Create {model.ColorName} color successfully!" }
                        );
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.ToString());
                }
            }
        }

        [HttpPut("update-color/{id}")]
        public async Task<IActionResult> UpdateColor(int id, ColorDto model)
        {
            var color = await _context.Colors.FirstOrDefaultAsync(c => c.ColorId == id);
            if (color == null) return BadRequest("Color does not exist !");
            var existColor = await _context.Colors.FirstOrDefaultAsync(c => c.ColorName == model.ColorName);
            if (existColor != null) return BadRequest("Color name has been exist, please try with another name again !");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    color.ColorName = model.ColorName;
                    _context.Update(color);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Ok("Update Color Successfully !");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.ToString());
                }

            }
        }
        #endregion

        #region Name Tag Manage Funtion
        [HttpGet("get-name-tags")]
        public async Task<IActionResult> GetNameTags()
        {
            var nametag = await _context.NameTags.ToListAsync();
            return Ok(nametag);
        }

        [HttpGet("get-name-tag/{id}")]
        public async Task<IActionResult> GetNameTagById(int id)
        {
            var nameTag = await _context.NameTags.FirstOrDefaultAsync(n => n.NameTagId == id);
            if (nameTag == null) return BadRequest("Name tag does not exist !");
            return Ok(nameTag);
        }

        [HttpPost("create-name-tag")]
        public async Task<IActionResult> CreateNameTag(NameTagDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existName = await _context.NameTags.FirstOrDefaultAsync(n => n.Tag == model.TagName);
            if (existName != null) return BadRequest("Name tag has been exist, please try again !");
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var nameTag = new NameTag
                    {
                        Tag = model.TagName,
                    };
                    _context.NameTags.Add(nameTag);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return CreatedAtAction(nameof(GetNameTagById),
                    new { id = nameTag.NameTagId },
                    new { title = "Tag Created", message = $"Create {model.TagName} tag successfully!" }
                    );
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region Size Manage Function
        [HttpGet("get-sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await _context.Sizes.ToListAsync();
            return Ok(sizes);
        }

        [HttpGet("get-size/{id}")]
        public async Task<IActionResult> GetSizeById(int id)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(s => s.SizeId == id);
            if (size == null) return BadRequest("Size does not exist");
            return Ok(size);
        }
        #endregion

        #region Product Manage Function
        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilters filter, int? pageNumber, int? pageSize)
        {
            int pageSizeValue = pageSize ?? 10;
            int pageNumberValue = pageNumber ?? 1;
            if (pageNumberValue < 0 || pageSizeValue <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseView()
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "INVALID_INPUT",
                        Message = "Invalid page number or page size"
                    }
                });
            }
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(filter.Name.ToLower()));
            }
            if (filter.Brand.HasValue)
            {
                query = query.Where(p => p.BrandId == filter.Brand);
            }
            if (filter.Color != null && filter.Color.Any())
            {
                query = query.Where(p => p.ProductColor.Any(pc => filter.Color.Contains(pc.ColorId)));
            }
            if (filter.Size != null && filter.Size.Any())
            {
                query = query.Where(p => p.ProductColor.Any(pc => pc.ProductColorSizes.Any(pcs => filter.Size.Contains(pcs.SizeId))));
            }
            try
            {
                var totalProducts = await query.CountAsync();
                await query.Include(p => p.Brand)
                    .Include(p => p.NameTags)
                    .ThenInclude(nt => nt.NameTag)
                    .Include(p => p.ProductColor)
                    .Where(p => p.Status == true).Skip((pageNumberValue - 1) * pageSizeValue)
                    .Take(pageSizeValue)
                    .ToListAsync();

                var productDtos = _mapper.Map<List<ListProductDto>>(query);
                var paginateData = new PaginateDataView<ListProductDto>()
                {
                    Data = productDtos,
                    totalCount = totalProducts
                };
                var response = new ResponseView<PaginateDataView<ListProductDto>>()
                {
                    Success = true,
                    Message = "Products retrieved successfully !",
                    Data = paginateData
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "INTERNAL_SERVER_ERROR",
                        Message = "Error retrieving products"
                    }
                });
            }
        }

        [HttpGet("get-product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.NameTags)
            .ThenInclude(nt => nt.NameTag)
            .Include(p => p.ProductColor)
            .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductColor)
            .ThenInclude(pc => pc.ProductColorSizes)
            .ThenInclude(pc => pc.Size)
            .FirstOrDefaultAsync(p => p.ProductId == id && p.Status == true);
            if (product == null) return NotFound();
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto model)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                var authorizationHeader = Request.Headers.Authorization.FirstOrDefault();
                var user = await _userServices.GetUserInfoFromJwtAsync(authorizationHeader);
                if (await _context.IsExistsAsync<Product>("ProductName", model.ProductName))
                {
                    message = $"Product name {model.ProductName} has been exist, please try with another name";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<Product>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorView
                        {
                            Code = "DUPPLICATE_NAME",
                            Message = message
                        }
                    });
                }
                if (!await _context.IsExistsAsync<Brand>("BrandId", model.BrandId))
                {
                    message = "Brand does not exist, please try again";
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<Product>
                    {
                        Success = false,
                        Message = message,
                        Error = new ErrorView
                        {
                            Code = "INVALID_DATA",
                            Message = message
                        }
                    });
                };
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var product = await _productServices.CreateProductAsync(model, model.BrandId, user);
                        if (model.NameTagId != null && model.NameTagId.Count > 0)
                        {
                            foreach (var tagId in model.NameTagId)
                            {
                                if (!await _context.IsExistsAsync<NameTag>("NameTagId", tagId))
                                {
                                    await transaction.RollbackAsync();
                                    message = "Inavalid name tag";
                                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<Product>
                                    {
                                        Success = false,
                                        Message = message,
                                        Error = new ErrorView
                                        {
                                            Message = message,
                                            Code = "INVALID_DATA"
                                        }
                                    });
                                }
                                await _productServices.CreateProductNameTagAsync(product, tagId);
                            }
                        }
                        foreach (var variant in model.Variants)
                        {
                            var imagesPath = string.Empty;
                            foreach (var image in variant.images)
                            {
                                var filePath = _imageServices.CreatePathForBase64Img("products", image);
                                imagesPath += $"{filePath};";
                            }
                            imagesPath = imagesPath.TrimEnd(';');
                            if (!await _context.IsExistsAsync<Models.Color>("ColorId", variant.ColorId))
                            {
                                await transaction.RollbackAsync();
                                message = "Inavalid color";
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<Product>
                                {
                                    Success = false,
                                    Message = message,
                                    Error = new ErrorView
                                    {
                                        Message = message,
                                        Code = "INVALID_DATA"
                                    }
                                });
                            };
                            var productColor = await _productServices.CreateProductColorAsync(product, variant.ColorId, variant.Price, imagesPath);

                            foreach (var size in variant.ProductColorSize)
                            {
                                if (!await _context.IsExistsAsync<Models.Size>("SizeId", size.SizeId))
                                {
                                    await transaction.RollbackAsync();
                                    message = "Inavalid size";
                                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<Product>
                                    {
                                        Success = false,
                                        Message = message,
                                        Error = new ErrorView
                                        {
                                            Message = message,
                                            Code = "INVALID_DATA"
                                        }
                                    });
                                }
                                var productColorSize = await _productServices.CreateProductColorSizeAsync(productColor, size.SizeId, size.Quantity);
                            }
                        }
                        await transaction.CommitAsync();
                        var response = new ResponseView<Product>
                        {
                            Success = true,
                            Message = "Product Created Successfully",
                            Data = product

                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        message = $"An error occurred while adding the product- " + ex.Message;

                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView<Product>
                        {
                            Success = false,
                            Message = message,
                            Error = new ErrorView
                            {
                                Code = "SERVER_ERROR",
                                Message = message
                            }
                        });
                    }
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseView<Product>
            {
                Success = false,
                Message = "Invalid input",
                Error = new ErrorView
                {
                    Code = "INPUT_VALIDATION_ERROR",
                    Message = ModelStateHelper.GetErrors(ModelState)
                }
            });
        }

        [HttpPost("change-status/{id}")]
        public async Task<IActionResult> ChangeProductStatus(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            product.Status = product.Status ? false : true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok("Change product status successfully");
        }
        #endregion

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
            var emailSend = new EmailSendDto(userToAdd.Email, "CONFIRM YOUR EMAIL", body);
            return await _emailService.SendEmail(emailSend);
        }

        private async Task<bool> CheckStaffRoleAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            bool isStaff = roles.Any(r => r == "Staff");
            return isStaff;
        }
        #endregion
    }
}
