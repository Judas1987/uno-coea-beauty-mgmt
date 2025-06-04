using AutoMapper;
using SalonManagement.Api.Models.Services;
using SalonManagement.Dal.Dtos;
using SalonManagement.Dal.Entities;

namespace SalonManagement.Api.Mappings;

public class ServiceMappingProfile : Profile
{
    public ServiceMappingProfile()
    {
        // Request > DTO
        CreateMap<CreateServiceRequest, ServiceDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.IsPromotional, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.PromotionalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        CreateMap<UpdateServiceRequest, ServiceDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.IsPromotional, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        // Entity <-> DTO
        CreateMap<Service, ServiceDto>();
        CreateMap<ServiceDto, Service>()
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.ServicePackages, opt => opt.Ignore());

        // Category mappings
        CreateMap<ServiceCategory, ServiceCategoryDto>();
        CreateMap<ServiceCategoryDto, ServiceCategory>()
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        // DTO > ViewModel
        CreateMap<ServiceDto, ServiceViewModel>()
            .ForMember(dest => dest.CategoryTitle,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Title : string.Empty))
            .ForMember(dest => dest.IsPromotional,
                opt => opt.MapFrom(src => src.IsPromotional ?? false));
    }
}
