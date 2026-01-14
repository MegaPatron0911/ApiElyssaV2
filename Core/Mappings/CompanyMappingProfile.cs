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
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Id));

        CreateMap<CompanyDto, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Country, opt => opt.Ignore())
            .ForMember(dest => dest.LegalTextDelivery, opt => opt.Ignore())
            .ForMember(dest => dest.LegalTextRecruiment, opt => opt.Ignore())
            .ForMember(dest => dest.LegalTextReturn, opt => opt.Ignore())
            .ForMember(dest => dest.LegalTextNews, opt => opt.Ignore());
    }
}
