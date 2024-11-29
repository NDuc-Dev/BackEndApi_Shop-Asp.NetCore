using System.Linq;
using AutoMapper;
using WebIdentityApi.Data;
using WebIdentityApi.DTOs.Brand;
using WebIdentityApi.DTOs.Color;
using WebIdentityApi.DTOs.NameTag;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.DTOs.ProductColor;
using WebIdentityApi.DTOs.ProductColorSize;
using WebIdentityApi.Models;

public class MappingProfile : Profile
{
    private readonly ApplicationDbContext _context;
    public MappingProfile(ApplicationDbContext context)
    {
        _context = context;
    }
    public MappingProfile()
    {
        CreateMap<Product, Product>();

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.NameTags.Select(nt => nt.NameTag)))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Variant, opt => opt.MapFrom(src => src.ProductColor));

        CreateMap<NameTag, NameTagDto>()
            .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.NameTagId))
            .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Tag));

        CreateMap<ProductColor, ProductColorDto>()
            .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName))
            .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ProductColorId, opt => opt.MapFrom(src => src.ProductColorId))
            .ForMember(dest => dest.ProductColorSize, opt => opt.MapFrom(src => src.ProductColorSizes))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList()));


        CreateMap<ProductColorDto, ProductColor>()
            .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.ProductColorId, opt => opt.MapFrom(src => src.ProductColorId))
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Color, opt => opt.Ignore());

        CreateMap<ProductColorSize, ProductColorSizeDto>()
            .ForMember(dest => dest.ProductColorSizeId, opt => opt.MapFrom(src => src.ProductColorSizeId))
            .ForMember(dest => dest.ProductColorId, opt => opt.MapFrom(src => src.ProductColorId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.SizeValue, opt => opt.MapFrom(src => src.Size.SizeValue))
            .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.Size.SizeId));


        CreateMap<Product, ListProductDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.NameTags.Select(t => t.NameTag)))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ProductColor.Select(pc => pc.ImagePath.Split(';', System.StringSplitOptions.RemoveEmptyEntries).First()).First()));

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
            .ForMember(dest => dest.Brand, opt => opt.Ignore());


        CreateMap<Brand, BrandDto>()
            .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descriptions))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath));

        CreateMap<Color, ColorDto>()
            .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
            .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ColorName));

    }
}