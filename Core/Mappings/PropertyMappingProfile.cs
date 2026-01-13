using AutoMapper;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Mappings;

public class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        CreateMap<Property, PropertyResponse>()
            .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.HasInventories, opt => opt.Ignore());

        CreateMap<PropertyType, PropertyTypeResponse>();

        CreateMap<Property, PropertyDetailResponse>()
            .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new PropertyLocationDto
            {
                Latitude = src.Latitude,
                Longitude = src.Longitude
            }))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => new PropertyTypeDetailDto
            {
                Id = src.PropertyType!.Id,
                Name = src.PropertyType.Name
            }))
            .ForMember(dest => dest.Stats, opt => opt.Ignore());
    }
}
