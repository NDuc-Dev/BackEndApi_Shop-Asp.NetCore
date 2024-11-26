﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebIdentityApi.Data;

#nullable disable

namespace WebIdentityApi.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241126101600_Seeding_DB")]
    partial class Seeding_DB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "5614873d-2870-4b09-91b7-a98140fb27e3",
                            ConcurrencyStamp = "1",
                            Name = "Admin",
                            NormalizedName = "Admin"
                        },
                        new
                        {
                            Id = "d2d9a920-584d-4181-9bda-fc51ee0416c4",
                            ConcurrencyStamp = "2",
                            Name = "Staff",
                            NormalizedName = "Staff"
                        },
                        new
                        {
                            Id = "e6878637-8822-4624-8c94-0bd66296c963",
                            ConcurrencyStamp = "3",
                            Name = "Customer",
                            NormalizedName = "Customer"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("WebIdentityApi.Models.ActionDetail", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataAfter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataBefore")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HandleAt")
                        .HasColumnType("datetime");

                    b.Property<string>("HandleByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HandleTable")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserHandle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("HandleByUserId");

                    b.ToTable("ActionDetails");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandId"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Descriptions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BrandId");

                    b.HasIndex("CreateByUserId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Color", b =>
                {
                    b.Property<int>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ColorId"));

                    b.Property<string>("ColorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ColorId");

                    b.HasIndex("CreateByUserId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("WebIdentityApi.Models.NameTag", b =>
                {
                    b.Property<int>("NameTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NameTagId"));

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NameTagId");

                    b.HasIndex("CreateByUserId");

                    b.ToTable("NameTags");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(9,0)");

                    b.Property<string>("OrderBy")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("OrderType")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("OrderBy");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebIdentityApi.Models.OrderDetails", b =>
                {
                    b.Property<int>("DetailsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DetailsId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductColorSizeId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(9,0)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(9,0)");

                    b.HasKey("DetailsId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductColorSizeId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("ProductId");

                    b.HasIndex("BrandId");

                    b.HasIndex("CreateByUserId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductColor", b =>
                {
                    b.Property<int>("ProductColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductColorId"));

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(9,0)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("ProductColorId");

                    b.HasIndex("ColorId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductColors");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductColorSize", b =>
                {
                    b.Property<int>("ProductColorSizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductColorSizeId"));

                    b.Property<int>("ProductColorId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.HasKey("ProductColorSizeId");

                    b.HasIndex("ProductColorId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductColorSizes");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductNameTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreateById")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NameTagId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreateById");

                    b.HasIndex("NameTagId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductNameTags");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<string>("CreateByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SizeValue")
                        .HasColumnType("int");

                    b.HasKey("SizeId");

                    b.HasIndex("CreateByUserId");

                    b.ToTable("Sizes");

                    b.HasData(
                        new
                        {
                            SizeId = 1,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4212),
                            SizeValue = 36
                        },
                        new
                        {
                            SizeId = 2,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4250),
                            SizeValue = 37
                        },
                        new
                        {
                            SizeId = 3,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4252),
                            SizeValue = 38
                        },
                        new
                        {
                            SizeId = 4,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4253),
                            SizeValue = 39
                        },
                        new
                        {
                            SizeId = 5,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4254),
                            SizeValue = 40
                        },
                        new
                        {
                            SizeId = 6,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4255),
                            SizeValue = 41
                        },
                        new
                        {
                            SizeId = 7,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4256),
                            SizeValue = 42
                        },
                        new
                        {
                            SizeId = 8,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4257),
                            SizeValue = 43
                        },
                        new
                        {
                            SizeId = 9,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4258),
                            SizeValue = 44
                        },
                        new
                        {
                            SizeId = 10,
                            CreateDate = new DateTime(2024, 11, 26, 17, 16, 0, 130, DateTimeKind.Local).AddTicks(4259),
                            SizeValue = 45
                        });
                });

            modelBuilder.Entity("WebIdentityApi.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("AccountStatus")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SpendingPoint")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalSpending")
                        .HasColumnType("decimal(9,0)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebIdentityApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebIdentityApi.Models.ActionDetail", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "HandleBy")
                        .WithMany("Actions")
                        .HasForeignKey("HandleByUserId");

                    b.Navigation("HandleBy");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Brand", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "CreatedByUser")
                        .WithMany("CreatedBrands")
                        .HasForeignKey("CreateByUserId");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Color", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "CreateBy")
                        .WithMany("CreatedColors")
                        .HasForeignKey("CreateByUserId");

                    b.Navigation("CreateBy");
                });

            modelBuilder.Entity("WebIdentityApi.Models.NameTag", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "CreateBy")
                        .WithMany("CreatedTags")
                        .HasForeignKey("CreateByUserId");

                    b.Navigation("CreateBy");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Order", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "OrderByUser")
                        .WithMany("CreatedOrders")
                        .HasForeignKey("OrderBy");

                    b.Navigation("OrderByUser");
                });

            modelBuilder.Entity("WebIdentityApi.Models.OrderDetails", b =>
                {
                    b.HasOne("WebIdentityApi.Models.Order", "Order")
                        .WithMany("Details")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebIdentityApi.Models.ProductColorSize", "ProductColorSize")
                        .WithMany("Details")
                        .HasForeignKey("ProductColorSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("ProductColorSize");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Product", b =>
                {
                    b.HasOne("WebIdentityApi.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebIdentityApi.Models.User", "CreatedByUser")
                        .WithMany("CreatedProducts")
                        .HasForeignKey("CreateByUserId");

                    b.Navigation("Brand");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductColor", b =>
                {
                    b.HasOne("WebIdentityApi.Models.Color", "Color")
                        .WithMany("ProductColor")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebIdentityApi.Models.Product", "Product")
                        .WithMany("ProductColor")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductColorSize", b =>
                {
                    b.HasOne("WebIdentityApi.Models.ProductColor", "ProductColor")
                        .WithMany("ProductColorSizes")
                        .HasForeignKey("ProductColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebIdentityApi.Models.Size", "Size")
                        .WithMany("ProductColorSize")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductColor");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductNameTag", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "CreateBy")
                        .WithMany()
                        .HasForeignKey("CreateById");

                    b.HasOne("WebIdentityApi.Models.NameTag", "NameTag")
                        .WithMany("ProductTags")
                        .HasForeignKey("NameTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebIdentityApi.Models.Product", "Product")
                        .WithMany("NameTags")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreateBy");

                    b.Navigation("NameTag");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Size", b =>
                {
                    b.HasOne("WebIdentityApi.Models.User", "CreateBy")
                        .WithMany("CreatedSizes")
                        .HasForeignKey("CreateByUserId");

                    b.Navigation("CreateBy");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Color", b =>
                {
                    b.Navigation("ProductColor");
                });

            modelBuilder.Entity("WebIdentityApi.Models.NameTag", b =>
                {
                    b.Navigation("ProductTags");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Order", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Product", b =>
                {
                    b.Navigation("NameTags");

                    b.Navigation("ProductColor");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductColor", b =>
                {
                    b.Navigation("ProductColorSizes");
                });

            modelBuilder.Entity("WebIdentityApi.Models.ProductColorSize", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("WebIdentityApi.Models.Size", b =>
                {
                    b.Navigation("ProductColorSize");
                });

            modelBuilder.Entity("WebIdentityApi.Models.User", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("CreatedBrands");

                    b.Navigation("CreatedColors");

                    b.Navigation("CreatedOrders");

                    b.Navigation("CreatedProducts");

                    b.Navigation("CreatedSizes");

                    b.Navigation("CreatedTags");
                });
#pragma warning restore 612, 618
        }
    }
}