using AutoMapper;
using Depom.Application.Transfer.DTOs;
using Depom.Domain.Entities;

namespace Depom.Application.Transfer.Mapping;

public class TransferMappingProfile : Profile
{
    public TransferMappingProfile()
    {
        CreateMap<Domain.Entities.Transfer, TransferListDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.SourceBranchName, o => o.MapFrom(s => s.SourceBranch.Name))
            .ForMember(d => d.TargetBranchName, o => o.MapFrom(s => s.TargetBranch.Name));

        CreateMap<Domain.Entities.Transfer, TransferDetailDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
            .ForMember(d => d.SourceBranchName, o => o.MapFrom(s => s.SourceBranch.Name))
            .ForMember(d => d.TargetBranchName, o => o.MapFrom(s => s.TargetBranch.Name))
            .ForMember(d => d.CreatedByUserName, o => o.MapFrom(s => s.CreatedByUser.FullName));

        CreateMap<TransferLog, TransferLogDto>()
            .ForMember(d => d.ActingUserName, o => o.MapFrom(s => s.ActingUser.FullName));

        CreateMap<TransferCreateDto, Domain.Entities.Transfer>();
    }
}
