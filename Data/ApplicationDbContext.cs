using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebIdentityApi.Models;

namespace WebIdentityApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Brand>()
                .HasMany(p => p.Products)
                .WithOne(pv => pv.Brand)
                .HasForeignKey(pv => pv.BrandId);
            builder.Entity<Product>()
                .HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId);
            builder.Entity<Size>()
                .HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Size)
                .HasForeignKey(pv => pv.SizeId);
            builder.Entity<Color>()
                .HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Color)
                .HasForeignKey(pv => pv.ColorId);
            builder.Entity<Product>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreateBy);

            base.OnModelCreating(builder);
            this.SeedRole(builder);
        }

        private void SeedRole(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "Staff", ConcurrencyStamp = "2", NormalizedName = "Staff" },
                new IdentityRole() { Name = "Customer", ConcurrencyStamp = "3", NormalizedName = "Customer" }
                );
        }
    }
}
