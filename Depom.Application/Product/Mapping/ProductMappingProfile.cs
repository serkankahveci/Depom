using AutoMapper;
using Depom.Application.Product.DTOs;

namespace Depom.Application.Product.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Domain.Entities.Product, ProductListDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));

        CreateMap<Domain.Entities.Product, ProductDetailDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));

        CreateMap<ProductCreateDto, Domain.Entities.Product>();
        CreateMap<ProductUpdateDto, Domain.Entities.Product>();
    }
}
