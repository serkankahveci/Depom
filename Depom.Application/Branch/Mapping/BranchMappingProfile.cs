using AutoMapper;
using Depom.Application.Branch.DTOs;

namespace Depom.Application.Branch.Mapping;

public class BranchMappingProfile : Profile
{
    public BranchMappingProfile()
    {
        CreateMap<Domain.Entities.Branch, BranchListDto>();
        CreateMap<Domain.Entities.Branch, BranchDetailDto>();
        CreateMap<BranchCreateDto, Domain.Entities.Branch>();
        CreateMap<BranchUpdateDto, Domain.Entities.Branch>();
    }
}
