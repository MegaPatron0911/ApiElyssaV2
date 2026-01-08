using AutoMapper;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Mappings;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        // Company -> CompanyDto
        CreateMap<Company, CompanyDto>();

        // CompanyDto -> Company
        CreateMap<CompanyDto, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
