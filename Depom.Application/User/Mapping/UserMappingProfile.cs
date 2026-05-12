using AutoMapper;
using Depom.Application.User.DTOs;
using Depom.Domain.Entities;

namespace Depom.Application.User.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Entity -> DTO
        CreateMap<AppUser, UserListDto>()
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch != null ? s.Branch.Name : null));

        CreateMap<AppUser, UserDetailDto>()
            .ForMember(d => d.BranchName, o => o.MapFrom(s => s.Branch != null ? s.Branch.Name : null));

        // DTO -> Entity (ÅŸifre alanlarÄ± UserService iÃ§inde ayrÄ±ca ele alÄ±nÄ±r)
        CreateMap<UserCreateDto, AppUser>()
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.CreatedAt,    o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastLoginAt,  o => o.Ignore());

        CreateMap<UserUpdateDto, AppUser>()
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.CreatedAt,    o => o.Ignore())
            .ForMember(d => d.LastLoginAt,  o => o.Ignore());
    }
}