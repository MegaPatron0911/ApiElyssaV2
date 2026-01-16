using AutoMapper;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Mappings;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));

        CreateMap<Country, CountryInfoDto>()
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId));

        CreateMap<CompanyDto, Company>()
            .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
            .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
            .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
            .ForMember(dest => dest.Country, opt => opt.Ignore());
    }
}
