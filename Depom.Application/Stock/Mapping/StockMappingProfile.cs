using AutoMapper;
using Depom.Application.Stock.DTOs;
using Depom.Domain.Entities;

namespace Depom.Application.Stock.Mapping;

public class StockMappingProfile : Profile
{
    public StockMappingProfile()
    {
        CreateMap<StockItem, StockItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.SKU, o => o.MapFrom(s => s.Product.SKU))
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch.Name))
            .ForMember(d => d.LowStockThreshold, o => o.MapFrom(s => s.Product.LowStockThreshold))
            .ForMember(d => d.AvailableQuantity, o => o.MapFrom(s => s.Quantity - s.ReservedQuantity));

        CreateMap<StockMovement, StockMovementListDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.CreatedByUserName, o => o.MapFrom(s => s.CreatedByUser.FullName));
    }
}
