using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using WebIdentityApi.Filters;
using System;

namespace WebIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilters filter, int skip, int take)
        {
            if (skip < 0 || take <= 0)
            {
                return BadRequest("Invalid skip or take value");
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
                    .Where(p => p.Status == true).Skip(skip)
                    .Take(take)
                    .ToListAsync();

                var productDtos = _mapper.Map<List<ListProductDto>>(query);
                var hasMore = skip + take < totalProducts;
                return Ok(new { data = productDtos, hasMore });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
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
    }
}
