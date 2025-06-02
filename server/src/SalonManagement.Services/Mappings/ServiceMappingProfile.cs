using AutoMapper;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Services.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        CreateMap<ServiceDto, Service>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.ServicePackages, opt => opt.Ignore());

        CreateMap<Service, ServiceDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
    }
} 