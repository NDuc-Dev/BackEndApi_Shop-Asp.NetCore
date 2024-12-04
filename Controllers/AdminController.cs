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
using WebIdentityApi.Extensions;
using WebIdentityApi.DTOs;
using Microsoft.Extensions.Logging;

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
        private readonly IBrandServices _brandServices;
        private readonly StaffServices _staffServices;
        private readonly IColorServices _colorServices;
        private readonly INameTagServices _nameTagServices;
        private readonly ISizeServices _sizeServices;
        private readonly IAuditLogServices _logger;
        public AdminController(
            UserManager<User> userManager,
            EmailService emailService,
            IConfiguration config,
            ApplicationDbContext context,
            IMapper mapper,
            UserServices userServices,
            IProductServices productServices,
            ImageServices imageServices,
            IBrandServices brandServices,
            StaffServices staffServices,
            IColorServices colorServices,
            INameTagServices nameTagServices,
            ISizeServices sizeServices,
            IAuditLogServices logger)
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
            _colorServices = colorServices;
            _nameTagServices = nameTagServices;
            _sizeServices = sizeServices;
            _logger = logger;
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
        public async Task<IActionResult> GetBrands()
        {
            var user = await _userServices.GetCurrentUserAsync();
            try
            {
                var brands = await _brandServices.GetBrands();
                if (brands == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new ResponseView()
                    {
                        Success = false,
                        Message = "Not have brand in list"
                    });
                }
                var brandDto = _mapper.Map<List<BrandDto>>(brands);
                var totalCount = brandDto.Count();
                var paginateData = new PaginateDataView<BrandDto>()
                {
                    ListData = brandDto,
                    totalCount = totalCount
                };
                return StatusCode(StatusCodes.Status200OK, new ResponseView<PaginateDataView<BrandDto>>
                {
                    Success = true,
                    Data = paginateData,
                    Message = "Retrive brand successfully !"
                });
            }
            catch (Exception e)
            {
                await _logger.LogActionAsync(user, "Get brands", "Brand", null, e.ToString(), Serilog.Events.LogEventLevel.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "SERVER_ERROR",
                        Message = "Error retrieving brand !"
                    }
                });
            }
        }

        [HttpGet("get-brand/{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var user = await _userServices.GetCurrentUserAsync();
            try
            {
                var brand = await _brandServices.GetBrandById(id);
                if (brand == null)
                {
                    await _logger.LogActionAsync(user, "Get Brand By Id", "Brand", null, $"Not found brand have id {id}", Serilog.Events.LogEventLevel.Warning);
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView()
                        {
                            Code = "NOT_FOUND",
                            Message = "Brand not found !"
                        }
                    });
                }
                var brandDto = _mapper.Map<BrandDto>(brand);
                return StatusCode(StatusCodes.Status200OK, new ResponseView<BrandDto>()
                {
                    Success = true,
                    Data = brandDto,
                    Message = "Get brand successfully !"
                });
            }
            catch (Exception e)
            {
                await _logger.LogActionAsync(user, "Get brand by id", "Brand", null, e.ToString(), Serilog.Events.LogEventLevel.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "SERVER_ERROR",
                        Message = "An occured error when get brand !"
                    }
                });
            }
        }

        [HttpPost("create-brand")]
        public async Task<IActionResult> CreateBrand([FromForm] CreateBrandDto model)
        {
            var user = await _userServices.GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                await _logger.LogActionAsync(user, "Create Brand", "Brand", null, "Model state invalid", Serilog.Events.LogEventLevel.Warning);
                var err = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var respone = new ErrorViewForModelState()
                {
                    Success = false,
                    Error = new ErrorModelStateView()
                    {
                        Code = "INVALID_INPUT",
                        Errors = err
                    }
                };
                return BadRequest(respone);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!_imageServices.ProcessImageExtension(model.Image))
                    {
                        await _logger.LogActionAsync(user, "Create Brand", "Brand", null, "Invalid image format", Serilog.Events.LogEventLevel.Warning);
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseView()
                        {
                            Success = false,
                            Error = new ErrorView
                            {
                                Code = "INVALID_DATA",
                                Message = "Invalid image format. Only JPG, JPEG, and PNG files are allowed."
                            }
                        });
                    }
                    string filePath = await _imageServices.CreatePathForImg("brands", model.Image);
                    if (await _context.IsExistsAsync<Brand>("BrandName", model.BrandName))
                    {
                        await _logger.LogActionAsync(user, "Create Brand", "Brand", null, $"Brand name {model.BrandName} is dupplicate", Serilog.Events.LogEventLevel.Warning);
                        var message = $"Brand name {model.BrandName} has been exist, please try with another name";
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseView
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
                    var brand = await _brandServices.CreateBrandAsync(model, user, filePath);
                    var brandDto = _mapper.Map<BrandDto>(brand);
                    await transaction.CommitAsync();
                    await _logger.LogActionAsync(user, "Create Brand", "Brand", brandDto.BrandId.ToString(), null);
                    return StatusCode(StatusCodes.Status201Created, new ResponseView<Brand>()
                    {
                        Success = true,
                        Message = "Brand Created Successfully",
                        Data = brand
                    });
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    await _logger.LogActionAsync(user, "Create Brand", "Brand", null, e.ToString(), Serilog.Events.LogEventLevel.Error);
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView()
                        {
                            Code = "SERVER_ERROR",
                            Message = "An occured error while creating brand !"
                        }
                    });
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
            var user = await _userServices.GetCurrentUserAsync();
            try
            {
                var colors = await _colorServices.GetColors();
                if (colors.Count() == 0 || colors == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, new ResponseView<List<ColorDto>>()
                    {
                        Success = false,
                        Data = null,
                        Message = "Not have color in list"
                    });
                }
                var colorDto = _mapper.Map<List<ColorDto>>(colors);
                return StatusCode(StatusCodes.Status200OK, new ResponseView<List<ColorDto>>()
                {
                    Success = true,
                    Data = colorDto,
                    Message = "Retrive color successfull !"
                });
            }
            catch (Exception e)
            {
                await _logger.LogActionAsync(user, "Get Colors", "Color", null, e.ToString(), Serilog.Events.LogEventLevel.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "SERVER_ERROR",
                        Message = "Error retrieving colors !"
                    }
                });
            }
        }

        [HttpGet("get-color/{id}")]
        public async Task<IActionResult> GetColorById(int id)
        {
            var user = await _userServices.GetCurrentUserAsync();
            try
            {
                var color = await _colorServices.GetColorById(id);
                if (color == null)
                {
                    await _logger.LogActionAsync(user, "Get Color by id", "Color", null, $"Not found color have color id {id}", Serilog.Events.LogEventLevel.Warning);
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView()
                        {
                            Code = "NOT_FOUND",
                            Message = "Color not found !"
                        }
                    });
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseView<Models.Color>()
                {
                    Success = true,
                    Message = "Retrived color successfully",
                    Data = color
                });
            }
            catch (Exception e)
            {
                await _logger.LogActionAsync(user, "Get Color by id", "Color", null, e.ToString(), Serilog.Events.LogEventLevel.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "SERVER_ERROR",
                        Message = "Error retrieving color !"
                    }
                });
            }
        }

        [HttpPost("create-color")]
        public async Task<IActionResult> CreateColor(CreateColorDto model)
        {
            var user = await _userServices.GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                await _logger.LogActionAsync(user, "Create Color", "Color", null, "Invalid model state", Serilog.Events.LogEventLevel.Warning);
                var err = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var respone = new ErrorViewForModelState()
                {
                    Success = false,
                    Error = new ErrorModelStateView()
                    {
                        Code = "INVALID_INPUT",
                        Errors = err
                    }
                };
                return BadRequest(respone);
            }
            string message;
            if (await _context.IsExistsAsync<Models.Color>("ColorName", model.ColorName))
            {
                message = $"Color name {model.ColorName} has been exist, please try with another name";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseView
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var color = await _colorServices.CreateColorAsync(model, user);
                    await transaction.CommitAsync();
                    // await _system.Log("Color", "Create", null, color, user);
                    return StatusCode(StatusCodes.Status201Created, new ResponseView<Models.Color>
                    {
                        Success = true,
                        Data = color,
                        Message = "Color created successfully !"
                    });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView()
                        {
                            Code = "SERVER_ERROR",
                            Message = "An unexpected error occurred while creating the color"
                        }
                    });
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
            var nametags = await _nameTagServices.GetNameTags();
            if (nametags.Count() == 0 || nametags == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, new ResponseView<List<NameTagDto>>()
                {
                    Success = false,
                    Data = null,
                    Message = "Not have name tag in list"
                });
            }
            var nameTagDtos = _mapper.Map<List<NameTagDto>>(nametags);
            var result = new ResponseView<List<NameTagDto>>()
            {
                Success = true,
                Data = nameTagDtos,
                Message = "Retrive name tag successfull !"
            };
            return Ok(result);
        }

        [HttpGet("get-name-tag/{id}")]
        public async Task<IActionResult> GetNameTagById(int id)
        {
            var nameTag = await _nameTagServices.GetNameTagById(id);
            if (nameTag == null) return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
            {
                Success = false,
                Error = new ErrorView()
                {
                    Code = "NOT_FOUND",
                    Message = "Color not found !"
                }
            });
            var result = new ResponseView<NameTag>()
            {
                Success = true,
                Message = "Get color successfully",
                Data = nameTag
            };
            return Ok(result);
        }

        [HttpPost("create-name-tag")]
        public async Task<IActionResult> CreateNameTag(NameTagDto model)
        {
            if (!ModelState.IsValid)
            {
                var err = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var respone = new ErrorViewForModelState()
                {
                    Success = false,
                    Error = new ErrorModelStateView()
                    {
                        Code = "INVALID_INPUT",
                        Errors = err
                    }
                };
                return BadRequest(respone);
            }
            var message = "";
            var user = await _userServices.GetCurrentUserAsync();
            if (user == null) return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
            {
                Success = false,
                Error = new ErrorView()
                {
                    Code = "NOT_FOUND",
                    Message = "User not found !"
                }
            });
            if (await _context.IsExistsAsync<NameTag>("Tag", model.TagName))
            {
                message = $"Tag name {model.TagName} has been exist, please try with another name";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseView
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var nameTag = await _nameTagServices.CreateNameTagAsync(model, user);
                    var result = new ResponseView<NameTag>()
                    {
                        Success = true,
                        Data = nameTag,
                        Message = "Create name tag successfully !"
                    };
                    await transaction.CommitAsync();
                    // await _system.Log("Tag", "Create", null, nameTag, user);
                    return Ok(result);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView()
                        {
                            Code = "SERVER_ERROR",
                            Message = "Have an occured error while create name tag !"
                        }
                    });
                }
            }
        }
        #endregion

        #region Size Manage Function
        [HttpGet("get-sizes")]
        public async Task<IActionResult> GetSizes()
        {
            var sizes = await _sizeServices.GetSizes();
            if (sizes.Count() == 0 || sizes == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, new ResponseView<List<SizeDto>>()
                {
                    Success = false,
                    Data = null,
                    Message = "Not have size in list"
                });
            }
            var sizeDtos = _mapper.Map<List<SizeDto>>(sizes);
            var result = new ResponseView<List<SizeDto>>()
            {
                Success = true,
                Data = sizeDtos,
                Message = "Retrive size successfull !"
            };
            return Ok(result);
        }

        [HttpGet("get-size/{id}")]
        public async Task<IActionResult> GetSizeById(int id)
        {
            var size = await _sizeServices.GetSizeById(id);
            if (size == null) return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
            {
                Success = false,
                Error = new ErrorView()
                {
                    Code = "NOT_FOUND",
                    Message = "Size not found !"
                }
            });
            var result = new ResponseView<Models.Size>()
            {
                Success = true,
                Message = "Get size successfully",
                Data = size
            };
            return Ok(result);
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
                await query
                    .OrderBy(p => p.ProductId)
                    .Include(p => p.Brand)
                    .Include(p => p.NameTags)
                    .ThenInclude(nt => nt.NameTag)
                    .Include(p => p.ProductColor)
                    .Where(p => p.Status == true)
                    .Skip((pageNumberValue - 1) * pageSizeValue)
                    .Take(pageSizeValue)
                    .ToListAsync();
                var totalProducts = await query.CountAsync();
                var productDtos = _mapper.Map<List<ListProductDto>>(query);
                var paginateData = new PaginateDataView<ListProductDto>()
                {
                    ListData = productDtos,
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
                        Code = "SERVER_ERROR",
                        Message = "Error retrieving products !"
                    }
                });
            }
        }

        [HttpGet("get-product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productServices.GetProductById(id);
            if (product == null) return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
            {
                Success = false,
                Error = new ErrorView()
                {
                    Code = "NOT_FOUND",
                    Message = "Product not found !"
                }
            });
            var productDto = _mapper.Map<ProductDto>(product);
            var result = new ResponseView<ProductDto>()
            {
                Success = true,
                Message = "Get product successfully",
                Data = productDto
            };

            return Ok(result);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto model)
        {
            var user = await _userServices.GetCurrentUserAsync();
            if (!ModelState.IsValid)
            {
                var err = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var respone = new ErrorViewForModelState()
                {
                    Success = false,
                    Error = new ErrorModelStateView()
                    {
                        Code = "INVALID_INPUT",
                        Errors = err
                    }
                };
                return BadRequest(respone);
            }
            string message;
            if (user == null)
            {
                message = "User not found !";
                return StatusCode(StatusCodes.Status404NotFound, new ResponseView
                {
                    Success = false,
                    Message = message,
                    Error = new ErrorView
                    {
                        Code = "NOT_FOUND",
                        Message = message
                    }
                });
            }
            if (await _context.IsExistsAsync<Product>("ProductName", model.ProductName))
            {
                message = $"Product name {model.ProductName} has been exist, please try with another name";
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseView
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
                message = "Brand does not exist, please try again!";
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
                            message = $"Inavalid color id {variant.ColorId}";
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
                                message = $"Inavalid size id {size.SizeId}";
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
                    // await _system.Log("Product", "Create", null, product, user);
                    return StatusCode(StatusCodes.Status200OK, new ResponseView<Product>
                    {
                        Success = true,
                        Message = "Product Created Successfully",
                        Data = product

                    });
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView
                        {
                            Code = "SERVER_ERROR",
                            Message = "An error occurred while adding the product !"
                        }
                    });
                }
            }
        }

        [HttpPost("change-product-status/{id}")]
        public async Task<IActionResult> ChangeProductStatus(int id)
        {
            var user = await _userServices.GetCurrentUserAsync();
            if (!await _context.IsExistsAsync<Product>("ProductId", id))
            {
                await _logger.LogActionAsync(user, "Change Status", "Product", null, $"Not found product have product id = {id}", Serilog.Events.LogEventLevel.Warning);
                return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
                {
                    Success = false,
                    Error = new ErrorView()
                    {
                        Code = "NOT_FOUND",
                        Message = "Product not found !"
                    }
                });
            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                    product.Status = product.Status ? false : true;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    await _logger.LogActionAsync(user, "Change status", "Product", product.ProductId.ToString(), null, Serilog.Events.LogEventLevel.Information);
                    var response = new ResponseView()
                    {
                        Success = true,
                        Message = "Change product status successfully"
                    };
                    return Ok(response);
                }
                catch (Exception e)
                {
                    await _logger.LogActionAsync(user, "Change status", "Product", null, e.ToString(), Serilog.Events.LogEventLevel.Error);
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseView()
                    {
                        Success = false,
                        Error = new ErrorView()
                        {
                            Code = "SERVER_ERROR",
                            Message = "Have an occured when change product status"
                        }
                    });
                }

            }
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
