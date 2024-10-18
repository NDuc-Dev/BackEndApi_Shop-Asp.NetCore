using AutoMapper;
using WebIdentityApi.DTOs.Product;
using WebIdentityApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.NameTags, opt => opt.Ignore())
            .ForMember(dest => dest.ProductColor, opt => opt.Ignore());
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    }
}