using System.Linq;
using AutoMapper;
using WebIdentityApi.DTOs.NameTag;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.DTOs.ProductColor;
using WebIdentityApi.DTOs.ProductColorSize;
using WebIdentityApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.NameTags.Select(nt => nt.NameTag.Tag)))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Variant, opt => opt.MapFrom(src => src.ProductColor));

        CreateMap<NameTag, NameTagDto>()
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


        CreateMap<Product, ProductGetListDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.NameTags.Select(nt => nt.NameTag.Tag)))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.BrandName))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ProductColor.Select(pc => pc .ImagePath.Split(';', System.StringSplitOptions.RemoveEmptyEntries).First()).First()));

    }
}