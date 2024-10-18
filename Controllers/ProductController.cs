using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Product;
using Microsoft.EntityFrameworkCore;

namespace WebIdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProduct()
        {
            var product = await _context.Products.ToArrayAsync();
            return Ok(product);
        }
    }
}
