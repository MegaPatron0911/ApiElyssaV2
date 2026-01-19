using AutoMapper;
using Elyssa.Core.Domain.Entities;
using Elyssa.Core.DTOs;

namespace Elyssa.Core.Mappings;

public class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        CreateMap<Property, PropertyResponseDto>()
            .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.PropertyId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModificationDate))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => new PropertyTypeResponseDto  
            {
                Id = src.PropertyTypeId,  
                Name = src.PropertyType != null ? src.PropertyType.typeName : string.Empty
            }))
            .ForMember(dest => dest.HasInventories, opt => opt.Ignore());

        CreateMap<PropertyType, PropertyTypeResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PropertyTypeId))  
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.typeName));

        CreateMap<Property, PropertyDetailResponseDto>()
            .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.PropertyId))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModificationDate))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new PropertyLocationDto
            {
                Latitude = src.Latitude,
                Longitude = src.Longitude
            }))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => new PropertyTypeDetailDto
            {
                Id = src.PropertyTypeId,
                Name = src.PropertyType != null ? src.PropertyType.typeName : string.Empty
            }))
            .ForMember(dest => dest.Stats, opt => opt.Ignore());
    }
}