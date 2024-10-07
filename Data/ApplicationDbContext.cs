using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebIdentityApi.Models;

namespace WebIdentityApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<NameTag> NameTags { get; set; }
        public DbSet<ProductNameTag> ProductNameTags { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductColorSize> ProductColorSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Brand>()
                .HasOne(b => b.CreatedByUser)
                .WithMany(u => u.CreatedBrands)
                .HasForeignKey(b => b.CreateByUserId);
            builder.Entity<Brand>()
                .HasMany(p => p.Products)
                .WithOne(pv => pv.Brand)
                .HasForeignKey(pv => pv.BrandId);

            builder.Entity<Product>()
                .HasMany(p => p.ProductColor)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId);
            builder.Entity<Product>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.CreatedProducts)
                .HasForeignKey(p => p.CreateByUserId);

            builder.Entity<Order>()
                .HasOne(o => o.OrderByUser)
                .WithMany(u => u.CreatedOrders)
                .HasForeignKey(o => o.OrderBy);

            builder.Entity<ProductNameTag>()
                .HasOne(pnt => pnt.Product)
                .WithMany(p => p.NameTags)
                .HasForeignKey(pnt => pnt.ProductId);
            builder.Entity<ProductNameTag>()
                .HasOne(pnt => pnt.NameTag)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(pnt => pnt.NameTagId);

            builder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithMany(o => o.Details)
                .HasForeignKey(od => od.OrderId);
            builder.Entity<OrderDetails>()
                .HasOne(od => od.ProductColorSize)
                .WithMany(p => p.Details)
                .HasForeignKey(od => od.ProductColorSizeId);

            builder.Entity<Size>()
                .HasMany(p => p.ProductColorSize)
                .WithOne(pv => pv.Size)
                .HasForeignKey(pv => pv.SizeId);

            builder.Entity<Color>()
                .HasMany(p => p.ProductColor)
                .WithOne(c => c.Color)
                .HasForeignKey(pv => pv.ColorId);

            builder.Entity<ProductColor>()
                .HasMany(pcs => pcs.ProductColorSizes)
                .WithOne(pc => pc.ProductColor)
                .HasForeignKey(pcs => pcs.ProductColorSizeId);



            base.OnModelCreating(builder);
            SeedRole(builder);
            SeedSize(builder);
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
        private void SeedSize(ModelBuilder builder)
        {
            builder.Entity<Size>().HasData(
                new Size() { SizeId = 1, SizeValue = 36 },
                new Size() { SizeId = 2, SizeValue = 37 },
                new Size() { SizeId = 3, SizeValue = 38 },
                new Size() { SizeId = 4, SizeValue = 39 },
                new Size() { SizeId = 5, SizeValue = 40 },
                new Size() { SizeId = 6, SizeValue = 41 },
                new Size() { SizeId = 7, SizeValue = 42 },
                new Size() { SizeId = 8, SizeValue = 43 },
                new Size() { SizeId = 9, SizeValue = 44 },
                new Size() { SizeId = 10, SizeValue = 45 }
                );
        }
    }
}
