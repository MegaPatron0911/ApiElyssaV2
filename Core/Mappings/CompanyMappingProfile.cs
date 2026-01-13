using AutoMapper;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Mappings;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Company, CompanyDto>();

        CreateMap<CompanyDto, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.BusinessName, opt => opt.Ignore())
            .ForMember(dest => dest.Nit, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.PlanType, opt => opt.Ignore());
    }
}
