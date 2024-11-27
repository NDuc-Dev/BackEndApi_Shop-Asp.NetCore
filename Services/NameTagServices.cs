using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.NameTag;
using WebIdentityApi.Interfaces;
using WebIdentityApi.Models;

namespace WebIdentityApi.Services
{
    public class NameTagServices : INameTagServices
    {
        private readonly ApplicationDbContext _context;
        public NameTagServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<NameTag>> GetNameTags()
        {
            return await _context.NameTags.ToListAsync();
        }
        public async Task<NameTag> GetNameTagById(int id)
        {
            return await _context.NameTags.FirstAsync(nt => nt.NameTagId == id);
        }
        public async Task<NameTag> CreateNameTagAsync(NameTagDto model, User user)
        {
            var tag = new NameTag
            {
                Tag = model.TagName,
                CreateBy = user
            };
            await _context.AddAsync(tag);
            await _context.SaveChangesAsync();

            return tag;
        }
    }
}